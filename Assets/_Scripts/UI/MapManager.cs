using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
   
    Animation animation;
    Button button, animBtn;
    Camera mapCamera;
    public bool isOpen = false;
   
    void Start()
    {
        animation = GetComponent<Animation>();

        button = transform.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);

        animBtn = GetComponent<Button>();
        animBtn.onClick.AddListener(animBtnClick);

        mapCamera = transform.parent.Find("MapCamera").GetComponent<Camera>();
    }

    private void animBtnClick()
    {
        animation["Map"].speed = 1f;
        animation.Play("Map");
        mapCamera.depth = 2;
        isOpen = true;
    }

    void ButtonClick()
    {
        animation["Map"].time = animation["Map"].clip.length;
        animation["Map"].speed = -1f;
        animation.Play("Map");
        mapCamera.depth = 0;
        isOpen = false;
    }
}