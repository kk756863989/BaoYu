using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public struct Triangle
{
    public List<Vector2> points;
    public List<Line> linelist;
}

public class StepUI_1 : CCGui
{
    private static StepUI_1 instance;
    public static StepUI_1 GetInstance()
    {
        return instance;
    }
    public Button startBtn, step2Btn, step5Btn, InfoBtn;
    public GameObject InfoWindow;
    bool infowindowOpen = true;
    public GameObject QAMoudle;
    public GameObject step2, step3, step4, step5;
    public List<Triangle> triangleList = new List<Triangle>();
    List<LineRenderer> errorLines = new List<LineRenderer>();
    public Material errorLineMat, rightlineMat;

    public override void OnFirstUse()
    {
        instance = this;


        InfoWindow.SetActive(true);
        FindComponent();
    }

    void FindComponent()
    {
        startBtn.onClick.AddListener(startBtn_Click);
        step2Btn.onClick.AddListener(step2Btn_Click);
        step5Btn.onClick.AddListener(step5Btn_Click);
        InfoBtn.onClick.AddListener(showInfoWindow);
    }

    void showInfoWindow()
    {
        if (infowindowOpen)
        {
            infowindowOpen = false;
            InfoWindow.SetActive(false);
        }
        else
        {
            infowindowOpen = true;
            InfoWindow.SetActive(true);
        }
    }

    int tipclicked;
    public void TipClick(int index)
    {
        tipclicked++;
        MsgSystem.Dispatch("showTool", index);
        if (tipclicked == 3)
        {
            step2.SetActive(true);
        }
    }

    public void startstep3()
    {
        step3.SetActive(true);

    }
    public void startstep4()
    {
        step4.SetActive(true);
        GameObject.Find("Scene/WayPoints").transform.Find("WayPoint").gameObject.SetActive(true);

    }

    public void startstep5()
    {
        step5.SetActive(true);
    }

    void startBtn_Click()
    {
        startBtn.gameObject.SetActive(false);
        InfoWindow.SetActive(false);
        infowindowOpen = false;
        QAMoudle.SetActive(true);
    }

    void step2Btn_Click()
    {
       
    }

    void step5Btn_Click()
    {
        GameObject.Find("Scene/YLZs").transform.Find("YuLiangZhan").gameObject.SetActive(true);

        StepEnd();
    }

