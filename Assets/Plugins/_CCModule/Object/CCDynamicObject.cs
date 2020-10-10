using System;

namespace CC
{
    public class CCDynamicObject : CCObject
    {
        private CCMap hostData;
        private CCMap tempdata;

        protected CCDynamicObject(CCMap data)
        {
            this.hostData = data;
            this.tempdata = new CCMap();
        }

        public override void OnFirstUse()
        {
        }

        public override void OnReuse()
        {
        }

        public override void OnRecycle()
        {
        }

        public override void OnRelease()
        {
        }

        public T GetInfo<T>(string key)
        {
            return hostData.Query<T>(key);
        }

        public T SetInfo<T>(string key, T value)
        {
            return hostData.Set<T>(key, value);
        }

        public int GetInfo(string key)
        {
            int result = 0;

            try
            {
                result = (int) hostData.Query<long>(key);
            }
            catch (Exception ex)
            {
                result = hostData.Query<int>(key);
            }

            return result;
        }

        public int AddInfo(string key, int value)
        {
            return hostData.Add(key, value);
        }

        public float AddInfo(string key, float value)
        {
            return hostData.Add(key, value);
        }

        public bool DeleteInfo(string key)
        {
            return hostData.Delete(key);
        }

        public T GetTemp<T>(string key)
        {
            return tempdata.Query<T>(key);
        }

        public int GetTemp(string key)
        {
            int result = 0;

            try
            {
                result = (int) tempdata.Query<long>(key);
            }
            catch (Exception ex)
            {
                result = tempdata.Query<int>(key);
            }

            return result;
        }

        public T SetTemp<T>(string key, T value)
        {
            return tempdata.Set<T>(key, value);
        }

        public int AddTemp(string key, int value)
        {
            return tempdata.Add(key, value);
        }

        public float AddTemp(string key, float value)
        {
            return tempdata.Add(key, value);
        }

        public bool DeleteTemp(string key)
        {
            return tempdata.Delete(key);
        }

        public void Clear()
        {
            hostData.Clear();
            tempdata.Clear();
        }

        public void ClearInfo()
        {
            hostData.Clear();
        }

        public void ClearTemp()
        {
            tempdata.Clear();
        }
    }
}