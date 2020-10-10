using System;

namespace CC
{
    public enum ProgressStyle
    {
        NONE, //不显示
        PERCENT, //百分比形式
        FILENUM, //文件数量形式
    }

    public struct CCProgress
    {
        private readonly ProgressStyle type;
        public float cur;
        public float max;

        public CCProgress(float cur = 0, float max = 1f, ProgressStyle type = ProgressStyle.PERCENT)
        {
            this.cur = cur;
            this.max = max;
            this.type = type;
        }

        public float GetPercent()
        {
            if (Math.Abs(max) <= 0) return 1;
            return cur / max;
        }

        public string GetString()
        {
            switch (type)
            {
                case ProgressStyle.PERCENT:
                    if (Math.Abs(max) <= 0) return "100%";
                    return Math.Floor(cur / max * 100) + "%";
                case ProgressStyle.FILENUM:
                    return Math.Floor(cur) + "/" + Math.Floor(max);
                default:
                    return "";
            }
        }
    }
}