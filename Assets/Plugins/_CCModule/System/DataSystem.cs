using UnityEngine;

namespace CC
{
    public static class DataSystem
    {
        private static bool running;
        private static CCUser user;
        private static CCScene scene;
        private static CCMap userData, sceneData, staticData, runtimeData;

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            if (IsRunning()) return;

            //Debug.Log("DataSystem Awake");

            SwitchState(true);
            InitData();
            InitUser();
            //InitScene();
        }

        /// <summary>
        /// 切换运行状态
        /// </summary>
        private static void SwitchState(bool state)
        {
            running = state;
        }

        /// <summary>
        /// 获取运行状态
        /// </summary>
        private static bool IsRunning()
        {
            return running;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private static void InitData()
        {
            staticData = new CCMap();
            runtimeData = new CCMap();
        }

        /// <summary>
        /// 获取运行时数据
        /// </summary>
        public static T SetRuntimeData<T>(string key, T value)
        {
            return runtimeData.Set(key, value);
        }

        /// <summary>
        /// 获取运行时数据
        /// </summary>
        public static int AddRuntimeData(string key, int value)
        {
            return runtimeData.Add(key, value);
        }

        /// <summary>
        /// 获取运行时数据
        /// </summary>
        public static float AddRuntimeData(string key, float value)
        {
            return runtimeData.Add(key, value);
        }

        /// <summary>
        /// 获取运行时数据
        /// </summary>
        public static T GetRuntimeData<T>(string key)
        {
            return runtimeData.Query<T>(key);
        }

        /// <summary>
        /// 初始化角色
        /// </summary>
        private static void InitUser()
        {
            userData = LoadUserData() ?? CreateUserData();

            user = new CCUser(userData);
            user.Init();
        }

        /// <summary>
        /// 从存档中载入角色数据
        /// </summary>
        /// <returns></returns>
        private static CCMap LoadUserData()
        {
            if (!PlayerPrefs.HasKey(CCConfig.KEY_FOR_USER_PREF)) return null;

            string dataString = PlayerPrefs.GetString(CCConfig.KEY_FOR_USER_PREF);

            return CCMap.Parse(dataString);
        }

        /// <summary>
        /// 创建角色数据
        /// </summary>
        /// <returns></returns>
        private static CCMap CreateUserData()
        {
            return CCMap.Parse(CCConfig.USER_DATA);
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns>The user.</returns>
        public static CCUser GetUser()
        {
            if (user == null) InitUser();
            return user;
        }

        /// <summary>
        /// 存储角色数据到存档中
        /// </summary>
        public static bool SaveUser()
        {
            if (userData == null) return false;
            PlayerPrefs.SetString(CCConfig.KEY_FOR_USER_PREF, CCMap.Stringify(userData));
            PlayerPrefs.Save();
            return true;
        }

        /// <summary>
        /// 初始化场景
        /// </summary>
        private static void InitScene()
        {
            sceneData = LoadSceneData();

            if (userData == null)
            {
                sceneData = CreateSceneData();
            }

            scene = new CCScene(sceneData, null);
        }

        /// <summary>
        /// 从存档中载入场景数据
        /// </summary>
        /// <returns></returns>
        private static CCMap LoadSceneData()
        {
            if (!PlayerPrefs.HasKey(CCConfig.KEY_FOR_SCENE_PREF)) return null;

            string dataString = PlayerPrefs.GetString(CCConfig.KEY_FOR_SCENE_PREF);

            return CCMap.Parse(dataString);
        }

        /// <summary>
        /// 创建场景数据
        /// </summary>
        /// <returns></returns>
        private static CCMap CreateSceneData()
        {
            return CCMap.Parse(CCConfig.SCENE_DATA);
        }

        /// <summary>
        /// 获取当前场景
        /// </summary>
        /// <returns>The user.</returns>
        public static CCScene GetScene()
        {
            if (scene == null) InitScene();
            return scene;
        }

        /// <summary>
        /// 存储场景数据到存档中
        /// </summary>
        public static bool SaveScene()
        {
            if (sceneData == null) return false;
            PlayerPrefs.SetString(CCConfig.KEY_FOR_SCENE_PREF, CCMap.Stringify(sceneData));
            PlayerPrefs.Save();
            return true;
        }

        /// <summary>
        /// 存储string数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 存储int类型数据
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void Save(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 存储float数据
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void Save(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 读取string数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void Load(string key, out string result)
        {
            result = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : "";
        }

        /// <summary>
        /// 读取int数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void Load(string key, out int result)
        {
            result = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : 0;
        }

        /// <summary>
        /// 读取float数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void Load(string key, out float result)
        {
            result = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : 0;
        }
    }
}