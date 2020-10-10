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
    public List<GameObject> WayPointList = new List<GameObject>();//场景中的雨量站路标
    public int caozuoIndex;

    void Start()
    {
        instance = this;


    }

  
    void Update()
    {
        
    }
}