    public bool DrawTriangleRight()
    {
        Vector2 p1, p2, p3;
        GameObject ylz1, ylz2 = null;
        List<Line> linelist = DrawLine.ylzlinelist;
        for (int i = 0; i < linelist.Count; i++)//第一条线
        {
            p1 = linelist[i].points[0];
            p2 = linelist[i].points[1];
            ylz1 = linelist[i].YLZS[0];
            ylz2 = linelist[i].YLZS[1];
            for (int j = 0; j < linelist.Count; j++)//第二条线
            {
                if (i == j) continue;
                if (linelist[j].points.Contains(p1))
                {
                    if (linelist[j].points.IndexOf(p1) == 0)
                        p3 = linelist[j].points[1];
                    else
                        p3 = linelist[j].points[0];
                 
                    for (int k = 0; k < linelist.Count; k++)//第三条线
                    {
                        if (linelist[k].points.Contains(p2) && linelist[k].points.Contains(p3))
                        {
                            AddTriangle(p1, p2, p3, linelist[i], linelist[j], linelist[k]);
                            if (isAcutetriangle(p1, p2, p3))
                                continue;
                            else
                            {
                                Debug.Log(p1 + "  " + p2 + "  " + p3 + " 不是锐角三角形 ");
                                triangleList[triangleList.Count - 1].linelist[0].lineRenderer.material = errorLineMat;
                                triangleList[triangleList.Count - 1].linelist[1].lineRenderer.material = errorLineMat;
                                triangleList[triangleList.Count - 1].linelist[2].lineRenderer.material = errorLineMat;
                                errorLines.Add(triangleList[triangleList.Count - 1].linelist[0].lineRenderer);
                                errorLines.Add(triangleList[triangleList.Count - 1].linelist[1].lineRenderer);
                                errorLines.Add(triangleList[triangleList.Count - 1].linelist[2].lineRenderer);

                                return false;
                            }
                        }
                    }
                }
                else if (linelist[j].points.Contains(p2))
                {
                    if (linelist[j].points.IndexOf(p2) == 0)
                        p3 = linelist[j].points[1];
                    else
                        p3 = linelist[j].points[0];

                    for (int k = 0; k < linelist.Count; k++)//第三条线
                    {
                        if (linelist[k].points.Contains(p1) && linelist[k].points.Contains(p3))
                        {
                            AddTriangle(p1, p2, p3, linelist[i], linelist[j], linelist[k]);
                            if (isAcutetriangle(p1, p2, p3))
                                continue;
                            else
                            {
                                Debug.Log(p1 + "  " + p2 + "  " + p3 + " 不是锐角三角形 ");
                                triangleList[triangleList.Count - 1].linelist[0].lineRenderer.material = errorLineMat;
                                triangleList[triangleList.Count - 1].linelist[1].lineRenderer.material = errorLineMat;
                                triangleList[triangleList.Count - 1].linelist[2].lineRenderer.material = errorLineMat;
                                errorLines.Add(triangleList[triangleList.Count - 1].linelist[0].lineRenderer);
                                errorLines.Add(triangleList[triangleList.Count - 1].linelist[1].lineRenderer);
                                errorLines.Add(triangleList[triangleList.Count - 1].linelist[2].lineRenderer);

                                return false;
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("画对了,一共有   " + triangleList.Count);
        return true;
    }

    public void ResetErrorLine()
    {
        if (errorLines.Count < 1) return;
        for (int i = 0; i < errorLines.Count; i++)
        {
            errorLines[i].material = rightlineMat;
        }
        errorLines.Clear();
    }

    void AddTriangle(Vector2 p1, Vector2 p2, Vector2 p3,Line l1,Line l2,Line l3)
    {
        Triangle triangle = new Triangle();
        triangle.points = new List<Vector2>();
        triangle.points.Add(p1);
        triangle.points.Add(p2);
        triangle.points.Add(p3);

        triangle.linelist = new List<Line>();
        triangle.linelist.Add(l1);
        triangle.linelist.Add(l2);
        triangle.linelist.Add(l3);


        for (int i = 0; i < triangleList.Count; i++)
        {
            if (triangleList[i].points.Contains(p1) && triangleList[i].points.Contains(p2) && triangleList[i].points.Contains(p3))
                return;
        }
        triangleList.Add(triangle);


    }

    bool isAcutetriangle(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        double max;
        float a, b, c;
        a = Vector2.Distance(p1, p2);
        b = Vector2.Distance(p1, p3);
        c = Vector2.Distance(p2, p3);
        max = Math.Max(a, b);
        max = Math.Max(max, c);
        if (max == a)
        {
            if (Math.Pow(max, 2) < Math.Pow(b, 2) + Math.Pow(c, 2))
                return true;
            else
                return false;
        }
        else if (max == b)
        {
            if (Math.Pow(max, 2) < Math.Pow(a, 2) + Math.Pow(c, 2))
                return true;
            else
                return false;

        }
        else if (max == c)
        {
            if (Math.Pow(max, 2) < Math.Pow(b, 2) + Math.Pow(a, 2))
                return true;
            else
                return false;
        }
        else
            return false;
        
    }

    public Vector2 GetTriangleExcenterRadius(Vector2 A, Vector2 B, Vector2 C)
    {
        Vector2 center;
        if (A == B && A == C)
        {
            center = A;
            return center;
        }
        float x1 = A.x, x2 = B.x, x3 = C.x, y1 = A.y, y2 = B.y, y3 = C.y;
        float C1 = (float)(Math.Pow(x1, 2) + Math.Pow(y1, 2) - Math.Pow(x2, 2) - Math.Pow(y2, 2));
        float C2 = (float)(Math.Pow(x2, 2) + Math.Pow(y2, 2) - Math.Pow(x3, 2) - Math.Pow(y3, 2));
        float centery = (C1 * (x2 - x3) - C2 * (x1 - x2)) / (2 * (y1 - y2) * (x2 - x3) - 2 * (y2 - y3) * (x1 - x2));
        float centerx = (C1 - 2 * centery * (y1 - y2)) / (2 * (x1 - x2));
        center = new Vector2(centerx, centery);
        Debug.Log(A + "   " + B + "  " + C + "  " + center);
        return center;
    }

    private float GetDistance(Vector2 A, Vector2 B)
    {
        return (float)Math.Sqrt(Math.Pow((A.x - B.x), 2) + Math.Pow((A.y - B.y), 2));
    }

    private void StepEnd()
    {
        transform.Find("END").gameObject.SetActive(true);
        MainManager.GetInstance().Step1ok = true;

    }

    public override void Close(params object[] args)
    {
        base.Close(args);
    }
}
