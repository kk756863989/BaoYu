using System;
using UnityEngine;
using UnityEngine.UI;

namespace CC
{
    class CCProgressBar : MonoBehaviour
    {
        private CCProgress ccProgress = new CCProgress(0, 1);
        public Transform bar;
        public Text label;

        public float progress
        {
            get { return ccProgress.GetPercent(); }
            set
            {
                ccProgress.cur = value < 1.0f ? value * ccProgress.max : ccProgress.max;
                if (bar != null) bar.localScale = new Vector3(value < 1.0f ? value : 1.0f, 1.0f, 1.0f);
                if (label != null) label.text = ccProgress.GetString();
            }
        }

        public void Init(float min = 0, float max = 1.0f, ProgressStyle progressStyle = ProgressStyle.PERCENT)
        {
            ccProgress = new CCProgress(min, max, progressStyle);
            progress = Math.Abs(max) <= 0 ? 1 : min / max;
        }
    }
}