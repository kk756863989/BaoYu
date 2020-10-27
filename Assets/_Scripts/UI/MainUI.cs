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
    Button TopBtn1, TopBtn2, TopBtn3, TopBtn4, ExitBtn, MenuBtn;
    GameObject ZongLan, JiLu, KaoHe, ExitWindow, VerticalBtns, CaoZuo, ChangeStepMask;
    GameObject ActiveObj;
    public GameObject LY_Image;
    [SerializeField]
    public int VerticalBtn_Index;

    public List<Button> undertoolBtns;
    //public List<GameObject> tools;
    public GameObject MapToolBtns1, MapToolBtns2, Map, Mask;
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
        ChangeStepMask = transform.Find("Main/ChangeStepMask").gameObject;
        TopBtn1 = transform.Find("Main/Top/Buttons/Btn1").GetComponent<Button>();
        TopBtn2 = transform.Find("Main/Top/Buttons/Btn2").GetComponent<Button>();
        TopBtn3 = transform.Find("Main/Top/Buttons/Btn3").GetComponent<Button>();
        TopBtn4 = transform.Find("Main/Top/Buttons/Btn4").GetComponent<Button>();
        ExitBtn = transform.Find("Main/Top/Buttons/ExitBtn").GetComponent<Button>();
        MenuBtn=transform.Find("MenuBtn").GetComponent<Button>();
      
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

        VerticalBtns = transform.Find("Main/ZongLan/VerticalBtns").gameObject;

        foreach (Transform item in VerticalBtns.transform)
        {
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                ChangeActiveObj();
                OpenZongLanUI(item.GetSiblingIndex());
            });
        }

        ZongLan = transform.Find("Main/ZongLan").gameObject;
        CaoZuo = transform.Find("Main/CaoZuo").gameObject;
        JiLu = transform.Find("Main/JiLu").gameObject;
        KaoHe = transform.Find("Main/KaoHe").gameObject;
        ExitWindow = transform.Find("Main/ExitWindow").gameObject;

        TopBtn1.onClick.AddListener(() => {
            topBtn_Click(ZongLan, 1);
        });
        TopBtn2.onClick.AddListener(() => {
            topBtn_Click(CaoZuo, 0);
        });
        TopBtn3.onClick.AddListener(() => {
            topBtn_Click(JiLu, 0);
        });
        TopBtn4.onClick.AddListener(() => {
            topBtn_Click(KaoHe, 0);
        });

        ExitBtn.onClick.AddListener(() =>
        {
            SiblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            ExitWindow.SetActive(true);
        });
        #endregion
    }

    void topBtn_Click(GameObject obj,int x)
    {
        if (StepChange()) return;
        ChangeActiveObj(obj);
        TopBtnMove(x);

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

    public bool StepChange()
    {
        if (MainManager.GetInstance().isInStep)//如果当前在步骤操作中
        {
            ChangeStepMask.SetActive(true);
            return true;
        }
        return false;
    }

    public void StepChangeEnd(int x)
    {
        ChangeStepMask.SetActive(false);
        if (x == 1)
        {
            MenuBtn.gameObject.SetActive(false);
            SetBackImage(true);
            Mask.SetActive(false);
            Map.SetActive(false);
            MainManager.GetInstance().isInStep = false;
            GuiSystem.Close(MainManager.GetInstance().NowStepName);
            MainManager.GetInstance().NowStepName = null;
            if (MainManager.GetInstance().caozuoIndex == 0)
            {
                if (MainManager.GetInstance().Step1ok) return;

                undertoolBtns[0].gameObject.SetActive(false);
                undertoolBtns[1].gameObject.SetActive(false);
                MainManager.GetInstance().Step1Reset();
            }

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