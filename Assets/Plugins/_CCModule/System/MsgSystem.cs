/*
 * 使用范例:
 	1. MsgSystem.Add<GameObject>("prop collected", PropCollected);
 	   MsgSystem.Dispatch<GameObject>("prop collected", prop);
 	2. MsgSystem.Add<float>("speed changed", SpeedChanged);
 	   MsgSystem.Dispatch<float>("speed changed", 0.5f);
 * 
 * MsgSystem每次在进入新场景时会自动进行清理Clear
 * 需在清理时保留的监听，请使用MsgSystem.MarkAsPersistent(string)
 * 
 */

//#define REQUIRE_LISTENER

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CC
{
    public delegate T Callback<T>(params object[] arg);

    public delegate void Callback(params object[] arg);

    public static class MsgSystem
    {
        #region 参数列表

        public static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>(); //消息列表
        public static List<string> persistentMessages = new List<string>(); //常驻消息列表（在清理消息列表时会被排除）

        #endregion

        static MsgSystem()
        {
           // Debug.Log("MsgSystem Awake");
            SceneManager.sceneLoaded += OnSceneLoaded;  //注释掉这行，重新载入场景后会出现消息没有响应的情况。如有问题可以尝试注释此行
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
        }

        public static void OnSceneLoaded(Scene unused, LoadSceneMode unusemode)
        {
            Clear();
        }

        #region 辅助函数

        /// <summary>
        /// 标注常驻消息
        /// </summary>
        /// <param name="eventType"></param>
        public static void MarkAsPersistent(string eventType)
        {
            persistentMessages.Add(eventType);
        }

        /// <summary>
        /// 清理消息列表
        /// </summary>
        public static void Clear()
        {
            var removeList = new List<string>();

            foreach (var pair in eventTable)
            {
                var wasFound = false;

                foreach (var message in persistentMessages)
                {
                    if (pair.Key != message) continue;
                    wasFound = true;
                    break;
                }

                if (!wasFound) removeList.Add(pair.Key);
            }

            foreach (var message in removeList)
            {
                eventTable.Remove(message);
            }
        }

        /// <summary>
        /// 打印键值对信息
        /// </summary>
        public static void PrintEventTable()
        {
            Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (var pair in eventTable)
            {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            Debug.Log("\n");
        }

        #endregion

        #region 添加或删除监听

        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public static void Add(string eventType, Callback handler)
        {
            OnAdding(eventType, handler);
            eventTable[eventType] = (Callback) eventTable[eventType] + handler;
            Debug.Log("---AddMsg:  " + eventType);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public static void Remove(string eventType, Callback handler)
        {
            OnRemoving(eventType, handler);
            eventTable[eventType] = (Callback) eventTable[eventType] - handler;
            OnRemoved(eventType);
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="arg"></param>
        public static void Dispatch(string eventType, params object[] arg)
        {
          //  Debug.Log("---DispatchMsg:  " + eventType);
            OnDispatching(eventType);

            Delegate d;

            if (eventTable.TryGetValue(eventType, out d))
            {
                if (d is Callback callback) callback(arg);
                else
                    Debug.Log(
(object)$"Dispatching message \"{eventType}\" but listeners have a different signature than the broadcaster.");
            }
        }

        #endregion

        #region 消息日志及异常处理

        /// <summary>
        /// 添加监听时的异常处理
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="listenerBeingAdded"></param>
        private static void OnAdding(string eventType, Delegate listenerBeingAdded)
        {
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }

            Delegate d = eventTable[eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                Debug.Log(
(object)$"Attempting to add listener with inconsistent signature for event type {eventType}. Current listeners have type {d.GetType().Name} and listener being added has type {listenerBeingAdded.GetType().Name}");
            }
        }

        /// <summary>
        /// 移除监听时的异常处理
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="listenerBeingRemoved"></param>
        private static void OnRemoving(string eventType, Delegate listenerBeingRemoved)
        {
            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];

                if (d == null)
                {
                    Debug.Log(
(object)$"Attempting to remove listener with for event type \"{eventType}\" but current listener is null.");
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    Debug.Log(
(object)$"Attempting to remove listener with inconsistent signature for event type {eventType}. Current listeners have type {d.GetType().Name} and listener being removed has type {listenerBeingRemoved.GetType().Name}");
                }
            }
            else
            {
                Debug.Log(
(object)$"Attempting to remove listener for type \"{eventType}\" but MsgSystem doesn't know about this event type.");
            }
        }

        /// <summary>
        /// 监听被移除后从字典中移除对应键值对
        /// </summary>
        /// <param name="eventType"></param>
        private static void OnRemoved(string eventType)
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }

        /// <summary>
        /// 广播消息时的异常处理
        /// </summary>
        /// <param name="eventType"></param>
        private static void OnDispatching(string eventType)
        {
#if REQUIRE_LISTENER
            if (!eventTable.ContainsKey(eventType))
            {
                Debug.Log(
                    $"Dispatching message \"{eventType}\" but no listener found. Try marking the message with MsgSystem.MarkAsPersistent.");
            }
#endif
        }

        #endregion
    }
}