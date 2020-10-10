using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CC;

public class CaoZuo_UI : CCGui
{
    public List<Button> BtnList;
    public Animation anim;
    public Button MenuBtn;
    public Text MenuBtnText;
    bool isOpen = true;
   
    public override void OnFirstUse()
    {
        foreach (var btn in BtnList)
        {
            btn.onClick.AddListener(() =>
            {
                StartSteps(BtnList.IndexOf(btn));
            });
        }
        MenuBtn.onClick.AddListener(MenuBtn_Click);
        MenuBtn.gameObject.SetActive(false);
    }

    void StartSteps(int stepIndex)
    {
        MenuBtn.gameObject.SetActive(true);
        MenuBtn_Click();
        CameraManager.ChangeCamera(Cameras.PlayerCamera, true);
        MainUI.GetInstance().GetComponent<Image>().enabled = false;
        GuiSystem.Open("StepUI_" + (stepIndex + 1).ToString());
        MainManager.GetInstance().caozuoIndex = stepIndex;
    }

    void MenuBtn_Click()
    {
        transform.SetAsLastSibling();
        if (isOpen)
        {
            anim["caozuoAnim_up"].speed = 1f;
            anim.Play("caozuoAnim_up");
            isOpen = false;
            MenuBtnText.text = "展开";
        }
        else
        {
            anim["caozuoAnim_up"].time = anim["caozuoAnim_up"].clip.length;
            anim["caozuoAnim_up"].speed = -1f;
            anim.Play("caozuoAnim_up");
            isOpen = true;
            MenuBtnText.text = "收起";
        }

    }
}
