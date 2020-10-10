using CC;
using CoreAudioApi;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : CCGui
{
    private static MainUI instance;
    public static MainUI GetInstance()
    {
        return instance;
    }

    int SiblingIndex;
    bool isUP = false;
    Animation animation;
    Button TopBtn1, TopBtn2, TopBtn3, TopBtn4, ExitBtn;
    GameObject ZongLan, JiLu, KaoHe, ExitWindow, VerticalBtns;
    GameObject ActiveObj;
    [SerializeField]
    public int VerticalBtn_Index;

    public List<Button> undertoolBtns;
    //public List<GameObject> tools;
    public GameObject MapToolBtns1, MapToolBtns2, Map;
    public Button InfoBtn, ylzSetBtn, ylzLineBtn, NoBtn, YLZOKBtn, WJYOKBtn, drawLYBtn, creatWJYbtn, drawWJYbtn;

    public bool canLineYLZ = false, canYLZ = false, Can_drawLY = false, canLineWJY = false;

    public GameObject YLZ_static, WJY_Point, ylzLines, YLZerror;
    public Transform WJY_Points;

    private void Awake()
    {
        instance = this;
       
    }

    public override void OnFirstUse()
    {
        MainManager.GetInstance().YlzList.Add(YLZ_static);
        animation = GetComponent<Animation>();
        FindComponent();
        ZongLan.SetActive(true);
        ActiveObj = ZongLan;

        TopBtn1.Select();
        TopBtn1.GetComponent<Animator>().SetBool("isPressed", true);

        MsgSystem.Add("showTool", showTool);
    }

    void FindComponent()
    {
        TopBtn1 = transform.Find("Top/Buttons/Btn1").GetComponent<Button>();
        TopBtn2 = transform.Find("Top/Buttons/Btn2").GetComponent<Button>();
        TopBtn3 = transform.Find("Top/Buttons/Btn3").GetComponent<Button>();
        TopBtn4 = transform.Find("Top/Buttons/Btn4").GetComponent<Button>();
        ExitBtn = transform.Find("Top/Buttons/ExitBtn").GetComponent<Button>();

        InfoBtn.onClick.AddListener(() =>
        {
            MsgSystem.Dispatch("showInfoWindow");
        });
        ylzSetBtn.onClick.AddListener(() =>
        {
            canYLZ = !canYLZ;
            canLineYLZ = !canYLZ;
            Can_drawLY = !canYLZ;
            StepUI_1.GetInstance().ResetErrorLine();
        });
        NoBtn.onClick.AddListener(() =>
        {
            canLineYLZ = false;
            canYLZ = false;
            Can_drawLY = false;
        });
        YLZOKBtn.onClick.AddListener(() =>
        {
            canLineYLZ = false;
            canYLZ = false;
            Can_drawLY = false;
            if (StepUI_1.GetInstance().DrawTriangleRight())
            {
                StepUI_1.GetInstance().startstep3();
                MapToolBtns1.SetActive(false);
                MapToolBtns2.SetActive(true);
            }
            else
            {
                YLZerror.SetActive(true);
            }
        });

        creatWJYbtn.onClick.AddListener(DrawTriangleExcenterRadius);

        drawWJYbtn.onClick.AddListener(() =>
        {
            ylzLines.SetActive(false);
            canLineWJY = true;
        });
        WJYOKBtn.onClick.AddListener(() =>
        {
            WJY_Points.gameObject.SetActive(false);
            StepUI_1.GetInstance().startstep4();

        });

        undertoolBtns[0].onClick.AddListener(() =>//记录本
        {
            
        });
        undertoolBtns[1].onClick.AddListener(() =>//地图
        {
            Map.SetActive(!Map.activeSelf);
        });
        ylzLineBtn.onClick.AddListener(() =>//笔
        {
            canLineYLZ = !canLineYLZ;
            canYLZ = !canLineYLZ;
            Can_drawLY = !canLineYLZ;
        });

        #region  TopBtn事件

        VerticalBtns = transform.Find("ZongLan/VerticalBtns").gameObject;

        foreach (Transform item in VerticalBtns.transform)
        {
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                ChangeActiveObj();
                OpenZongLanUI(item.GetSiblingIndex());
            });
        }

        ZongLan = transform.Find("ZongLan").gameObject;
        JiLu = transform.Find("JiLu").gameObject;
        KaoHe = transform.Find("KaoHe").gameObject;
        ExitWindow = transform.Find("ExitWindow").gameObject;

        TopBtn1.onClick.AddListener(() => {
            ChangeActiveObj(ZongLan);
            GuiSystem.Close("CaoZuo_UI");
        });
        TopBtn2.onClick.AddListener(() => {
            ChangeActiveObj();
            GuiSystem.Open("CaoZuo_UI");
        });
        TopBtn3.onClick.AddListener(() => {
            ChangeActiveObj(JiLu);
            GuiSystem.Close("CaoZuo_UI");
        });
        TopBtn4.onClick.AddListener(() => {
            ChangeActiveObj(KaoHe);
            GuiSystem.Close("CaoZuo_UI");
        });
        ExitBtn.onClick.AddListener(() =>
        {
            SiblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            ExitWindow.SetActive(true);
        });
        #endregion

    }

    public void DrawLYbtn()
    {
        Can_drawLY = !Can_drawLY;
        canYLZ = !Can_drawLY;
        canLineYLZ = !Can_drawLY;
    }

    void DrawTriangleExcenterRadius()
    {
        List<Triangle> list = StepUI_1.GetInstance().triangleList;
        for (int i = 0; i <list.Count; i++)
        {
            GameObject tempWJY = Instantiate(WJY_Point);
            tempWJY.SetActive(true);
            tempWJY.transform.SetParent(WJY_Points);
            tempWJY.transform.position = StepUI_1.GetInstance().GetTriangleExcenterRadius(list[i].points[0], list[i].points[1], list[i].points[2]);
            tempWJY.transform.position = new Vector3(tempWJY.transform.position.x, tempWJY.transform.position.y, 1);
            tempWJY.transform.localScale = Vector3.one;
        }
    }

    void OpenZongLanUI(int btnIndex)
    {
        VerticalBtn_Index = btnIndex;
        GuiSystem.Open("ZongLanUI");
    }

    private void showTool(object[] arg)
    {
        int index = (int)arg[0];
        switch (index)
        {
            case 0:
                undertoolBtns[0].gameObject.SetActive(true);
                break;
            case 1:
                MapToolBtns1.SetActive(true);
                break;
            case 2:
                undertoolBtns[1].gameObject.SetActive(true);
                break;

        }
    }

    public void SetBackImage(bool enabled,params float[] Alpa)
    {
        transform.GetComponent<Image>().enabled = enabled;
        if (Alpa.Length > 0)
            transform.GetComponent<Image>().color = new Color(1, 1, 1, Alpa[0]);
    }

    void ChangeActiveObj(params GameObject[] obj)
    {
        CameraManager.ChangeCamera(Cameras.SceneCamera, false);

        transform.GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);

        if (ActiveObj)
            ActiveObj.SetActive(false);

        if (obj.Length != 0)
        {
            ActiveObj = obj[0];
            ActiveObj.SetActive(true);
            ResetActiveObj();
        }
        if (ActiveObj.GetComponent<Animation>())
            ActiveObj.GetComponent<Animation>().Play();
        GuiSystem.Close("ZongLanUI");
    }

    void ResetActiveObj()
    {
        switch (ActiveObj.name)
        {
            case "ZongLan":

                break;
            case "CaoZuo":

                break;
            case "JiLu":

                break;
            case "KaoHe":

                break;
            case "ExitWindow":

                break;
        }

    }

    /// <summary>
    /// X: 0:上  1:下 
    /// </summary>
    /// <param name="x"></param>
    public void TopBtnMove(int x)
    {
        switch (x)
        {
            case 0:
                if (isUP) return;
                animation["MainTopBtn"].speed = 1f;
                animation.Play("MainTopBtn");
                isUP = true;
                break;
            case 1:
                if (!isUP) return;
                animation["MainTopBtn"].time = animation["MainTopBtn"].clip.length;
                animation["MainTopBtn"].speed = -1f;
                animation.Play("MainTopBtn");
                isUP = false;
                break;
        }
    }

    public void ExitGame(bool exit)
    {
        if (exit)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        else
        {
            ExitWindow.SetActive(false);
            transform.SetSiblingIndex(SiblingIndex);
        }
    }

}