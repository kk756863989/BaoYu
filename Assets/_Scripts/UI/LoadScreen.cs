using CC;
using UnityEngine.UI;

public class LoadScreen : CCGui
{
   public Button startbtn;

    public override void OnFirstUse()
    {
        startbtn.onClick.AddListener(StartBtnClick);
    }

    private void StartBtnClick()
    {
        GuiSystem.Open("MainUI");
        Close();
    }
}
