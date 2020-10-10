using System.Runtime.InteropServices;

namespace CC
{
    public class IosBridger : SdkBridger
    {
#if !UNITY_EDITOR && UNITY_IOS
        [DllImport("__Internal")]
        public static extern void exchange(string productID);

        [DllImport("__Internal")]
        public static extern void restoreExchange(string productID);

        [DllImport("__Internal")]
        public static extern void showVideo(string tag);

        [DllImport("__Internal")]
        public static extern void showInter(string tag);

        [DllImport("__Internal")]
        public static extern bool hasAvailableInter();

        [DllImport("__Internal")]
        public static extern bool hasAvailableVideo();

        [DllImport("__Internal")]
        public static extern void share(string text, string subject, string[] files, int fileCount);

        [DllImport("__Internal")]
        public static extern void postDesignEvent(string eventType, string eventName, string eventData);

        [DllImport("__Internal")]
        public static extern void showToast(string title, string content);

        [DllImport("__Internal")]
        public static extern void showWaiting(string title, string content);

        [DllImport("__Internal")]
        public static extern void hideWaiting();

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        public override void PostDesignEvent(string eventType, string eventName, string eventData)
        {
            CCLog.I("发送打点事件");

            postDesignEvent(eventType, eventName, eventData);
        }

        /// <summary>
        /// 发起交易
        /// </summary>
        /// <param name="productID"></param>
        public override void Exchange(string productID)
        {
            CCLog.I("开始交易");
            exchange(productID);
        }

        public override void LoadVideoCB(string isSuccess)
        {
            CCLog.I("视频加载成功");
        }

        /// <summary>
        /// 开始播放视频
        /// </summary>
        /// <param name="adsTag"></param>
        public override void ShowVideo(string adsTag)
        {
            CCLog.I("开始播放视频");
            showVideo(adsTag);
        }

        /// <summary>
        /// 插屏加载完毕回调
        /// </summary>
        /// <param name="isSuccess"></param>
        public override void LoadInterCB(string isSuccess)
        {
            CCLog.I("插屏加载成功");
        }

        /// <summary>
        /// 开始播放插屏
        /// </summary>
        /// <param name="adsTag"></param>
        public override void ShowInter(string adsTag)
        {
            CCLog.I("开始播放插屏");
            showInter(adsTag);
        }

        public override bool IsVideoReady()
        {
            return hasAvailableVideo();
        }

        public override bool IsInterReady()
        {
            return hasAvailableInter();
        }

        /// <summary>
        /// 截图方式开始分享
        /// </summary>
        /// <param name="args"></param>
        protected override void StartShare(params object[] args)
        {
            var title = (string) args[0];
            var text = (string) args[1];
            var files = (string[]) args[2];
            var mimes = (string[]) args[3];

            share(text, SHARE_URL, files, files.Length);
        }

        /// <summary>
        /// 显示IOS浮窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public override void ShowToast(string title, string content)
        {
            showToast(title, content);
        }

        /// <summary>
        /// 显示IOS加载窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public override void ShowWaiting(string title, string content)
        {
            showWaiting(title, content);
        }

        /// <summary>
        /// 隐藏IOS加载窗
        /// </summary>
        public override void HideWaiting()
        {
            hideWaiting();
        }
#endif
    }
}