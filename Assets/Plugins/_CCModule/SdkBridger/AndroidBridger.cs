using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class AndroidBridger : SdkBridger
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        private readonly AndroidJavaClass appSupport = new AndroidJavaClass("com.koko.sdkBridger.AppSupport");

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        public override void PostDesignEvent(string eventType, string eventName, string eventData)
        {
            CCLog.I("发送打点事件");
            appSupport.CallStatic("postDesignEvent", eventType, eventName, eventData);
        }

        /// <summary>
        /// 开始交易
        /// </summary>
        /// <param name="productId"></param>
        public override void Exchange(string productId)
        {
            appSupport.CallStatic("exchange", productId);
        }

        public override void LoadVideoCB(string isSuccess)
        {
            CCLog.I("视频加载成功");
        }

        /// <summary>
        /// 开始播放视频
        /// </summary>
        /// <param name="tag"></param>
        public override void ShowVideo(string tag)
        {
            CCLog.I("开始播放视频");
            appSupport.CallStatic("showVideo", tag);
        }

        /// <summary>
        /// 加载插屏结束回调
        /// </summary>
        /// <param name="isSuccess"></param>
        public override void LoadInterCB(string isSuccess)
        {
            CCLog.I("插屏加载成功");
        }

        /// <summary>
        /// 开始播放视频
        /// </summary>
        /// <param name="adsTag"></param>
        public override void ShowInter(string adsTag)
        {
            CCLog.I("开始播放插屏");
            appSupport.CallStatic("showInter", adsTag);
        }

        /// <summary>
        /// 视频是否准备就绪
        /// </summary>
        /// <returns></returns>
        public override bool IsVideoReady()
        {
            return appSupport.CallStatic<bool>("hasAvailableVideo");
        }

        /// <summary>
        /// 插屏是否准备就绪
        /// </summary>
        /// <returns></returns>
        public override bool IsInterReady()
        {
            return appSupport.CallStatic<bool>("hasAvailableInter");
        }

        /// <summary>
        /// 截图方式开始分享
        /// </summary>
        /// <param name="args"></param>
        protected override void StartShare(params object[] args)
        {
            var title = (string) args[0];
            var text = (string) args[1];
            var file = (string) args[3];
            var mime = (string) args[4];

            appSupport.CallStatic("share", title, text, SHARE_URL, file, mime);
        }

        /// <summary>
        /// 显示浮窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public override void ShowToast(string title, string content)
        {
            appSupport.CallStatic("showToast", title, content);
        }

        /// <summary>
        /// 显示加载窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public override void ShowWaiting(string title, string content)
        {
            appSupport.CallStatic("showWaiting", title, content);
        }

        /// <summary>
        /// 隐藏IOS加载窗
        /// </summary>
        public override void HideWaiting()
        {
            appSupport.CallStatic("hideWaiting");
        }

        /// <summary>
        /// 震动
        /// </summary>
        public override void Vibrate(int vibrateTime)
        {
            appSupport.CallStatic("vibrateStart", vibrateTime);
        }

        /// <summary>
        /// 停止震动
        /// </summary>
        public override void VibrateCancel()
        {
            appSupport.CallStatic("vibrateCancel");
        }

        /// <summary>
        /// Unity一般在主线程下运行，所以一般不需要关注此方法
        /// 多线程环境下，在调用SDK前需使用此方法绑定当前线程,否则可能导致内存泄漏或程序crash
        /// 主线程环境下不需要调用此方法，否则也可能导致程序crash
        /// </summary>
        public void AttachCurrentThread()
        {
            AndroidJNI.AttachCurrentThread();
        }

        /// <summary>
        /// 多线程环境下，取消绑定当前线程
        /// </summary>
        public void DetachCurrentThread()
        {
            AndroidJNI.DetachCurrentThread();
        }

        /// <summary>
        /// Dictionary转Map
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public AndroidJavaObject DicToMap(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                map.Call<string>("put", pair.Key, pair.Value);
            }

            return map;
        }
#endif
    }
}