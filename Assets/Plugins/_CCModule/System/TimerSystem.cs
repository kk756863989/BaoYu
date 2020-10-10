using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public enum TimerType
    {
        NONE, //占位计时器
        COMMON, //常驻计时器,无论如何都会执行,不会被其他迭代
        MAIN, //不兼容计时器,会被其他不兼容计时器迭代
        GAME //不兼容计时器,会被其他不兼容计时器迭代
    }

    public enum LoopType
    {
        LOOP = 0, //循环计时器
        ONCE = 1 //一次性计时器，如需循环多次则设置为大于1的数值即可
    }

    public enum RunResult
    {
        STATIC = 0, //未运行
        CONTINUE = 1, //继续
        BREAK = 2 //停止
    }

    public static class TimerSystem
    {
        private const float DelayTimeOfMainLoop = 0.1f; //主循环运行间隔时长
        private static float timeScale = 1.0f; //时间缩放参数
        private static readonly Dictionary<string, Dictionary<string, CCTimer>> Timers;
        private static readonly TimerSchedule LoopTimer;
        private static int timerCount;
        private static TimerType curOnlyTimer = TimerType.MAIN;

        /// <summary>
        /// 主循环计时器
        /// 1.检查节点
        /// 2.定义循环计时器
        /// 3.定义主循环逻辑
        /// 4.先清空所有运行的定时回调
        /// 5.开始运行主循环回调，间隔时间0.1秒
        /// </summary>
        static TimerSystem()
        {
            //Debug.Log("TimerSystem Awake");
            Timers = new Dictionary<string, Dictionary<string, CCTimer>>
            {
                {GetTimerTypeName(TimerType.COMMON), new Dictionary<string, CCTimer>()},
                {GetTimerTypeName(TimerType.MAIN), new Dictionary<string, CCTimer>()},
                {GetTimerTypeName(TimerType.GAME), new Dictionary<string, CCTimer>()},
                {GetTimerTypeName(TimerType.NONE), new Dictionary<string, CCTimer>()}
            };

            var helper = new GameObject("TimerHelper");
            Object.DontDestroyOnLoad(helper);
            LoopTimer = helper.AddComponent<TimerSchedule>();
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            Start();
        }

        /// <summary>
        /// 开始循环计时.
        /// </summary>
        public static void Start()
        {
            LoopTimer.InvokeRepeating("MainLoop", 0, DelayTimeOfMainLoop * timeScale);
        }

        /// <summary>
        /// 结束循环计时.
        /// </summary>
        public static void Stop()
        {
            LoopTimer.CancelInvoke("MainLoop");
        }

        /// <summary>
        /// 计时器时长缩放.
        /// </summary>
        public static void TimeScale(float scale)
        {
            Stop();
            timeScale = scale;
            Start();
        }

        /// <summary>
        /// 主循环
        /// </summary>
        public static void MainLoop()
        {
            RunStayTimers();
            RunOnlyTimers();
        }

        /// <summary>
        /// 获取计时器字典类型名
        /// </summary>
        /// <param name="timerType"></param>
        /// <returns></returns>
        private static string GetTimerTypeName(TimerType timerType)
        {
            string result = "Undefined";

            switch (timerType)
            {
                case TimerType.COMMON:
                    result = "Common";
                    break;
                case TimerType.MAIN:
                    result = "Main";
                    break;
                case TimerType.GAME:
                    result = "Game";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 依次执行当前类型的所有计时器
        /// </summary>
        public static void RunStayTimers()
        {
            string timerKey = GetTimerTypeName(TimerType.COMMON);
            if (!Timers.ContainsKey(timerKey)) return;

            Dictionary<string, CCTimer> runTimers = Timers[timerKey];

            if (runTimers.Count == 0) return;

            List<string> arr = new List<string>(runTimers.Keys);
            string pair;

            for (int i = 0, l = arr.Count; i < l; i++)
            {
                pair = arr[i];
                if (!runTimers.ContainsKey(pair) || runTimers[pair] == null) continue;
                Run(TimerType.COMMON, pair);
            }
        }

        /// <summary>
        /// 依次执行当前类型的所有计时器
        /// </summary>
        public static void RunOnlyTimers()
        {
            string timerKey = GetTimerTypeName(curOnlyTimer);
            if (!Timers.ContainsKey(timerKey)) return;

            Dictionary<string, CCTimer> runTimers = Timers[timerKey];

            if (runTimers.Count == 0) return;

            List<string> arr = new List<string>(runTimers.Keys);
            string pair;

            for (int i = 0, l = arr.Count; i < l; i++)
            {
                pair = arr[i];
                if (!runTimers.ContainsKey(pair) || runTimers[pair] == null) continue;
                Run(curOnlyTimer, pair);
            }
        }

        //#region 不兼容计时器---游戏内即时切换计时器
        /// <summary>
        /// 主场景计时器
        /// 主场景计时器\游戏场景计时器与战斗计时器不兼容,三者只会同时存在一种,相互切换
        /// 主场景计时器计时器通常用来处理角色身上的定时功能,如打坐\挂机\修炼\疗伤CD等
        /// </summary>
        public static void RunMainTimer()
        {
            curOnlyTimer = TimerType.MAIN;
        }

        /// <summary>
        /// 游戏场景计时器
        /// </summary>
        public static void RunGameTimer()
        {
            curOnlyTimer = TimerType.GAME;
        }

        /// <summary>
        /// 停止主场景计时器
        /// </summary>
        public static void StopMainTimer()
        {
            ClearCCTimers(TimerType.MAIN);
        }

        /// <summary>
        /// 停止游戏场景计时器
        /// </summary>
        public static void StopGameTimer()
        {
            ClearCCTimers(TimerType.GAME);
        }

        /// <summary>
        /// 暂停运行时不兼容计时器
        /// </summary>
        public static void PauseRuntimeTimer()
        {
            curOnlyTimer = TimerType.NONE;
        }

        //#endregion
        //#region 添加/移除/清除计时器
        /// <summary>
        /// 添加计时器
        /// </summary>
        /// <param name="timerType"></param>
        /// <param name="loopType"></param>
        /// <param name="delayTime"></param>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CCTimer Add(TimerType timerType, int loopType, float delayTime, Callback callback,
            params object[] args)
        {
            var uid = GenerateUid();
            var ccTimer = new CCTimer(uid, loopType, delayTime, callback, args);

            AddCCTimer(timerType, uid, ccTimer);
            return ccTimer;
        }

        /// <summary>
        /// 移除计时器
        /// </summary>
        /// <param name="timerType"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static CCTimer Remove(TimerType timerType, Callback callback)
        {
            var timerKey = GetTimerTypeName(timerType);
            if (!Timers.ContainsKey(timerKey)) return null;

            CCTimer ccTimer = null;
            var arr = new List<string>(Timers[timerKey].Keys);

            for (int i = 0, l = arr.Count; i < l; i++)
            {
                var pair = arr[i];
                ccTimer = GetCCTimer(timerType, pair);
                if (ccTimer == null) continue;
                if (!ccTimer.Compare(callback)) continue;

                RemoveCCTimer(timerType, pair);
                break;
            }

            return ccTimer;
        }

        /// <summary>
        /// 按UID来移除计时器
        /// </summary>
        /// <param name="timerType">Timer type.</param>
        /// <param name="uid">Uid.</param>
        public static void RemoveByUid(TimerType timerType, string uid)
        {
            RemoveCCTimer(timerType, uid);
        }

        /// <summary>
        /// 执行计时器
        /// </summary>
        /// <returns>The run.</returns>
        /// <param name="timerType">Timer type.</param>
        /// <param name="uid">Uid.</param>
        private static RunResult Run(TimerType timerType, string uid)
        {
            var ccTimer = GetCCTimer(timerType, uid);
            var result = ccTimer.Execute();

            if (result == RunResult.BREAK) RemoveCCTimer(timerType, uid);
            return result;
        }

        /// <summary>
        /// 添加观察者
        /// </summary>
        /// <returns>The obserser.</returns>
        /// <param name="timerType">Timer type.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="ccTimer">Observer.</param>
        private static CCTimer AddCCTimer(TimerType timerType, string uid, CCTimer ccTimer)
        {
            var timerName = GetTimerTypeName(timerType);
            if (!Timers.ContainsKey(timerName)) Timers[timerName] = new Dictionary<string, CCTimer>();
            Timers[timerName][uid] = ccTimer;
            return ccTimer;
        }

        /// <summary>
        /// 获取观察者
        /// </summary>
        /// <returns>The ccTimer.</returns>
        /// <param name="timerType">Timer type.</param>
        /// <param name="uid">Uid.</param>
        private static CCTimer GetCCTimer(TimerType timerType, string uid)
        {
            var timerName = GetTimerTypeName(timerType);
            return !Timers.ContainsKey(timerName) ? null : Timers[timerName][uid];
        }

        /// <summary>
        /// 用uid移除对应计时器
        /// </summary>
        /// <param name="timerType">Timer type.</param>
        /// <param name="uid">Uid.</param>
        private static void RemoveCCTimer(TimerType timerType, string uid)
        {
            var timerName = GetTimerTypeName(timerType);
            if (!Timers.ContainsKey(timerName)) return;

            Timers[timerName].Remove(uid);
        }

        /// <summary>
        /// Clears all the CCTimers.
        /// </summary>
        /// <param name="timerType">Timer type.</param>
        private static void ClearCCTimers(TimerType timerType)
        {
            var timerName = GetTimerTypeName(timerType);
            if (!Timers.ContainsKey(timerName)) return;

            Timers.Remove(timerName);
        }

        /// <summary>
        /// 清空所有计时器
        /// </summary>
        public static void Clear()
        {
            Timers.Clear();
            timerCount = 0;
        }

        /// <summary>
        /// 生成计时器唯一ID
        /// </summary>
        /// <returns>The uid.</returns>
        private static string GenerateUid()
        {
            timerCount++;
            return "uid" + timerCount;
        }

        //#endregion
    }

    /// <summary>
    /// 计时器节点组件，用于启用/停止主循环
    /// </summary>
    internal class TimerSchedule : MonoBehaviour
    {
        private void MainLoop()
        {
            TimerSystem.MainLoop();
        }
    }

    /// <summary>
    /// 计时器观察者
    /// </summary>
    public class CCTimer
    {
        private string Uid { get; }
        private float StartTime { get; set; }
        private float DelayTime { get; }
        private TimerType timerType;
        private int Type { get; set; }
        private Callback MCallback { get; }
        private object[] Args { get; }

        public CCTimer(string uid, int loopType, float delayTime, Callback callback, params object[] args)
        {
            Uid = uid;
            Type = loopType;
            StartTime = 0;
            DelayTime = delayTime;
            MCallback = callback;
            Args = args;
        }

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <returns>The notify.</returns>
        public RunResult Execute()
        {
            var result = RunResult.STATIC;

            StartTime++;
            if (StartTime < DelayTime) return result;

            result = Type == 1 ? RunResult.BREAK : RunResult.CONTINUE;
            if (Type > (int) LoopType.ONCE) Type--;
            StartTime = 0;
            MCallback?.Invoke(Args);
            return result;
        }

        /// <summary>
        /// 上下文比较
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool Compare(Callback callback)
        {
            return MCallback.Equals(callback);
        }

        /// <summary>
        /// 获取唯一ID
        /// </summary>
        /// <returns></returns>
        public string GetUid()
        {
            return Uid;
        }
    }
}