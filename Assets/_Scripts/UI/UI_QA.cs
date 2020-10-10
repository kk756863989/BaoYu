using CC;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Q_A
{
    public int Index;
    public string QStr;
    public string[] Answers;
    public int AnswerNum;
}

public class UI_QA : MonoBehaviour {
    public Text Qstr, Astr1, Astr2, Astr3, Astr4;
    public Transform ToggleGroup;
    public Button SubmitBtn;
    bool hasAnswer = false, QAfinish = false;
    public int all_QACount = 5, need_QACount = 3, has_QACount = 0;
    int AnswerIndex;
    Toggle tempToggle;
    public GameObject step1;

    public void Start()
    {
        SubmitBtn.onClick.AddListener(SubmitAnswer);
        ShowQuestion();
    }

    void SubmitAnswer()
    {
        if (QAfinish)
        {
            Skip();
            return;
        }
        int AnswerNum = 99;
        for (int i = 0; i < ToggleGroup.childCount; i++)
        {
            if (ToggleGroup.GetChild(i).GetComponent<Toggle>().isOn == true)
            {
                tempToggle = ToggleGroup.GetChild(i).GetComponent<Toggle>();
                hasAnswer = true;
                AnswerNum = i;
                break;
            }
        }
        if (!hasAnswer)  return;

        has_QACount++;

        if (AnswerNum == AnswerIndex)    Debug.Log("yes");
        else   Debug.Log("no"); 

        if (has_QACount == need_QACount)
        {
            SubmitBtn.transform.GetChild(0).GetComponent<Text>().text = "完成";
            MainManager.GetInstance().QAList.Clear();
            QAfinish = true;
            return;
        }
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (tempToggle != null)
            tempToggle.isOn = false;
        int RangeNum = (int)Random.Range(1, all_QACount + 1);
        if (MainManager.GetInstance().QAList.Contains(RangeNum))
        {
            ShowQuestion();
            return;
        }
        MainManager.GetInstance().QAList.Add(RangeNum);

        hasAnswer = false;
        LoadQAJson(RangeNum);
    }

    public void Skip()
    {
        gameObject.SetActive(false);
        step1.SetActive(true);
    }

    CCMap QAInfo;
    JObject QAJson;
    void LoadQAJson(int Index)
    {
        QAInfo = AssetsLoad.dataSources["QAInfo"];
        QAJson = QAInfo.Query<JObject>("QA" + Index);

        Qstr.text = QAJson.GetValue("qstr").ToObject<string>();
        Astr1.text = QAJson.GetValue("astr1").ToObject<string>();
        Astr2.text = QAJson.GetValue("astr2").ToObject<string>();
        Astr3.text = QAJson.GetValue("astr3").ToObject<string>();
        Astr4.text = QAJson.GetValue("astr4").ToObject<string>();
        AnswerIndex = QAJson.GetValue("answer").ToObject<int>();
    }
}