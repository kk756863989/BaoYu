using UnityEngine;

namespace CC
{
    class CCLaunchScreen : MonoBehaviour
    {
        //Disable the unused variable warning
#pragma warning disable 0649
        public CCProgressBar ccProgressBar;
#pragma warning disable 0649
        public ProgressStyle progressStyle = ProgressStyle.PERCENT;
        public float delayTime;

        private void Start()
        {
            //ccProgressBar.Init(0, AssetLib.GetMaxProgress(), progressStyle);
        }

        void Update()
        {
            //delayTime -= Time.deltaTime;
        }

        private void LateUpdate()
        {
            //if (AssetLib.GetPercent() >= 1 && delayTime <= 0)
            //{
            //    OnComplete();
            //}
            //else
            //{
            //    ccProgressBar.progress = AssetLib.GetPercent();
            //}
        }

        private void OnComplete()
        {
            ccProgressBar.progress = 1.0f;
            Destroy(gameObject);
        }
    }
}