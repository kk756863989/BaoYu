﻿using CC;
using UnityEngine;
using UnityEngine.UI;

public class StepUI_2 : CCGui
{
    private static StepUI_2 instance;
    public static StepUI_2 GetInstance()
    {
        return instance;
    }
    public Button startBtn, step2Btn;
    public Animation InfoWindowAnim;
    bool infowindowOpen = true;
    public GameObject QAMoudle;
    public GameObject step2;

    public override void OnFirstUse()
    {
        instance = this;


        InfoWindowAnim.gameObject.SetActive(true);
        MainUI.GetInstance().InfoBtn.gameObject.SetActive(true);
        MsgSystem.Add("showInfoWindow", showInfoWindow);
        FindComponent();
    }

    void FindComponent()
    {
        startBtn.onClick.AddListener(startBtn_Click);
        step2Btn.onClick.AddListener(step2Btn_Click);
    }

    private void showInfoWindow(object[] arg)
    {
        if (infowindowOpen)
        {
            infowindowOpen = false;
            InfoWindowAnim.gameObject.SetActive(false);
        }
        else
        {
            infowindowOpen = true;
            InfoWindowAnim.gameObject.SetActive(true);
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
        QAMoudle.SetActive(true);
        MsgSystem.Dispatch("showInfoWindow");
    }

    void step2Btn_Click()
    {

    }

    public override void Close(params object[] args)
    {
        MsgSystem.Remove("showInfoWindow", showInfoWindow);
        base.Close(args);
    }
}
