using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public enum ProductType
    {
        PRODUCT_IGNORE,
        PRODUCT_SUBSCRIBE,
        PRODUCT_GIFTPACK,
        PRODUCT_RESTORE
    }

    public enum ShareType
    {
        SHARE_COMMON,
        SHARE_REWARD
    }

    public enum AdsType
    {
        ADS_VIDEO,
        ADS_INTER,
        ADS_BANNER
    }

    public class SdkBridger : MonoBehaviour
    {
        private static SdkBridger instance;

        public static SdkBridger GetInstance()
        {
            return instance;
        }

#if UNITY_IOS
        public const string SHARE_URL = "https://itunes.apple.com/us/app/id1469521739";
#else
        public const string SHARE_URL = "https://play.google.com/store/apps/details?id=com.entdream.candymerge";
#endif

        public readonly string[] ingotIds = new string[6]
        {
            "INGOT0001",
            "INGOT0002",
            "INGOT0003",
            "INGOT0004",
            "INGOT0005",
            "INGOT0006",
        };

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            GameObject obj = new GameObject("SdkBridger");

            DontDestroyOnLoad(obj);

#if UNITY_EDITOR
            instance = obj.AddComponent<SdkBridger>();
#elif UNITY_IOS
            instance = obj.AddComponent<IosBridger>();
#elif UNITY_ANDROID
            instance = obj.AddComponent<AndroidBridger>();
#else
            instance = obj.AddComponent<SdkBridger>();
#endif
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        public virtual void PostDesignEvent(string eventType, string eventName, string eventData)
        {
            Debug.Log("发送打点事件：" + eventType + eventName + eventData);
        }

        /// <summary>
        /// 交易
        /// </summary>
        /// <param name="productId"></param>
        public virtual void Exchange(string productId)
        {
            Debug.Log("此平台不支持交易，直接返回成功");

            var table = new Hashtable
            {
                {"productID", productId},
                {"purchaseDate", UnixUtility.GetCurrentStamp().ToString()},
                {"isSuccess", "true"}
            };


            ExchangeCB(table.ToJsonData());
        }

        /// <summary>
        /// 交易回调
        /// </summary>
        /// <param name="jsonParams"></param>
        public void ExchangeCB(string jsonParams)
        {
            try
            {
                JsonData.ExchangeData exchangeData = JsonConvert.DeserializeObject<JsonData.ExchangeData>(jsonParams);
                MsgSystem.Dispatch(
                    ProductType.PRODUCT_IGNORE.EnumToString(),
                    exchangeData.productID,
                    int.Parse(exchangeData.purchaseDate),
                    exchangeData.isSuccess == "true"
                );
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        /// <summary>
        /// 恢复所有一次性商品
        /// </summary>
        /// <param name="exchangeIds"></param>
        public void RestoreAllExchangeCB(string exchangeIds)
        {
            string[] ids = exchangeIds.Split(',');

            Hashtable table;

            for (int i = 0; i < ids.Length; i++)
            {
                table = new Hashtable();
                table.Add("productID", ids[i]);
                table.Add("purchaseDate", UnixUtility.GetCurrentStamp().ToString());
                table.Add("isSuccess", "true");

                RestoreExchangeCB(table.ToJsonData());
            }
        }

        /// <summary>
        /// 恢复一次性商品
        /// </summary>
        /// <param name="jsonParams"></param>
        public void RestoreExchangeCB(string jsonParams)
        {
            try
            {
                JsonData.ExchangeData exchangeData = JsonConvert.DeserializeObject<JsonData.ExchangeData>(jsonParams);
                MsgSystem.Dispatch(
                    ProductType.PRODUCT_RESTORE.EnumToString(),
                    exchangeData.productID,
                    int.Parse(exchangeData.purchaseDate),
                    true
                );
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        /// <summary>
        /// 加载视频结束回调
        /// </summary>
        /// <param name="isSuccess"></param>
        public virtual void LoadVideoCB(string isSuccess)
        {
            Debug.Log("此平台不支持视频，直接返回视频加载成功");
        }

        /// <summary>
        /// 显示视频
        /// </summary>
        /// <param name="adsTag"></param>
        public virtual void ShowVideo(string adsTag)
        {
            Debug.Log("此平台不支持视频，直接返回成功");

            var table = new Hashtable {{"adsTag", adsTag}, {"isSuccess", "true"}};

            ShowVideoCB(table.ToJsonData());
        }

        /// <summary>
        /// 视频播放完毕回调
        /// </summary>
        /// <param name="jsonParams"></param>
        public virtual void ShowVideoCB(string jsonParams)
        {
            try
            {
                Debug.Log((object)jsonParams);
                JsonData.VideoData videoData = JsonConvert.DeserializeObject<JsonData.VideoData>(jsonParams);

                MsgSystem.Dispatch(AdsType.ADS_VIDEO.EnumToString(), videoData.tag, videoData.isSuccess == "true");
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        /// <summary>
        /// 插屏播放结束回调
        /// </summary>
        /// <param name="isSuccess"></param>
        public virtual void LoadInterCB(string isSuccess)
        {
            Debug.Log("此平台不支持插屏，直接返回视插屏加载成功");
        }

        /// <summary>
        /// 显示插屏
        /// </summary>
        /// <param name="adsTag"></param>
        public virtual void ShowInter(string adsTag)
        {
            Debug.Log("此平台不支持插屏，直接返回成功");

            var table = new Hashtable {{"adsTag", adsTag}, {"isSuccess", "true"}};

            ShowInterCB(table.ToJsonData());
        }

        /// <summary>
        /// 插屏播放结束回调
        /// </summary>
        /// <param name="jsonParams"></param>
        public virtual void ShowInterCB(string jsonParams)
        {
            try
            {
                Debug.Log((object)jsonParams);
                var videoData = JsonConvert.DeserializeObject<JsonData.VideoData>(jsonParams);

                MsgSystem.Dispatch(AdsType.ADS_INTER.EnumToString(), videoData.tag, videoData.isSuccess == "true");
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        /// <summary>
        /// 视屏是否准备就绪
        /// </summary>
        /// <returns></returns>
        public virtual bool IsVideoReady()
        {
            Debug.Log("此平台不支持视频，直接返回成功");

            return false;
        }

        /// <summary>
        /// 插屏是否准备就绪
        /// </summary>
        /// <returns></returns>
        public virtual bool IsInterReady()
        {
            Debug.Log("此平台不支持插屏，直接返回成功");

            return false;
        }

        /// <summary>
        /// 开始分享
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public void Share(string title, string text)
        {
            Debug.Log("开始分享");
            StartCoroutine(ShareWithScreenShot(StartShare, title, text));
        }

        /// <summary>
        /// 截图分享
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private IEnumerator ShareWithScreenShot(Callback callback, string title, string text)
        {
            ScreenCapture.CaptureScreenshot("ScreensShot.png");
            yield return new WaitForSeconds(1f);

            var mime = "image/png";
            var filePath = Application.persistentDataPath + "/" + "ScreensShot.png";

            callback?.Invoke(title, text, SHARE_URL, filePath, mime);

            yield return new WaitUntil(() => Application.isFocused);
        }

        /// <summary>
        /// 启动分享
        /// </summary>
        /// <param name="args"></param>
        protected virtual void StartShare(params object[] args)
        {
            Debug.Log("此平台不支持分享，直接返回成功");
            var table = new Hashtable {{"isSuccess", "true"}};

            ShareCB(table.ToJsonData());
        }

        /// <summary>
        /// 分享回调
        /// </summary>
        /// <param name="jsonParams"></param>
        public virtual void ShareCB(string jsonParams)
        {
            try
            {
                JsonData.ShareData shareData = JsonConvert.DeserializeObject<JsonData.ShareData>(jsonParams);

                MsgSystem.Dispatch(ShareType.SHARE_COMMON.EnumToString(), shareData.isSuccess == "true");
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <param name="tag"></param>
        public virtual void Ladder(string tag)
        {
            Debug.Log("此平台不支持查看排行，直接返回成功");
            LadderCB("true");
        }

        /// <summary>
        /// 排名回调
        /// </summary>
        /// <param name="jsonParams"></param>
        public virtual void LadderCB(string jsonParams)
        {
            Debug.Log("此平台不支持查看排行，假装看到了很多人吧");
        }

        /// <summary>
        /// 显示浮窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public virtual void ShowToast(string title, string content)
        {
            Debug.Log("此平台不支持原生toast");
        }

        /// <summary>
        /// 显示loading
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public virtual void ShowWaiting(string title, string content)
        {
            Debug.Log("此平台不支持原生loading");
        }

        /// <summary>
        /// 隐藏loading
        /// </summary>
        public virtual void HideWaiting()
        {
            Debug.Log("此平台不支持原生loading");
        }

        /// <summary>
        /// 震动
        /// </summary>
        public virtual void Vibrate(int vibrateTime)
        {
            Debug.Log("此平台不支持震动，假装震了一下");
        }

        /// <summary>
        /// 取消震动
        /// </summary>
        public virtual void VibrateCancel()
        {
            Debug.Log("此平台不支持震动，假装停止了");
        }

        /// <summary>
        /// 游戏暂停和继续时的处理
        /// </summary>
        /// <param name="pauseStatus"></param>
        void OnApplicationPause(bool pauseStatus)
        {
            //游戏未运行起来时，逻辑部分不需要做处理，此时消息系统可能还没有初始化
            if (!CCGame.IsRunning()) return;
#if UNITY_EDITOR
#elif UNITY_IOS || UNITY_ANDROID
    if(pauseStatus)
    {
        //TODO Activity的onPause()方法
        MsgSystem.Dispatch("GAME_PAUSE");
    }
    else
    {
        //TODO Activity的onResume()方法
        MsgSystem.Dispatch("GAME_RESUME");
    }
#endif
        }

        void OnApplicationFocus()
        {
        }
    }
}