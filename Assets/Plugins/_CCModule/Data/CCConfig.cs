namespace CC
{
    public static class CCConfig
    {
        public const bool IS_OFFLINE = true; //是否是离线模式
        public const int FPS = 60; //（-1为跟随系统，0以上为强制帧率，最高60）
        public const int SLEEP_DELAY_TIME = -1; //（-1不自动休眠，-2跟随系统设置，0以上为具体延时时间）
        public const int MASK_LAYER_ORDER = 2000; //遮罩显示层级
        public const int TIP_LAYER_ORDER = 1000; //tips显示层级
        public const int DESIGN_RESOLUTION_WIDTH = 800; //自定义尺寸宽度
        public const int DESIGN_RESOLUTION_HEIGHT = 1280; //自定义尺寸高度
        public const string USER_DATA = "{\"id\":1,\"name\":\"doctor strange\"}";
        public const string SCENE_DATA = "{\"id\":1,\"name\":\"doctor strange\"}";
        public const string KEY_FOR_USER_PREF = "curUser";
        public const string KEY_FOR_SCENE_PREF = "curScene";
        public const string KEY_FOR_MUSIC = "music";
        public const string KEY_FOR_SOUND = "sound";
        public const string UI_CAMERA_NAME = "UICamera";

        public static readonly string[] DefaultAudios =
            {"BACK", "FORE", "GUI", "FUI", "USER", "FUSER", "SCENE", "NPC", "MONSTER"};

        public static readonly int[] DefaultAudioTypes = {1, 1, 0, 0, 0, 0, 0, 0, 0};

        public static readonly int AutoHealInternalTime = 1800; //自动恢复间隔时长(s)
        public static readonly int AutoHealNum = 1; //自动恢复值
        public static readonly int MaxHp = 1; //角色生命值上限
        public static readonly int MaxLevel = 200; //关卡总数量
        public static readonly int LimitLevel = 200; //开放关卡数量
        public static readonly int MaxBallCount = 30; //每关球最大数量
        public static readonly int MaxEnergy = 100; //每关能量上限
        public static readonly int MaxBallNumber = 2048;
        public static readonly float[] TargetPercents = new float[] {1, 2, 3}; //积分偏移
        public static readonly float AddBallDelayTime = 0.1f; //触发器通用延时
        public static readonly float TriggerDelayTime = 0.2f; //触发器通用延时
        public static readonly float BombRange = 1.3f; //爆炸范围
        public static readonly int ScoreBase = 100;
        public static readonly int ScoreOffset = 50;
        public static readonly int InterInternalTime = 120;

        public static string[] productIds =
        {
            "INGOT0001",
            "INGOT0002",
            "INGOT0003",
            "INGOT0004",
            "INGOT0005",
            "INGOT0006"
        };

        /// <summary>
        /// 消息键值
        /// </summary>
        public enum MsgKey
        {
            UPDATE_COIN,
            UPDATE_GOLD,
            UPDATE_LEVEL_SCORE,
            UPDATE_LEVEL_ENERGY,
            UPDATE_LEVEL_STEP,
            UPDATE_ITEM_COUNT,
            UPDATE_TARGET_TYPE,
            SELECT_BALL_FOR_BOMB
        }

        public enum VideoTag
        {
            ENTER_LEVEL_ITEM_FREE, //进入关卡时视频免费得吹风机
            SCENE_ITEM_EXCHANGE, //场景内通过视频购买道具
            NEXT_LEVEL_INTER, //前往下一关时播放插屏（间隔120s）
            PAUSE_LEVEL_INTER, //点击暂停时，如果有插屏，直接播放，不计时
            SIGN_IN
        }

        public enum InterTag
        {
            SIGNIN_TO_MAIN,
            SHOP_TO_MAIN,
            LEVEL_TO_MAIN,
            NEXT_LEVEL_INTER,
            GAME_PAUSE
        }

        public enum EventName
        {
            LEVEL_COMPLETE, // 关卡结束 参数关卡ID
            GUIDE_STEP, // 引导步骤 参数步骤ID
            OPEN_GUI, // 打开界面 参数界面名
            CLOSE_GUI, // 关闭界面 参数界面名
            EXCHANGE_ITEM // 购买道具 参数道具ID
        }

        public enum EventType
        {
            LEVEL,
            GUIDE,
            GUI,
            EXCHANGE
        }

        /// <summary>
        /// 预加载配置表列表
        /// </summary>
        public static readonly string[] ListOfData =
        {
            "ExInfo",
            "QAInfo"
        };

        /// <summary>
        /// 预加载UI列表
        /// </summary>
        public static readonly string[] ListOfGui =
        {
            "MainUI",
            "UI_Exnfo",
            "MenuBtn",
            "UI_QA",
            "UI_Step1",
            "UI_Step2",
            "UI_Step3",
            "UI_Step4",
            "UI_Start",
            "UI_ChoisedEq"

        };

        /// <summary>
        /// 预加载图集列表
        /// </summary>
        public static readonly string[] ListOfAtlas =
        {

        };

        /// <summary>
        /// 预加载图集列表
        /// </summary>
        public static readonly string[] ListOfSprite =
        {
            //"ball",
            //"guide"
        };
    }
}