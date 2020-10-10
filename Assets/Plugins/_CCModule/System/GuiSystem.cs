using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CC
{
    public enum MaskType
    {
        UTOD,
        DTOU,
        LTOR,
        RTOL
    }

    public static class GuiSystem
    {
        public class GuiSystemHelper : MonoBehaviour
        {
            void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
            {
                StartCoroutine(MaskOut(MaskType.UTOD));
            }

            public void AddSceneLoadedListener()
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
            }

            public void RemoveSceneLoadedListener()
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private static List<string> activeUi = new List<string>();
        private static readonly Canvas Canvas;
        private static readonly Camera UiCamera;
        private static readonly EventSystem EventSystem;
        internal static GameObject GuiHelper { get; }
        internal static readonly GameObject TipHelper;
        internal static readonly GameObject MaskHelper;
        internal static readonly GuiSystemHelper GuiSysHelper;
        private static readonly string SceneMaskPath = "Prefabs/SceneMask";
        private static readonly float MaskMinHeight = 0;//367.0f;
        private static readonly float MaskMaxHeight = 3000.0f;
        private static RectTransform sceneMask;
        private static GameObject tipNode;
        private static CCNodePool tipNodePool;
        public static GameObject GuideNode { get; }
        public static CCNodePool GuideNodePool { get; }

        static GuiSystem()
        {
            //Debug.Log("GuiSystem Awake");

            UiCamera = new GameObject("UICamera").AddComponent<Camera>();
            Canvas = new GameObject("Canvas").AddComponent<Canvas>();
            EventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
            GuiSysHelper = Canvas.gameObject.AddComponent<GuiSystemHelper>();
            GuiHelper = new GameObject("GUI");
            TipHelper = new GameObject("TIP");
            MaskHelper = new GameObject("MASK");

            var transform = Canvas.transform;
            GuiHelper.transform.parent = transform;
            TipHelper.transform.parent = transform;
            MaskHelper.transform.parent = transform;

            Canvas.gameObject.layer = LayerMask.NameToLayer("UI");
            GuiHelper.layer = LayerMask.NameToLayer("UI");
            TipHelper.layer = LayerMask.NameToLayer("UI");
            MaskHelper.layer = LayerMask.NameToLayer("UI");

            Object.DontDestroyOnLoad(Canvas.gameObject);
            Object.DontDestroyOnLoad(UiCamera.gameObject);
            Object.DontDestroyOnLoad(EventSystem.gameObject);
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            InitCamera();
            InitCanvas();
            InitCanvasScaler();
            InitCcguiRaycaster();
            InitEventSystem();
            InitInputModule();
            InitSubCanvas();
        }

        /// <summary>
        /// Inits the UiCamera.
        /// </summary>
        private static void InitCamera()
        {
            UiCamera.clearFlags = CameraClearFlags.Depth;
            UiCamera.cullingMask = LayerMask.GetMask("UI");
            UiCamera.orthographic = true;
            UiCamera.orthographicSize = CCConfig.DESIGN_RESOLUTION_HEIGHT / 2.0f;
            UiCamera.depth = 1;
        }

        /// <summary>
        /// Inits the canvas.
        /// </summary>
        private static void InitCanvas()
        {
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.pixelPerfect = false;
            Canvas.worldCamera = UiCamera;
            Canvas.planeDistance = 100;
            Canvas.sortingLayerID = 0;
            Canvas.sortingOrder = 0;
            Canvas.gameObject.GetOrAddComponent<Mask>();
        }

        /// <summary>
        /// 初始化子Canvas
        /// </summary>
        private static void InitSubCanvas()
        {
            var rt = GuiHelper.GetOrAddComponent<RectTransform>();

            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(0, 0);

            rt = TipHelper.GetOrAddComponent<RectTransform>();

            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(0, 0);

            rt = MaskHelper.GetOrAddComponent<RectTransform>();

            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(0, 0);
        }

        /// <summary>
        /// Inits the canvas scaler.
        /// </summary>
        private static void InitCanvasScaler()
        {
            var canvasScaler = Canvas.gameObject.AddComponent<CanvasScaler>();

            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution =
                new Vector2(CCConfig.DESIGN_RESOLUTION_WIDTH, CCConfig.DESIGN_RESOLUTION_HEIGHT);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            //canvasScaler.matchWidthOrHeight = GetScreenPercent() > 16 / 9f ? 1.0f : 0f;
            canvasScaler.matchWidthOrHeight = 0.5F;
        }

        /// <summary>
        /// 获取屏蔽比例
        /// </summary>
        /// <returns></returns>
        private static float GetScreenPercent()
        {
            var screenPercent = Screen.height / Screen.width;
            return screenPercent;
        }

        /// <summary>
        /// 初始化输入检测组件
        /// </summary>
        private static void InitCcguiRaycaster()
        {
            var graphicRaycaster = Canvas.gameObject.AddComponent<CCGUIRaycaster>();
            graphicRaycaster.DefaultSetOfGUI();
        }

        /// <summary>
        /// 初始化事件系统
        /// </summary>
        private static void InitEventSystem()
        {
            EventSystem.sendNavigationEvents = true;
            EventSystem.pixelDragThreshold = 5;
        }

        /// <summary>
        /// 初始化输入控制组件
        /// </summary>
        private static void InitInputModule()
        {
            var inputModule = EventSystem.gameObject.AddComponent<StandaloneInputModule>();

            inputModule.horizontalAxis = "Horizontal";
            inputModule.verticalAxis = "Vertical";
            inputModule.submitButton = "Submit";
            inputModule.cancelButton = "Cancel";
            inputModule.inputActionsPerSecond = 10;
            inputModule.repeatDelay = 0.5f;
            inputModule.forceModuleActive = false;
        }

        /// <summary>
        /// 获取遮罩节点
        /// </summary>
        /// <returns></returns>
        private static RectTransform GetMaskRectTransform()
        {
            var sceneMaskTran = MaskHelper.transform.Find("SceneMask");

            if (sceneMask == null)
            {
                sceneMaskTran = Object.Instantiate(Resources.Load<GameObject>(SceneMaskPath)).transform;
                sceneMaskTran.gameObject.name = "SceneMask";
                sceneMaskTran.SetParent(MaskHelper.transform);
            }

            sceneMask = sceneMaskTran.GetComponent<RectTransform>();
            var maskCanvas = sceneMask.GetComponent<Canvas>();

            if (maskCanvas != null) return sceneMask;

            maskCanvas = sceneMask.gameObject.AddComponent<Canvas>();
            maskCanvas.overrideSorting = true;
            maskCanvas.sortingOrder = CCConfig.MASK_LAYER_ORDER;

            return sceneMask;
        }

        /// <summary>
        /// 开始遮罩动画
        /// </summary>
        /// <param name="inOrOut"></param>
        /// <param name="maskType"></param>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        public static void StartMask(bool inOrOut, MaskType maskType, Callback callback = null, params object[] args)
        {
            GuiSysHelper.StartCoroutine(inOrOut
                ? MaskIn(maskType, callback, args)
                : MaskOut(maskType, callback, args));
        }

        /// <summary>
        /// 遮罩图入场
        /// </summary>
        /// <param name="maskType"></param>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IEnumerator MaskIn(MaskType maskType, Callback callback = null, params object[] args)
        {
            var maskRect = GetMaskRectTransform();
            var canvasRect = Canvas.GetComponent<RectTransform>();
            var sizeDelta = canvasRect.sizeDelta;
            var currentSizeData = new Vector2(0, 0);
            var targetSizeData = new Vector2(sizeDelta.x, sizeDelta.y);
            float frameTime = 0;

            maskRect.sizeDelta = currentSizeData;
            maskRect.anchorMin = new Vector2(0, 0);
            maskRect.anchorMax = new Vector2(1, 1);
            maskRect.offsetMin = new Vector2(0, 0);
            maskRect.offsetMax = new Vector2(1, 1);
            maskRect.pivot = new Vector2(0.5f, 1f);

            sceneMask.localScale = Vector3.one / 2;
            switch (maskType)
            {
                case MaskType.UTOD:
                    sceneMask.localPosition =
                        Vector3.up * (((RectTransform)Canvas.transform).sizeDelta.y / 2 );
                    break;
                case MaskType.DTOU:
                    sceneMask.localPosition =
                        Vector3.down * (((RectTransform)Canvas.transform).sizeDelta.y / 2 );
                    break;
            }

            while (targetSizeData.y - maskRect.sizeDelta.y > 5)
            {
                frameTime += Time.deltaTime;

                if (frameTime > 1.0f / AnimManager.FPS)
                {
                    frameTime = 0;
                    maskRect.sizeDelta = Vector2.Lerp(maskRect.sizeDelta, targetSizeData, 0.1f);
                }

                yield return new WaitForEndOfFrame();
            }

            maskRect.sizeDelta = targetSizeData;
            GuiSysHelper.AddSceneLoadedListener();
            callback?.Invoke(args);
        }

        /// <summary>
        /// 遮罩图退出
        /// </summary>
        /// <param name="maskType"></param>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IEnumerator MaskOut(MaskType maskType, Callback callback = null, params object[] args)
        {
            var maskRect = GetMaskRectTransform();
            var canvasRect = Canvas.GetComponent<RectTransform>();
            var sizeDelta = canvasRect.sizeDelta;
            var currentSizeData = new Vector2(sizeDelta.x, MaskMaxHeight);
            var targetSizeData = new Vector2(sizeDelta.x, MaskMinHeight);
            float frameTime = 0;

            maskRect.anchorMin = new Vector2(0, 0);
            maskRect.anchorMax = new Vector2(1, 0);
            maskRect.offsetMin = new Vector2(0, 0);
            maskRect.offsetMax = new Vector2(0, 0);
            maskRect.pivot = new Vector2(0.5f, 0);
            maskRect.sizeDelta = currentSizeData;

            switch (maskType)
            {
                case MaskType.UTOD:
                    sceneMask.localPosition = Vector3.down * (MaskMaxHeight / 2 + MaskMinHeight);
                    break;
                case MaskType.DTOU:
                    sceneMask.localPosition = Vector3.up * (MaskMaxHeight / 2 + MaskMinHeight);
                    break;
            }

            while (maskRect.sizeDelta.y - targetSizeData.y > 5)
            {
                frameTime += Time.deltaTime;

                if (frameTime > 1.0f / AnimManager.FPS)
                {
                    frameTime = 0;
                    maskRect.sizeDelta = Vector2.Lerp(maskRect.sizeDelta, targetSizeData, 0.2f);
                }

                yield return new WaitForEndOfFrame();
            }

            maskRect.sizeDelta = targetSizeData;
            GuiSysHelper.RemoveSceneLoadedListener();
            callback?.Invoke(args);
        }

        /// <summary>
        /// 获取场景UI画布
        /// </summary>
        /// <returns></returns>
        public static Canvas GetGuiCanvas()
        {
            return Canvas;
        }

        /// <summary>
        /// 获取UI相机
        /// </summary>
        /// <returns></returns>
        public static Camera GetGuiCamera()
        {
            return UiCamera;
        }

        /// <summary>
        /// 获取GUI层坐标
        /// </summary>
        public static Vector3 GetCanvasPosition(Transform tran)
        {
            var parent = tran.parent;
            var result = tran.localPosition;

            while (parent && parent != Canvas.transform.parent)
            {
                result += parent.localPosition;
                parent = parent.parent;
            }

            return result;
        }

        /// <summary>
        /// GUI节点坐标转指定摄像机下的世界坐标
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="lookCamera"></param>
        /// <param name="worldPosition"></param>
        public static void GuiToWorldPosition(Transform tran, Camera lookCamera, out Vector3 worldPosition)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(Canvas.transform as RectTransform,
                UiCamera.WorldToScreenPoint(tran.position), lookCamera, out worldPosition);
        }

        /// <summary>
        /// 指定摄像机下的节点坐标转GUI坐标
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="lookCamera"></param>
        /// <param name="worldPosition"></param>
        public static void WorldToGuiPosition(Transform tran, Camera lookCamera, out Vector3 worldPosition)
        {
            worldPosition =
                RectTransformUtility.WorldToScreenPoint(lookCamera, lookCamera.WorldToScreenPoint(tran.position));
        }

        /// <summary>
        /// 获取屏幕尺寸
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetScreenSize()
        {
            return ((RectTransform) Canvas.transform).sizeDelta;
        }

        /// <summary>
        /// 根据UI名称查找UI节点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CCGui FindUiByName(string name)
        {
            var result = GuiHelper.transform.Find(name);
            return result == null ? null : result.GetComponent<CCGui>();
        }

        /// <summary>
        /// 查找最顶层的一个UI节点
        /// </summary>
        /// <returns></returns>
        public static CCGui FindLastUi()
        {
            if (activeUi.Count <= 0) return null;
            var lastNodeName = activeUi[activeUi.Count - 1];
            return FindUiByName(lastNodeName);
        }

        /// <summary>
        /// 打开一个UI界面
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static CCGui Open(string name, Callback callback = null, params object[] arg)
        {
            if (!AssetsLoad.guiSources.ContainsKey(name))
            {
                Debug.Log((object)("UI资源未加载，立即加载--" + name));
                var loadResult = AssetsLoad.Load(name, "GUI/", LoadTemplate);
                return loadResult == null ? null : Open(name);
            }

            var gui = FindUiByName(name);
            GameObject uiNode;

            if (gui == null)
            {
                uiNode = Object.Instantiate(AssetsLoad.guiSources[name], GuiHelper.transform, true);
                uiNode.name = name;
                gui = uiNode.GetComponent<CCGui>();
            }
            else
            {
                uiNode = gui.gameObject;
                uiNode.SetActive(true);
            }

            gui.Init();

            AddActiveUI(name);
            //SortGui(gui);
            gui.Resize();
            gui.Enter();

            callback?.Invoke(arg);

            return gui;
        }

        /// <summary>
        /// 移除UI
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CCGui Close(string name)
        {
            var gui = FindUiByName(name);
            if (!gui) return null;

            RemoveActiveUI(name);
            gui.Exit();
            return FindLastUi();
        }

        /// <summary>
        /// 移除顶层UI
        /// </summary>
        /// <returns></returns>
        public static CCGui CloseTopUI()
        {
            var gui = FindLastUi();

            if (!gui || !gui.onBack) return null;

            gui.Close();
            return FindLastUi();
        }

        /// <summary>
        /// 即时加载UI
        /// </summary>
        /// <returns>The template.</returns>
        /// <param name="args">Arguments.</param>
        private static GameObject LoadTemplate(params object[] args)
        {
            var name = args[0] as string;
            var template = args[1] as GameObject;

            if (template == null) Debug.Log((object)("加载资源失败:" + name));
            AssetsLoad.RegisterUI(name, template);
            return template;
        }

        /// <summary>
        /// 设置显示顺序
        /// </summary>
        /// <param name="gui"></param>
        private static void SortGui(CCGui gui)
        {
            Canvas tempCanvas = gui.GetOrAddComponent<Canvas>();

            tempCanvas.overrideSorting = true;

            switch (gui.guiType)
            {
                case GuiType.MASK:
                    gui.GetComponent<Canvas>().sortingOrder = activeUi.Count * 2 + CCConfig.MASK_LAYER_ORDER;
                    break;
                default:
                    gui.GetComponent<Canvas>().sortingOrder = activeUi.Count * 2;
                    break;
            }
        }

        /// <summary>
        /// 向已激活列表中添加UI
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<string> AddActiveUI(string name)
        {
            if (!activeUi.Contains(name)) activeUi.Add(name);
            return activeUi;
        }

        /// <summary>
        /// 从已激活列表中移除UI
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<string> RemoveActiveUI(string name)
        {
            if (activeUi.Contains(name)) activeUi.Remove(name);
            return activeUi;
        }

        public static void AddTips(string text)
        {
            if (tipNode == null)
            {
                var tipsRes = Resources.Load<GameObject>("Prefabs/UI_Tip");
                tipNode = Object.Instantiate(tipsRes, TipHelper.transform, true);
                tipNode.transform.localScale = Vector3.one;
                tipNode.GetComponent<Canvas>().sortingOrder = CCConfig.TIP_LAYER_ORDER;
                //tipNodePool = new CCNodePool(tipNode, 3);
            }

            //var obj = tipNodePool.Add(PoolScaleType.RECYCLE, Vector3.zero);
            tipNode.transform.Find("Text").GetComponent<Text>().text = text;
            tipNode.SetActive(true);
            //TimerSystem.Add(TimerType.COMMON, 1, 2f, RemoveTips, obj);
        }

        /// <summary>
        /// 移除tips
        /// </summary>
        /// <param name="args"></param>
        private static void RemoveTips(params object[] args)
        {
            var obj = args[0] as GameObject;
            tipNodePool.Put(obj);
        }

        /// <summary>
        /// 清空UI列表
        /// </summary>
        public static void Clear()
        {
            for (var i = activeUi.Count - 1; i >= 0; i--)
            {
                var gui = FindUiByName(activeUi[i]);
                gui.OnClose();
            }

            activeUi = new List<string>();
        }
    }
}