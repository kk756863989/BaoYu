using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CC;

public class CaoZuo_UI : MonoBehaviour
{
    public GameObject MainUI_Top, Mask;
    public List<Button> BtnList;
    public Animation anim;
    public Button MenuBtn;
    public Text MenuBtnText;
    bool isOpen = true;

    private void Start()
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
        if (MainUI.GetInstance().StepChange())//如果当前在步骤操作中
            return;

        MenuBtn.gameObject.SetActive(true);
        MenuBtn_Click();
        CameraManager.ChangeCamera(Cameras.PlayerCamera, true);
        MainUI.GetInstance().GetComponent<Image>().enabled = false;
        MainManager.GetInstance().NowStepName = "StepUI_" + (stepIndex + 1).ToString();
        GuiSystem.Open(MainManager.GetInstance().NowStepName);
        MainManager.GetInstance().caozuoIndex = stepIndex;
        MainManager.GetInstance().isInStep = true;
    }

    void MenuBtn_Click()
    {
        if (isOpen)
        {
            MainUI.GetInstance().transform.SetAsFirstSibling();
            Mask.SetActive(false);
            anim["caozuoAnim_up"].speed = 1f;
            anim.Play("caozuoAnim_up");
            isOpen = false;
            MainUI_Top.SetActive(false);
            MenuBtnText.text = "展开";
        }
        else
        {
            MainUI.GetInstance().transform.SetAsLastSibling();
            Mask.SetActive(true);
            anim["caozuoAnim_up"].time = anim["caozuoAnim_up"].clip.length;
            anim["caozuoAnim_up"].speed = -1f;
            anim.Play("caozuoAnim_up");
            isOpen = true;
            MainUI_Top.SetActive(true);
            MenuBtnText.text = "收起";
        }

    }
}
