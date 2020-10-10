using System;
using UnityEngine;

namespace CC
{
    public enum CostType
    {
        coin,
        gold
    }

    public enum LifeType
    {
        hp, //生命值
        mp, //魔法值
        ph //体力值
    }

    public class CCUser : CCDynamicObject
    {
        public CCUser(CCMap data) : base(data)
        {
        }

        public override void OnFirstUse()
        {
            //InitHp();
            InitWeekData();
        }

        public void InitHp()
        {
            AlignHealTime();
        }

        public void Heal(LifeType lifeType, int healNum, int maxNum)
        {
            if (healNum == 0 || maxNum <= 0) return;

            var propName = lifeType.EnumToString();
            var remainNum = GetInfo(propName);

            remainNum += healNum;
            remainNum = remainNum < 0 ? 0 : (remainNum > maxNum ? maxNum : remainNum);
            SetInfo(propName, remainNum);

            if (remainNum < maxNum) StartAutoHeal();
            else StopAutoHeal();
        }

        private void AlignHealTime()
        {
            var lastHealTime = GetLastHealTime();
            var curTime = UnixUtility.GetCurrentStamp();
            var healCount = Mathf.FloorToInt((curTime - lastHealTime) / (CCConfig.AutoHealInternalTime * 1000f));
            var lostTime = (curTime - lastHealTime) % (CCConfig.AutoHealInternalTime * 1000);
            var remainTime = CCConfig.AutoHealInternalTime - lostTime;

            if (healCount > 0) Heal(LifeType.hp, healCount * CCConfig.AutoHealNum, CCConfig.MaxHp);

            if (GetInfo("Hp") >= CCConfig.MaxHp) return;

            TimerSystem.Add(TimerType.COMMON, 1, remainTime / 100f, AutoHeal);
        }

        public int GetLastHealTime()
        {
            var result = GetInfo("lastHealTime");

            if (result == 0) result = SetInfo("lastHealTime", UnixUtility.GetCurrentStamp());

            return result;
        }

        public void StartAutoHeal()
        {
            TimerSystem.Add(TimerType.COMMON, 1, CCConfig.AutoHealInternalTime, AutoHeal);
        }

        public void StopAutoHeal()
        {
            TimerSystem.Remove(TimerType.COMMON, AutoHeal);
        }

        public void AutoHeal(params object[] args)
        {
            Heal(LifeType.hp, CCConfig.AutoHealNum, CCConfig.MaxHp);
        }

        public bool Cost(CostType costType, int costNum)
        {
            var propName = costType.EnumToString();
            var remainNum = GetInfo(propName);

            if (remainNum < costNum) return false;

            remainNum -= costNum;
            remainNum = remainNum < 0 ? 0 : remainNum;
            SetInfo(propName, remainNum);

            switch (costType)
            {
                case CostType.coin:
                    MsgSystem.Dispatch(CCConfig.MsgKey.UPDATE_COIN.EnumToString());
                    break;
                case CostType.gold:
                    MsgSystem.Dispatch(CCConfig.MsgKey.UPDATE_GOLD.EnumToString());
                    break;
            }

            return true;
        }

        private void InitWeekData()
        {
            if (NeedRefreshSignInData()) RefreshSignInData();
        }

        private bool NeedRefreshSignInData()
        {
            var lastRefreshTime = GetInfo("refreshDateOfSign");

            if (lastRefreshTime == 0) return true;
            return !(UnixUtility.IsSameWeek(UnixUtility.UnixToDate(lastRefreshTime), DateTime.Now));
        }

        public bool HasSignInDay(int day)
        {
            return GetSignInfo()[day] == 1;
        }

        private void RefreshSignInData()
        {
            SetInfo("signDate0", 0);
            SetInfo("signDate1", 0);
            SetInfo("signDate2", 0);
            SetInfo("signDate3", 0);
            SetInfo("signDate4", 0);
            SetInfo("signDate5", 0);
            SetInfo("signDate6", 0);

            SetInfo("refreshDateOfSign", UnixUtility.GetCurrentStamp());
            DataSystem.SaveUser();
        }

        public bool SignIn(int day)
        {
            if (day < 0 || day >= 7)
            {
                Debug.Log((object)"签到参数有误");
                return false;
            }

            var signInfo = GetSignInfo();

            if (signInfo[day] == 1)
            {
                Debug.Log("重复，已经签到了:" + (day + 1));
                return false;
            }

            SetInfo("signDate" + day, 1);
            DataSystem.SaveUser();
            return true;
        }

        public int[] GetSignInfo()
        {
            var result = new int[7];

            for (var i = 0; i < 7; i++)
            {
                result[i] = GetInfo("signDate" + i);
            }

            return result;
        }
    }
}