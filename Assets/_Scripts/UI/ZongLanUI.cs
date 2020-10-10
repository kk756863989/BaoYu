using CC;
using UnityEngine;
using UnityEngine.UI;
using Gaia;
using System.Collections.Generic;

public class ZongLanUI : CCGui
{
    private static ZongLanUI instance;
    public static ZongLanUI GetInstance()
    {
        return instance;
    }

    public List<GameObject> LeftBtns, Contents, RightBtns, Content1Texts;

    private void Awake()
    {
        instance = this;
    }

    public override void OnFirstUse()
    {
        FindComponent();
        SetState();
    }

    void FindComponent()
    {
        foreach (GameObject item in LeftBtns)
        {
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowContent(LeftBtns.IndexOf(item));
                if (item.name == "Btn3")
                {
                    CameraManager.ChangeCamera(Cameras.SceneCamera, true);
                    MainUI.GetInstance().SetBackImage(false);
                    MainUI.GetInstance().TopBtnMove(0);
                }
                else
                {
                    CameraManager.ChangeCamera(Cameras.SceneCamera, false);
                    MainUI.GetInstance().SetBackImage(true, 0.7f);
                    MainUI.GetInstance().TopBtnMove(1);
                }
                ResetContent(item);
            });
        }

        foreach (GameObject item in RightBtns)
        {
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                foreach (var text in Content1Texts)
                {
                    text.SetActive(false);
                }
                Content1Texts[RightBtns.IndexOf(item)].SetActive(true);
            });
        }
    }

    void ShowContent(int index)
    {
        foreach (var item in Contents)
        {
            item.SetActive(false);
        }
        Contents[index].SetActive(true);
    }

    void ResetContent(GameObject obj)
    {
        switch (obj.name)
        {
            case "Content0":

                break;
            case "Content1":
                //RightBtns[0].GetComponent<Button>().onClick.Invoke();
                //RightBtns[0].GetComponent<Button>().Select();
                break;
            case "Content2":

                break;
            case "Content3":

                break;
            default:
                break;
        }
    }

    void SetState()
    {
        LeftBtns[MainUI.GetInstance().VerticalBtn_Index].GetComponent<Button>().onClick.Invoke();
        LeftBtns[MainUI.GetInstance().VerticalBtn_Index].GetComponent<Button>().Select();
    }
   
}
