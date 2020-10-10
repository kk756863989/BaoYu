using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CC;

public class YLZ_Set : MonoBehaviour, IPointerDownHandler
{
    private Transform OrPoint;
    private Vector3 mapSize;
    private Vector2 miniMapSize;
    Transform waypointP;
    public Transform YLZ_Points;
    public GameObject YLZ_Point;
    public float MapScale;  

    float xx;
    Vector2 tempPos;

    private void Awake()
    {
        mapSize = new Vector3(800, 0.01f, 800);//地图实体大小
        miniMapSize = new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);//小地图大小
        xx = mapSize.x / miniMapSize.x;
        waypointP = GameObject.Find("Scene/WayPoints").transform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!MainUI.GetInstance().canYLZ)
            return;
       // tempPos = new Vector2((eventData.position.x - 284.2f) / MapScale, (eventData.position.y - 44.2f) / MapScale);
        tempPos = new Vector2((eventData.position.x - 228.2f) / MapScale, (eventData.position.y + 0.2f) / MapScale);
        GameObject tempYLZ = Instantiate(YLZ_Point);
        tempYLZ.SetActive(true);
        tempYLZ.transform.SetParent(YLZ_Points);
        tempYLZ.GetComponent<RectTransform>().anchoredPosition = tempPos;
        tempYLZ.transform.localPosition = new Vector3(tempYLZ.transform.localPosition.x, tempYLZ.transform.localPosition.y, 0);
        tempYLZ.transform.localScale = Vector3.one;
        MainManager.GetInstance().YlzList.Add(tempYLZ);
        SetYLZPosition(tempPos);
    }

    void SetYLZPosition(Vector2 vector2)
    {
        Vector3 target = new Vector3(vector2.x * xx, 0, vector2.y * xx);
        GameObject obj = Instantiate(Resources.Load("WayPoint")) as GameObject;
        obj.transform.position = target;
        obj.transform.SetParent(waypointP);
        obj.SetActive(false);
        MainManager.GetInstance().WayPointList.Add(obj);
    }

    public void RemoveYLZ(GameObject ylz)
    {
        MsgSystem.Dispatch("removeYLZ", ylz);
        int index = MainManager.GetInstance().YlzList.IndexOf(ylz);
        Destroy(MainManager.GetInstance().WayPointList[index]);
        MainManager.GetInstance().YlzList.RemoveAt(index);
        MainManager.GetInstance().WayPointList.RemoveAt(index);
        Line line;
        for (int i = 0; i < DrawLine.ylzlinelist.Count; i++)
        {
            line = DrawLine.ylzlinelist[i];
            if (line.YLZS.Contains(ylz))
            {
                DrawLine.ylzlinelist.Remove(line);
            }
        }
        Destroy(ylz);

    }

    public void showBtn(GameObject btn)
    {
        if (MainManager.GetInstance().caozuoIndex == 1)
        {



            return;
        }

        if (MainUI.GetInstance().canYLZ)
        {
            if (btn.transform.parent.name == "YLZ_static") return;
            btn.SetActive(!btn.activeSelf);
        }

        if (MainUI.GetInstance().canLineYLZ)
            MsgSystem.Dispatch("DrawLine", btn.transform.parent.gameObject);
    }

    public void WJYClick(GameObject obj)
    {
        if(MainUI.GetInstance().canLineWJY)
            MsgSystem.Dispatch("DrawLine", obj);

    }
}