using System.Collections;
using UnityEngine;

namespace CC
{
    public static class AnimManager
    {
        public const int FPS = 60;

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="duration"></param>
        /// <param name="fromPos"></param>
        /// <param name="targetPos"></param>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerator Move(Transform tran, float duration, Vector3 fromPos, Vector3 targetPos,
            Callback callback = null, params object[] args)
        {
            tran.localPosition = fromPos;
            duration = 1 - duration;

            while (Vector3.Distance(tran.localPosition, targetPos) > 0.5f)
            {
                tran.localPosition = Vector3.Lerp(tran.localPosition, targetPos, duration);

                yield return new WaitForEndOfFrame();
            }

            tran.localPosition = targetPos;
            callback?.Invoke(args);
        }

        /// <summary>
        /// 移动回弹
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="duration"></param>
        /// <param name="fromPos"></param>
        /// <param name="targetPos"></param>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerator ElasticMove(Transform tran, float duration, Vector3 fromPos, Vector3 targetPos,
            Callback callback = null, params object[] args)
        {
            var distance = Vector3.Distance(targetPos, fromPos);
            var moveDuration = 1 - duration;
            var elasticDuration = moveDuration * 2;
            var frameTime = 0f;
            var toTargetPos = fromPos + (targetPos - fromPos).normalized * distance * 1.02f;

            tran.localPosition = fromPos;

            while (Vector3.Distance(tran.localPosition, toTargetPos) > 1f)
            {
                frameTime += Time.deltaTime;

                if (frameTime > 1.0f / FPS)
                {
                    frameTime = 0;
                    tran.localPosition = Vector3.Lerp(tran.localPosition, toTargetPos, moveDuration);
                }

                yield return new WaitForEndOfFrame();
            }

            tran.localPosition = toTargetPos;
            frameTime = 0;

            while (Vector3.Distance(tran.localPosition, targetPos) > 1f &&
                   Utils.IsOnDirection(tran.localPosition, fromPos, targetPos))
            {
                frameTime += Time.deltaTime;

                if (frameTime > 1.0f / FPS)
                {
                    frameTime = 0;
                    tran.localPosition = Vector3.Lerp(tran.localPosition, targetPos, elasticDuration);
                }

                yield return new WaitForEndOfFrame();
            }

            tran.localPosition = targetPos;

            callback?.Invoke(args);
        }

        /// <summary>
        /// 缩放回弹
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="duration"></param>
        /// <param name="fromScale"></param>
        /// <param name="targetScale"></param>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerator ElasticScale(Transform tran, float duration, float fromScale, float targetScale,
            Callback callback = null, params object[] args)
        {
            var scaleUpOrDown = (targetScale - fromScale).Normalize();
            var toTargetScale = targetScale * (1 + scaleUpOrDown * 0.1f);
            var scaleDuration = duration * 2 / 3;
            var elasticDuration = duration - scaleDuration;
            var deltaScale = toTargetScale / scaleDuration / FPS;

            var tempScale = scaleUpOrDown * deltaScale;

            tran.localScale = new Vector3(fromScale, fromScale, 1);

            callback?.Invoke(args);

            if (tran.localScale.x < targetScale)
            {
                while (tran.localScale.x < targetScale)
                {
                    yield return new WaitForEndOfFrame();
                    tran.localScale += new Vector3(tempScale, tempScale, 0);
                }
            }
            else if (tran.localScale.x > targetScale)
            {
                while (tran.localScale.x > targetScale)
                {
                    yield return new WaitForEndOfFrame();
                    tran.localScale += new Vector3(tempScale, tempScale, 0);
                }
            }
            tran.SetScale(targetScale, targetScale, 1);

            //while (tran.localScale.x < targetScale)
            //{
            //    Debug.Log("xiao");
            //    yield return new WaitForEndOfFrame();

            //    tran.localScale += new Vector3(tempScale, tempScale, 0);
            //}
            //tran.SetScale(targetScale, targetScale, 1);

            //tran.SetScale(toTargetScale, toTargetScale, 1);
            //deltaScale = -targetScale / elasticDuration / FPS;
            //tempScale = scaleUpOrDown * deltaScale;

            //while (tran.localScale.x > targetScale)
            //{
            //    Debug.Log("da");
            //    yield return new WaitForEndOfFrame();

            //    tran.localScale += new Vector3(tempScale, tempScale, 0);
            //}
            //tran.SetScale(targetScale, targetScale, 1);

            //callback?.Invoke(args);
        }

        public static IEnumerator DelayTime(float duration, Callback callback = null, params object[] args)
        {
            yield return new WaitForSeconds(duration);

            callback?.Invoke(args);
        }

        /// <summary>
        /// 震动
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="duration"></param>
        /// <param name="shakeDelta"></param>
        /// <returns></returns>
        public static IEnumerator Shake(Transform tran, float duration, float shakeDelta)
        {
            var originPos = tran.localPosition;
            float frameTime = 0;

            while (duration > 0)
            {
                yield return new WaitForEndOfFrame();
                duration -= Time.deltaTime;
                frameTime += Time.deltaTime;

                if (!(frameTime > 1.0f / FPS)) continue;

                frameTime = 0;
                var deltaMoveX = shakeDelta * (Random.value - 0.5f);
                var deltaMoveY = shakeDelta * (Random.value - 0.5f);
                tran.localPosition += new Vector3(deltaMoveX, deltaMoveY, 0);
            }

            while (Vector3.Distance(tran.localPosition, originPos) > 0.1f)
            {
                yield return new WaitForEndOfFrame();

                tran.localPosition = Vector3.Lerp(tran.localPosition, originPos, 0.5f);
            }

            tran.localPosition = originPos;
        }

        public static IEnumerator RotatePingPong(Transform tran, float duration, Callback callback = null,
            params object[] args)
        {
            float time;
            var count = 24;
            var cachTime = duration;
            var onceLoop = duration / count;
            var anglePerframe = 2;
            var deltaMoveXPerframe = 1;
            var deltaMoveYPerframe = 0.2f;
            var maxAngle = 10;
            var originPos = tran.localPosition;

            while (cachTime > 0)
            {
                time = onceLoop;
                while (time > 0)
                {
                    cachTime -= Time.deltaTime;
                    time -= Time.deltaTime;
                    tran.localPosition += new Vector3(deltaMoveXPerframe, deltaMoveYPerframe, 0);
                    tran.Rotate(new Vector3(0, 0, anglePerframe), Space.Self);
                    yield return new WaitForEndOfFrame();
                }

                tran.rotation = Quaternion.Euler(0, 0, maxAngle);
                time = onceLoop;

                while (time > 0)
                {
                    cachTime -= Time.deltaTime;
                    time -= Time.deltaTime;
                    tran.localPosition += new Vector3(-deltaMoveXPerframe, deltaMoveYPerframe, 0);
                    tran.Rotate(new Vector3(0, 0, -anglePerframe), Space.Self);
                    yield return new WaitForEndOfFrame();
                }

                tran.rotation = Quaternion.Euler(0, 0, -maxAngle);
            }

            time = duration / 12;

            while (time > 0)
            {
                time -= Time.deltaTime;
                tran.localPosition += new Vector3(deltaMoveXPerframe, deltaMoveYPerframe, 0);
                tran.Rotate(new Vector3(0, 0, anglePerframe), Space.Self);
                yield return new WaitForEndOfFrame();
            }

            tran.localPosition += originPos;
            tran.rotation = Quaternion.Euler(0, 0, 0);
            callback?.Invoke(args);
        }
    }
}