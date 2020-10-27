using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager instance;
    public static MainManager GetInstance()
    {
        return instance;
    }
   
    public List<int> QAList = new List<int>();
    public List<GameObject> YlzList = new List<GameObject>();//流域地形图上的雨量站
    public List<GameObject> ylzObjList = new List<GameObject>();//场景中的雨量站
    public List<GameObject> WayPointList = new List<GameObject>();//场景中的雨量站路标
    public int caozuoIndex;

    public bool Step1ok = false;

    public string NowStepName;//当前步骤UI名称
    public bool isInStep = false;//是否在步骤中

    void Start()
    {
        instance = this;


    }

  
    void Update()
    {
        
    }

    public void Step1Reset()
    {
        for (int i = YlzList.Count - 1; i > 0; i--)
        {
            Destroy(YlzList[i]);
            YlzList.RemoveAt(i);
        }

        for (int i = WayPointList.Count-1; i >0; i--)
        {
            Destroy(WayPointList[i]);
            WayPointList.RemoveAt(i);
        }
        for (int i = 0; i < ylzObjList.Count; i++)
        {
            Destroy(ylzObjList[i]);
        }
        ylzObjList.Clear();

        MainUI.GetInstance().LY_Image.GetComponent<LineRenderer>().positionCount = 0;
    }
}
