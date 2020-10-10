using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CC
{
    /// <summary>
    /// 游戏状态
    /// </summary>
    public enum GameState
    {
        OFFLINE, //离线
        LAUNCH, //加载
        READY, //已就绪
        BEGIN, //开始
        START, //关卡开始
        PAUSE, //关卡暂停
        RESUME, //关卡继续
        DEFEAT, //关卡失败
        SUCCESS, //关卡胜利
        STOP, //关卡结束
        RESTART, //关卡重启
        EXIT //关卡退出
    }

    public static class CCGame
    {
        private static GameState gameState = GameState.OFFLINE;

        static CCGame()
        {
            Application.targetFrameRate = CCConfig.FPS;
            Screen.sleepTimeout = CCConfig.SLEEP_DELAY_TIME;
            SwitchState(GameState.OFFLINE);
        }

        /// <summary>
        /// 游戏资源初始化
        /// </summary>
        public static void Init()
        {
            //SdkBridger.Init();
            AssetsLoad.Init();
        }

        public static GameState GetState()
        {
            return gameState;
        }

        /// <summary>
        /// 切换游戏状态
        /// </summary>
        private static void SwitchState(GameState state)
        {
            gameState = state;
            MsgSystem.Dispatch(GetStateName(state));
        }

        /// <summary>
        /// 获取游戏状态事件名
        /// </summary>
        private static string GetStateName(GameState state)
        {
            string result = "Undefined";

            switch (state)
            {
                case GameState.OFFLINE:
                    break;
                case GameState.LAUNCH:
                    result = "GameLaunch"; //加载
                    break;
                case GameState.READY:
                    result = "GameReady"; //就绪
                    break;
                case GameState.BEGIN:
                    result = "GameBegin"; //开始
                    break;
                case GameState.START:
                    result = "LevelStart"; //开始关卡
                    break;
                case GameState.PAUSE:
                    result = "LevelPause"; //暂停关卡
                    break;
                case GameState.RESUME:
                    result = "LevelResume"; //关卡继续
                    break;
                case GameState.DEFEAT:
                    result = "LevelDefeat"; //关卡失败
                    break;
                case GameState.SUCCESS:
                    result = "LevelSuccess"; //关卡胜利
                    break;
                case GameState.STOP:
                    result = "LevelStop"; //关卡结束
                    break;
                case GameState.RESTART:
                    result = "LevelRestart"; //关卡退出
                    break;
                case GameState.EXIT:
                    result = "LevelExit"; //关卡退出
                    break;
            }
            return result;
        }

        /// <summary>
        /// 游戏是否启动
        /// </summary>
        public static bool IsRunning()
        {
            return gameState != GameState.OFFLINE;
        }

        public static bool IsPlaying()
        {
            return gameState == GameState.START;
        }

       

        /// <summary>
        /// 游戏系统初始化
        /// </summary>
        public static void Launch()
        {
           // Debug.Log("GAME LAUNCH...");
            SwitchState(GameState.LAUNCH);
            MsgSystem.Init();
            ////TimerSystem.Init();
           // AudioSystem.Init();
            GuiSystem.Init();
            DataSystem.Init();
            Ready();
        }

        /// <summary>
        /// 游戏预备完毕，可以开始关卡
        /// </summary>
        public static void Ready()
        {
            //Debug.Log("GAME READY...");
            SwitchState(GameState.READY);
            Begin();
        }

        /// <summary>
        /// 游戏开始
        /// </summary>
        public static void Begin()
        {
            //Debug.Log("GAME BEGIN...");
            SwitchState(GameState.BEGIN);
            TimerSystem.RunMainTimer();
            if (CCLauncher.GetInstance().FirstUI != null)
                GuiSystem.Open(CCLauncher.GetInstance().FirstUI.name);
        }

        /// <summary>
        /// 关卡开始,切换场景
        /// </summary>
        public static void Start()
        {
            Debug.Log("GAME START...");
            SwitchState(GameState.START);
            //TimerSystem.RunGameTimer();
            SwitchSceneWithMask("Main", false);
        }

        /// <summary>
        /// 关卡开始
        /// </summary>
        public static void Next()
        {
           // Debug.Log("GAME START...");
            SwitchState(GameState.START);
            //TimerSystem.RunGameTimer();
            SwitchSceneWithMask("Game", true);
        }

        /// <summary>
        /// 关卡暂停
        /// </summary>
        public static void Pause()
        {
            Debug.Log("GAME PAUSE...");
            SwitchState(GameState.PAUSE);
            //TimerSystem.PauseRuntimeTimer();
        }

        public static void Resume()
        {
            Debug.Log("GAME RESUME...");
            SwitchState(GameState.START);
            //TimerSystem.RunGameTimer();
        }

        /// <summary>
        /// 关卡失败
        /// </summary>
        public static void Defeat()
        {
            Debug.Log("GAME DEFEAT...");
            SwitchState(GameState.DEFEAT);
            //TimerSystem.StopGameTimer();
        }

        /// <summary>
        /// 关卡胜利
        /// </summary>
        public static void Success()
        {
            Debug.Log("GAME SUCCESS...");
            SwitchState(GameState.SUCCESS);
            //TimerSystem.StopGameTimer();
        }

        /// <summary>
        /// 关卡停止
        /// </summary>
        public static void Stop()
        {
            Debug.Log("GAME STOP...");
            SwitchState(GameState.STOP);
            //TimerSystem.StopGameTimer();
        }

        /// <summary>
        /// 关卡重启
        /// </summary>
        public static void Restart()
        {
            Debug.Log("GAME RESTART...");
            SwitchState(GameState.RESTART);
            //TimerSystem.StopGameTimer();
            Start();
        }

        /// <summary>
        /// 退出关卡
        /// </summary>
        public static void Exit()
        {
            Debug.Log("GAME Exit...");
            SwitchState(GameState.EXIT);
            //TimerSystem.StopGameTimer();
            SwitchSceneWithMask("Start", false);
        }

        /// <summary>
        /// 开始执行异步任务
        /// </summary>
        /// <param name="backgroundDoWork"></param>
        /// <param name="backgroundProgressChanged"></param>
        public static void StartWork(DoWorkEventHandler backgroundDoWork,
            ProgressChangedEventHandler backgroundProgressChanged, RunWorkerCompletedEventHandler workCompleted)
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.WorkerReportsProgress = true;
            worker.DoWork += backgroundDoWork;
            worker.ProgressChanged += backgroundProgressChanged;
            worker.RunWorkerCompleted += workCompleted;

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="needReload"></param>
        public static void SwitchScene(string sceneName, bool needReload = false)
        {
            CachSystem.Clear();
            GuiSystem.Clear();
            LoadOrReloadScene(sceneName, needReload);
        }

        /// <summary>
        /// 以遮罩方式切换场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="needReload"></param>
        private static void SwitchSceneWithMask(string sceneName, bool needReload)
        {
            CachSystem.Clear();
            GuiSystem.Clear();
            GuiSystem.StartMask(true, MaskType.UTOD, LoadOrReloadScene, sceneName, needReload);
        }

        /// <summary>
        /// 加载或重载场景
        /// </summary>
        /// <param name="args"></param>
        private static void LoadOrReloadScene(params object[] args)
        {
            string sceneName = (string) args[0];
            bool needReload = (bool) args[1];

            if (needReload) ReLoadScene();
            else LoadScene(sceneName);
        }

        /// <summary>
        /// 添加场景加载时的监听
        /// </summary>
        /// <param name="myact"></param>
        public static void AddListenerOnSceneLoaded(UnityAction<Scene, LoadSceneMode> myact)
        {
            SceneManager.sceneLoaded += myact;
        }

        /// <summary>
        /// 重载当前场景
        /// </summary>
        private static void ReLoadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// 根据场景索引号载入场景
        /// </summary>
        /// <param name="buildindex"></param>
        public static void LoadScene(int buildindex)
        {
            SceneManager.LoadScene(buildindex);
        }

        /// <summary>
        /// 加载场景名
        /// </summary>
        /// <param name="sceneName"></param>
        private static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="buildindex"></param>
        /// <param name="timer"></param>
        /// <returns></returns>
        public static IEnumerator LoadSceneLater(int buildindex, float timer)
        {
            yield return new WaitForSeconds(timer);
            SceneManager.LoadScene(buildindex);
        }

        /// <summary>
        /// 获取当前场景的索引号
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetLoadedSceneId(this GameObject obj)
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        /// <summary>
        /// 获取游戏场景总数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetSceneCount(this GameObject obj)
        {
            return SceneManager.sceneCount;
        }
    }
}