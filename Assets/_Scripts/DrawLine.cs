using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CC;
using System;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    public Transform rayPoint;
    Camera UICamera;
    public LineRenderer LY_Image;
    public GameObject ylzLinePrefab, wjyLinePrefab, drawLyEndBtn, YLZ_Line, WJY_Line;

    public Button drawLy_Y, drawLy_N;
    public static List<Line> ylzlinelist = new List<Line>();
    public static List<Line> wjylinelist = new List<Line>();

    GameObject templineObj;
    Line templine;
   
    void Start()
    {
        MsgSystem.Add("DrawLine", AddLine);
        //MsgSystem.Add("DeleLYimage", DeleLYimage);
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        UICamera.cullingMask = LayerMask.GetMask("UI", "LineRenderer");

        drawLy_Y.onClick.AddListener(() => { drawLYend(true); });
        drawLy_N.onClick.AddListener(() => { drawLYend(false); });

    }

    private void DeleLYimage(object[] arg)
    {
        LY_Image.positionCount = 0;
    }

    private void Update()
    {
        if (MainUI.GetInstance().Can_drawLY)
            DrawLY_Image();
    }

    private Vector3 position;
    private void DrawLY_Image()
    {
        if (Input.GetMouseButton(0))
        {
            LY_Image.positionCount++;
            position = UICamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
            LY_Image.SetPosition(LY_Image.positionCount - 1, position);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (LY_Image.positionCount > 0)
            {
                drawLyEndBtn.SetActive(true);
                MainUI.GetInstance().Can_drawLY = false;
            }
        }
    }

    void drawLYend(bool bo)
    {
        if (bo)
            MainUI.GetInstance().Can_drawLY = false;
        else
        {
            MainUI.GetInstance().Can_drawLY = true;
            LY_Image.positionCount = 0;
        }
        drawLyEndBtn.SetActive(false);
    }

    private void AddLine(object[] arg)
    {
        GameObject obj = arg[0] as GameObject;
        Vector3 point = obj.transform.position;
        point.z = 1;

        if (obj.tag == "YLZ_Point")
        {
            if (templineObj == null)
            {
                templineObj = Instantiate(ylzLinePrefab) as GameObject;
                templineObj.SetActive(true);
                templineObj.transform.SetParent(YLZ_Line.transform);

                templine = templineObj.GetComponent<Line>();
                templine.YLZS.Add(obj);

                templine.lineRenderer = templineObj.GetComponent<LineRenderer>();
                templine.lineRenderer.positionCount = 1;
                templine.lineRenderer.SetPosition(0, point);
                templine.lineRenderer.startWidth = 5f;
                templine.lineRenderer.endWidth = 5f;
                templine.points.Add(point);
            }
            else
            {
                if (templine.YLZS[0] == obj)
                {
                    Destroy(templineObj);
                    templine.Remove();
                    templine = null;
                    templineObj = null;
                    return;
                }
                templine.YLZS.Add(obj);
                templine.lineRenderer.positionCount++;
                templine.lineRenderer.SetPosition(1, point);
                templine.points.Add(point);

                for (int i = 0; i < ylzlinelist.Count; i++)
                {
                    if (ylzlinelist[i].YLZS.Contains(templine.YLZS[0]) && ylzlinelist[i].YLZS.Contains(templine.YLZS[1]))
                    {
                        Destroy(templineObj);
                        templine.Remove();
                        templine = null;
                        templineObj = null;
                        return;
                    }
                }
                ylzlinelist.Add(templine);
                templine = null;
                templineObj = null;
            }
        }
        else if (obj.tag == "WJY_Point")
        {
            if (templineObj == null)
            {
                templineObj = Instantiate(wjyLinePrefab) as GameObject;
                templineObj.SetActive(true);
                templineObj.transform.SetParent(WJY_Line.transform);

                templine = templineObj.GetComponent<Line>();
                templine.YLZS.Add(obj);

                templine.lineRenderer = templineObj.GetComponent<LineRenderer>();
                templine.lineRenderer.positionCount = 1;
                templine.lineRenderer.SetPosition(0, point);
                templine.lineRenderer.startWidth = 5f;
                templine.lineRenderer.endWidth = 5f;
                templine.points.Add(point);
            }
            else
            {
                if (templine.YLZS[0] == obj)
                {
                    Destroy(templineObj);
                    templine.Remove();
                    templine = null;
                    templineObj = null;
                    return;
                }
                templine.YLZS.Add(obj);
                templine.lineRenderer.positionCount++;
                templine.lineRenderer.SetPosition(1, point);
                templine.points.Add(point);

                for (int i = 0; i < wjylinelist.Count; i++)
                {
                    if (wjylinelist[i].YLZS.Contains(templine.YLZS[0]) && wjylinelist[i].YLZS.Contains(templine.YLZS[1]))
                    {
                        Destroy(templineObj);
                        templine.Remove();
                        templine = null;
                        templineObj = null;
                        return;
                    }
                }
                wjylinelist.Add(templine);
                templine = null;
                templineObj = null;
            }
        }
    }
}