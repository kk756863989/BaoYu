using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CC
{
    public enum AssetType
    {
        PREFAB = 1,
        DATA = 2,
        GUI = 3,
        ATLAS = 4
    }

    public static class AssetsLoad
    {
        private static CCProgress progress = new CCProgress(0, 0);
        private static readonly Loader resourceLoader;
        public static readonly Dictionary<string, GameObject> guiSources = new Dictionary<string, GameObject>();
        public static readonly Dictionary<string, GameObject> atlasSources = new Dictionary<string, GameObject>();
        public static readonly Dictionary<string, CCMap> dataSources = new Dictionary<string, CCMap>();
        public static readonly Dictionary<string, Sprite> spriteSources = new Dictionary<string, Sprite>();

        static AssetsLoad()
        {
           // Debug.Log("AssetLib Awake");
            resourceLoader = new GameObject("LoaderHelper").AddComponent<Loader>();
            Object.DontDestroyOnLoad(resourceLoader.gameObject);
        }

        /// <summary>
        /// 预加载资源
        /// </summary>
        public static void Init()
        {
            AddMaxProgress(CCConfig.ListOfGui.Length + CCConfig.ListOfAtlas.Length + CCConfig.ListOfData.Length);

            Load(CCConfig.ListOfGui, "GUI/", RegisterUI);
            Load(CCConfig.ListOfAtlas, "Atlas/", RegisterAtlas);
            Load(CCConfig.ListOfData, "Data/", RegisterData);
            LoadAll<Sprite>(CCConfig.ListOfSprite, "Atlas/", RegisterSprite);
        }

        /// <summary>
        /// 预加载UI资源
        /// </summary>
        public static GameObject RegisterUI(params object[] args)
        {
            var key = args[0] as string;
            var template = args[1] as GameObject;

            if (template == null) return null;

            if (guiSources.ContainsKey(key))
            {
                guiSources[key] = template;
            }
            else guiSources.Add(key, template);

            return template;
        }

        /// <summary>
        /// 预加载图集资源
        /// </summary>
        private static GameObject RegisterAtlas(params object[] args)
        {
            var key = args[0] as string;
            var template = args[1] as GameObject;

            if (template == null) return null;

            if (atlasSources.ContainsKey(key))
            {
                atlasSources[key] = template;
            }
            else atlasSources.Add(key, template);

            return template;
        }

        private static void RegisterSprite(params object[] args)
        {
            var key = args[0] as string;

            if (!(args[1] is object[] template)) return;

            foreach (Sprite s in template)
            {
                if (spriteSources.ContainsKey(key + s.name))
                {
                    spriteSources[key + s.name] = s;
                }
                else spriteSources.Add(key + s.name, s);
            }
        }

        /// <summary>
        /// 预加载数据资源
        /// </summary>
        private static TextAsset RegisterData(params object[] args)
        {
            var key = args[0] as string;
            var template = args[1] as TextAsset;

            if (template == null) return null;

            var mapData = CCMap.Parse(template.text);

            if (dataSources.ContainsKey(key))
            {
                dataSources[key] = mapData;
            }
            else dataSources.Add(key, mapData);

            return template;
        }

        /// <summary>
        /// 创建加载器
        /// </summary>
        /// <param name="names"></param>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        private static void Load<T>(string[] names, string path = "", Callback<T> callback = null) where T : class
        {
            if (names.Length <= 0) return;

            resourceLoader.StartCoroutine(resourceLoader.LoadRes(names, path, callback));
        }

        private static void LoadAll<T>(string[] names, string path = "", Callback callback = null) where T : class
        {
            if (names.Length <= 0) return;

            resourceLoader.StartCoroutine(resourceLoader.LoadAllRes<T>(names, path, callback));
        }

        public static T Load<T>(string name, string path, Callback<T> callback = null) where T : class
        {
            return resourceLoader.LoadRes(name, path, callback);
        }

        /// <summary>
        /// 资源加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static GameObject LoadAndInitRes(string path, Vector3 position)
        {
            return Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
        }

        /// <summary>
        /// 获取当前进度
        /// </summary>
        /// <returns></returns>
        public static float GetCurProgress()
        {
            return progress.cur;
        }

        /// <summary>
        /// 获取总进度
        /// </summary>
        /// <returns></returns>
        public static float GetMaxProgress()
        {
            return progress.max;
        }

        /// <summary>
        /// 增加当前进度
        /// </summary>
        /// <returns>The current progress.</returns>
        /// <param name="num">Number.</param>
        private static void AddCurProgress(int num)
        {
            progress.cur += num;
            if (progress.cur >= progress.max)
            {
                Complete();
            }
        }

        /// <summary>
        /// 增加进度上限
        /// </summary>
        /// <returns>The max progress.</returns>
        /// <param name="num">Number.</param>
        private static void AddMaxProgress(int num)
        {
            progress.max += num;
        }

        /// <summary>
        /// 获取比例值（保留小数点后两位）
        /// </summary>
        /// <returns></returns>
        public static float GetPercent()
        {
            return progress.GetPercent();
        }

        /// <summary>
        /// 获取比例文本
        /// </summary>
        /// <returns>The percent.</returns>
        public static string GetString()
        {
            return progress.GetString();
        }

        /// <summary>
        /// 加载完成
        /// </summary>
        private static void Complete()
        {
            CCGame.Launch();
        }

        /// <summary>
        /// 清空加载器列表
        /// </summary>
        public static void Clear()
        {
            resourceLoader.StopAllCoroutines();
            progress = new CCProgress(0, 0);
        }

        public class Loader : MonoBehaviour
        {
            /// <summary>
            /// 加载迭代器
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="names"></param>
            /// <param name="path"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            public IEnumerator LoadRes<T>([NotNull] string[] names, string path = "", Callback<T> callback = null)
                where T : class
            {
                if (names == null) throw new ArgumentNullException(nameof(names));

                foreach (var t in names)
                {
                    var obj = Resources.Load(path + t) as T;

                    callback?.Invoke(t, obj);
                    AddCurProgress(1);
                    yield return new WaitForSeconds(0.02f);
                }
            }

            public IEnumerator LoadAllRes<T>(string[] names, string path = "", Callback callback = null) where T : class
            {
                if (names.Length == 0)
                    throw new ArgumentException("Value cannot be an empty collection.", nameof(names));
                foreach (var t in names)
                {
                    var obj = Resources.LoadAll(path + t, typeof(T));

                    callback?.Invoke(t, obj);
                    AddCurProgress(1);
                    yield return new WaitForSeconds(0.02f);
                }
            }

            /// <summary>
            /// 立即加载资源
            /// </summary>
            /// <param name="resName"></param>
            /// <param name="path"></param>
            /// <param name="callback"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public T LoadRes<T>(string resName, string path, Callback<T> callback = null) where T : class
            {
                var obj = Resources.Load(path + resName) as T;

                return callback?.Invoke(resName, obj);
            }
        }
    }
}