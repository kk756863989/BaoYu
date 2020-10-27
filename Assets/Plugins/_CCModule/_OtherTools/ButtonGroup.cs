using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonGroup : MonoBehaviour
{
    List<GameObject> btnList = new List<GameObject>();
    public List<GameObject> IgnoreBtn = new List<GameObject>();
    void Start()
    {
        foreach (Transform item in transform)
        {
            if (IgnoreBtn.Contains(item.gameObject)) continue;
            if (!item.GetComponent<Button>()) continue;
            btnList.Add(item.gameObject);


            //EventTrigger.Entry en1 = new EventTrigger.Entry();
            //en1.eventID = EventTriggerType.PointerDown;
            //EventTrigger eventTrigger = item.GetComponent<EventTrigger>();
            //en1.callback = new EventTrigger.TriggerEvent();
            //en1.callback.AddListener((BaseEventData pointData) => { Click(item.gameObject); });


            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                Click(item.gameObject);
            });
        }
    }

    void Click( GameObject obj)
    {
        foreach (var btn in btnList)
        {
            if (obj == btn) continue;
            btn.GetComponent<Animator>().SetTrigger("OtherClick");
            btn.GetComponent<Animator>().SetBool("isPressed", false);
            btn.GetComponent<Button>().interactable = true;

        }
        obj.GetComponent<Animator>().SetBool("isPressed",true);
        obj.GetComponent<Button>().interactable = false;
       
    }
}
