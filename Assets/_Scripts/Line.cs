using System.Collections.Generic;
using UnityEngine;
using CC;
using System;

public class Line : MonoBehaviour
{
    public List<Vector2> points = new List<Vector2>();
    public List<GameObject> YLZS = new List<GameObject>();
    public LineRenderer lineRenderer;

    private void Start()
    {
        MsgSystem.Add("removeYLZ", Remove);
    }

    private void Remove(object[] arg)
    {
        GameObject ylz = arg[0] as GameObject;
        if (YLZS.Contains(ylz))
        {
            Remove();
            DrawLine.ylzlinelist.Remove(this);

            List<Triangle> trianglelist = StepUI_1.GetInstance().triangleList;

            for (int i = trianglelist.Count-1; i >-1; i--)
            {
                if(trianglelist[i].points.Contains(points[0])|| trianglelist[i].points.Contains(points[1]))
                {
                    StepUI_1.GetInstance().triangleList.RemoveAt(i);
                }
            }
            Destroy(gameObject);
        }
    }

    public void Remove()
    {
        MsgSystem.Remove("removeYLZ", Remove);
    }
}