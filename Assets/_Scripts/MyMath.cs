using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMath
{
    /// <summary>
    /// 求流域平均降雨量
    /// </summary>
    /// <param name="rainfallData">降雨量数据</param>
    /// <param name="areaData">泰森多边形面积数据</param>
    /// <param name="F">流域面积</param>
    /// <returns></returns>
    public static float Get_Averagerainfall(float[] rainfallData,float[] areaData,float F)
    {
        float x = 0;
        for (int i = 0; i < rainfallData.Length; i++)
        {
            x += (rainfallData[i] * areaData[i]);
        }
        x = (x / F);
        return x;
    }

    /// <summary>
    /// 求土壤水分补充量
    /// </summary>
    /// <param name="_FC">稳定下渗率</param>
    /// <param name="_KF">土壤却数量对下渗率的影响的灵敏系数</param>
    /// <param name="_WM">流域平均蓄水容量</param>
    /// <param name="_M"></param>
    /// <param name="_PE">扣除雨间蒸发的降雨</param>
    /// <param name="_BF">反映下渗能力的空间分布特征</param>
    /// <param name="_B">蓄水容量分布曲线指数</param>
    /// <param name="_W">流域实际土壤含水量</param>
    /// <param name="_P">雨量</param>
    /// <returns></returns>
    public static float Get_deltaW(float _FC,float _KF,float _WM,float _M,float _PE,float _BF,float _B,float _W,float _P)
    {
        float FC = _FC, KF = _KF, WM = _WM, M = _M, PE = _PE, BF = _BF, B = _B, W = _W, P = _P;
        float FM, FA, a, RS, R;
        float RR = 0, E = 0;

        FM = FC * (1 + (KF * ((WM - M) / WM)));

        if (PE >= (FM * (BF + 1)))
        {
            FA = FM;
        }
        else
        {
            FA = FM - FM * Mathf.Pow(1 - (PE / (FM * (BF + 1))), BF + 1);
        }

        RS = PE - FA;

        a = WM * (B + 1) * (1 - Mathf.Pow(1 - W / WM, 1 / (B + 1)));

        if (FA + a >= WM * (B + 1))
        {
            RR = FA + W - WM;
        }
        else
        {
            RR = FA + W - WM + WM * Mathf.Pow(1 - (FA + a) / (WM * (B + 1)), B + 1);
        }

        R = RS + RR;

        float deltaW = P - E - R;

        return deltaW;
    }

    /// <summary>
    /// 求水流含沙量
    /// </summary>
    /// <param name="_h">水深</param>
    /// <param name="_J">坡度</param>
    /// <param name="_w">泥沙的沉速</param>
    /// <param name="_Ys">泥沙的容量</param>
    /// <param name="_Y">水的容量</param>
    /// <param name="_D">沙的粒径</param>
    /// <param name="_g">重力加速度</param>
    /// <returns></returns>
    public static float Get_Sand(float _h,float _J,float _Ys,float _Y,float _D,float _g)
    {
        float S, h = _h, J = _J, w;
        float Ys = _Ys, Y = _Y, D = _D, g = _g;

        w = 1.044f * Mathf.Sqrt((Ys - Y) / Y * g * D);

        S = 4 * Mathf.Pow(h, 0.5f) * J / (Mathf.Pow(h, 0.5f) * w);
        return S;
    }


    public static float Get_Ws()
    {
        float Ws = 0;






        return Ws;
    }

    /// <summary>
    /// 氮磷随泥沙迁移量
    /// </summary>
    /// <param name="C_Soil">氮磷在土壤中的含量</param>
    /// <param name="Q_sed">泥沙的生成量</param>
    /// <param name="Er">某种污染物富集率</param>
    /// <returns></returns>
    public static float Get_Nut_sed(float C_Soil,float Q_sed,float Er)
    {
        float Nut_sed;

        Nut_sed = 86.4f * C_Soil * Q_sed * Er;

        return Nut_sed;
    }

    /// <summary>
    /// 径流中可溶性氮磷的浓度
    /// </summary>
    /// <param name="C_nut">土壤溶液中氮磷的平均浓度</param>
    /// <param name="Nut_exp">天然氮磷进入径流的流出系数，一般取0.1</param>
    /// <param name="Q">径流量</param>
    /// <returns></returns>
    public static float Get_Nut_Rf(float C_nut,float Nut_exp,float Q)
    {
        float Nut_Rf;

        Nut_Rf = 86.4f * C_nut * Nut_exp * Q;

        return Nut_Rf;
    }



    public static void Test()
    {
        float X;
        X = 1 - (2 + 46) / (4 * (5 + 1));
        Debug.Log(X);
    }
}
