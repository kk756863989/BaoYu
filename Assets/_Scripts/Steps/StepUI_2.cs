using CC;
using UnityEngine;
using UnityEngine.UI;

public class StepUI_2 : CCGui
{
    private static StepUI_2 instance;
    public static StepUI_2 GetInstance()
    {
        return instance;
    }
    public Button startBtn, step2Btn, InfoBtn;
    public GameObject InfoWindow;
    bool infowindowOpen = true;
    public GameObject QAMoudle;
    public GameObject step2;

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
        InfoBtn.onClick.AddListener(showInfoWindow);
    }

    private void showInfoWindow()
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

    public override void Close(params object[] args)
    {
        base.Close(args);
    }
}
