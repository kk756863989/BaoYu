using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CC
{
    /// <summary>
    /// gui动画类型
    /// </summary>
    public enum AnimType
    {
        NONE,
        FADE,
        SCALEUP,
        SCALEDOWN,
        ROTATE,
        MOVELEFT,
        MOVERIGHT,
        MOVEUP,
        MOVEDOWN,
    }

    /// <summary>
    /// 0默认，关闭时销毁此界面节点
    /// 1隐藏，关闭时隐藏此界面节点
    /// 2界面永不关闭，例如主界面
    /// </summary>
    public enum CloseType
    {
        DEFAULT = 0,
        HIDE = 1,
        NEVER = 2,
    }

    /// <summary>
    /// Gui显示层级类型
    /// </summary>
    public enum GuiType
    {
        GUI = 0,
        MASK = 1
    }

    [RequireComponent(typeof(GraphicRaycaster))]
    public class CCGui : MonoBehaviour, IReuseObject
    {
        [Header("BaseAttribute")]
        public GuiType guiType; //层级类型
        public CloseType closeType = CloseType.DEFAULT; //关闭方式
        public bool allowDelayUpdate; //是否响应延时刷新

        [Header("GuiAnimation")] [SerializeField]
        private bool isElastic; //是否回弹
        public AnimType enterType; //入场方式
        public AnimType exitType; //出场方式


        //[Header("GuiNativeHandler")]
        [HideInInspector]
        public bool onBack; //是否响应安卓返回键
        [Header("Local")]
        public string HeaderTip;
        public bool HasUsed { get; set; }
        public string EntityId { get; set; }

        /// <summary>
        /// 界面初始化
        /// </summary>
        public void Init()
        {
            if (!HasUsed)
            {
                OnFirstUse();
                HasUsed = true;
            }
            else
            {
                OnReuse();
            }
        }

        public virtual void OnFirstUse()
        {
        }

        public virtual void OnReuse()
        {
        }

        public virtual void OnRecycle()
        {
        }

        public virtual void OnRelease()
        {
        }

        /// <summary>
        /// 入场动画
        /// </summary>
        public virtual void Enter()
        {
            var rt = gameObject.GetComponent<RectTransform>();
            switch (enterType)
            {
                case AnimType.FADE:
                    break;
                case AnimType.SCALEUP:
                    rt.localPosition = Vector3.zero;
                    StartCoroutine(AnimManager.ElasticScale(rt, 0.7f, 0.3f, 1.0f, UpdateData));
                    break;
                case AnimType.SCALEDOWN:
                    rt.localPosition = Vector3.zero;
                    StartCoroutine(AnimManager.ElasticScale(rt, 0.7f, 2f, 1.0f, UpdateData));
                    break;
                // case MotionType.ROTATE:
                //     break;
                case AnimType.MOVERIGHT:
                    rt.SetScale(1.0f, 1.0f, 1.0f);

                    if (isElastic)
                    {
                        StartCoroutine(AnimManager.ElasticMove(rt, 0.4f,
                            new Vector3(-CCConfig.DESIGN_RESOLUTION_WIDTH, 0, 0), Vector3.zero, UpdateData));
                    }
                    else
                    {
                        StartCoroutine(AnimManager.Move(rt, 0.4f, new Vector3(-CCConfig.DESIGN_RESOLUTION_WIDTH, 0, 0),
                            Vector3.zero, UpdateData));
                    }

                    break;
                case AnimType.MOVELEFT:
                    rt.SetScale(1.0f, 1.0f, 1.0f);

                    if (isElastic)
                    {
                        StartCoroutine(AnimManager.ElasticMove(rt, 0.4f,
                            new Vector3(CCConfig.DESIGN_RESOLUTION_WIDTH, 0, 0), Vector3.zero, UpdateData));
                    }
                    else
                    {
                        StartCoroutine(AnimManager.Move(rt, 0.4f, new Vector3(CCConfig.DESIGN_RESOLUTION_WIDTH, 0, 0),
                            Vector3.zero, UpdateData));
                    }

                    break;
                case AnimType.MOVEUP:
                    rt.localPosition = new Vector3(0, -1280, 0);
                    rt.SetScale(1.0f, 1.0f, 1.0f);

                    StartCoroutine(
                        isElastic
                            ? AnimManager.ElasticMove(rt, 0.4f, new Vector3(0, -1280, 0), Vector3.zero, UpdateData)
                            : AnimManager.Move(rt, 0.4f, new Vector3(0, -1280, 0), Vector3.zero, UpdateData));

                    break;
                case AnimType.MOVEDOWN:
                    rt.localPosition = new Vector3(0, 1280, 0);
                    rt.SetScale(1.0f, 1.0f, 1.0f);

                    if (isElastic)
                    {
                        StartCoroutine(AnimManager.ElasticMove(rt, 0.4f, new Vector3(0, 1280, 0), Vector3.zero,
                            UpdateData));
                    }
                    else
                    {
                        StartCoroutine(AnimManager.Move(rt, 0.4f, new Vector3(0, 1280, 0), Vector3.zero, UpdateData));
                    }

                    break;
                default:
                    rt.localPosition = Vector3.zero;
                    rt.SetScale(1.0f, 1.0f, 1.0f);

                    if (allowDelayUpdate) StartCoroutine(UpdateData(0.2f));
                    else UpdateData();
                    break;
            }
        }

        /// <summary>
        /// 退场动画
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        public virtual void Exit(Callback callback = null, params object[] args)
        {
            callback = callback == null ? OnClose : OnClose + callback;
            switch (exitType)
            {
                case AnimType.FADE:
                    break;
                case AnimType.SCALEUP:
                    transform.localPosition = Vector3.zero;
                    StartCoroutine(AnimManager.ElasticScale(
                        transform,
                        0.7f,
                        1.0f,
                        5.0f,
                        callback,
                        args));
                    break;
                case AnimType.SCALEDOWN:
                    transform.localPosition = Vector3.zero;
                    StartCoroutine(AnimManager.ElasticScale(
                        transform,
                        0.7f,
                        5.0f,
                        0f,
                        callback,
                        args));
                    break;
                case AnimType.ROTATE:
                    break;
                case AnimType.MOVEUP:
                    if (isElastic)
                    {
                        StartCoroutine(AnimManager.ElasticMove(
                            transform,
                            0.3f,
                            transform.localPosition,
                            new Vector3(0, CCConfig.DESIGN_RESOLUTION_HEIGHT, 0),
                            callback,
                            args));
                    }
                    else
                    {
                        StartCoroutine(AnimManager.Move(
                            transform,
                            0.3f,
                            transform.localPosition,
                            new Vector3(0, CCConfig.DESIGN_RESOLUTION_HEIGHT, 0),
                            callback,
                            args));
                    }

                    break;
                case AnimType.MOVEDOWN:
                    if (isElastic)
                    {
                        StartCoroutine(AnimManager.ElasticMove(
                            transform,
                            0.4f,
                            transform.localPosition,
                            new Vector3(0, -CCConfig.DESIGN_RESOLUTION_HEIGHT, 0),
                            callback,
                            args));
                    }
                    else
                    {
                        StartCoroutine(AnimManager.Move(
                            transform,
                            0.4f,
                            transform.localPosition,
                            new Vector3(0, -CCConfig.DESIGN_RESOLUTION_HEIGHT, 0),
                            callback,
                            args));
                    }

                    break;
                case AnimType.MOVELEFT:
                    if (isElastic)
                    {
                        StartCoroutine(AnimManager.ElasticMove(
                            transform,
                            0.4f,
                            transform.localPosition,
                            new Vector3(-CCConfig.DESIGN_RESOLUTION_WIDTH, 0, 0),
                            callback,
                            args));
                    }
                    else
                    {
                        StartCoroutine(AnimManager.Move(
                            transform,
                            0.4f,
                            transform.localPosition,
                            new Vector3(-CCConfig.DESIGN_RESOLUTION_WIDTH, 0, 0),
                            callback,
                            args));
                    }

                    break;
                case AnimType.MOVERIGHT:
                    if (isElastic)
                    {
                        StartCoroutine(AnimManager.ElasticMove(
                            transform,
                            0.4f,
                            transform.localPosition,
                            new Vector3(CCConfig.DESIGN_RESOLUTION_WIDTH, 0, 0),
                            callback,
                            args));
                    }
                    else
                    {
                        StartCoroutine(AnimManager.Move(
                            transform,
                            0.4f,
                            transform.localPosition,
                            new Vector3(CCConfig.DESIGN_RESOLUTION_WIDTH, 0, 0),
                            callback,
                            args));
                    }

                    break;
                default:
                    callback();
                    break;
            }
        }

        /// <summary>
        /// 延时更新界面
        /// </summary>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        private IEnumerator UpdateData(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            UpdateData();
        }

        /// <summary>
        /// 更新界面信息
        /// </summary>
        /// <param name="args"></param>
        protected virtual void UpdateData(params object[] args)
        {

        }

        /// <summary>
        /// 重设界面尺寸
        /// </summary>
        public void Resize()
        {
            var rt = GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(0, 0);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiName"></param>
        protected void Open(string uiName)
        {
            AudioSystem.Play(AudioName.GUI.EnumToString(), "click");
            GuiSystem.Open(uiName);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="args"></param>
        public virtual void Close(params object[] args)
        {
            AudioSystem.Play(AudioName.GUI.EnumToString(), "click");
            GuiSystem.Close(gameObject.name);
        }

        /// <summary>
        /// 关闭回调
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnClose(params object[] args)
        {
            switch (closeType)
            {
                default:
                    OnRelease();
                    Destroy(gameObject);
                    break;
                case CloseType.HIDE:
                    OnRecycle();
                    gameObject.SetActive(false);
                    break;
                case CloseType.NEVER:
                    break;
            }
        }

        /// <summary>
        /// 为物体添加按钮事件
        /// </summary>
        /// <param name="tran">需要添加事件的物体</param>
        /// <param name="callback">按钮事件</param>
        /// <param name="needClear">是否需要清空老事件</param>
        /// <param name="callbackData">事件参数</param>
        /// <returns></returns>
        protected void AddClickEvent(Transform tran, Callback callback, bool needClear = true,
            params object[] callbackData)
        {
            var button = tran.gameObject.GetOrAddComponent<Button>();

            if (needClear) button.onClick = new Button.ButtonClickedEvent();

            button.onClick.AddListener(delegate() { callback(callbackData); });
        }
    }
}