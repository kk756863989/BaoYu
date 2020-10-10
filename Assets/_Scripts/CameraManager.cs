using Gaia;
using UnityEngine;

public enum Cameras
{
    SceneCamera,
    PlayerCamera,
}
public class CameraManager : MonoBehaviour
{
    public static GameObject SceneCamera, PlayerCamera, DummyCamera;
    public static GameObject Player, Map;

    void Start()
    {
        SceneCamera = transform.Find("SceneCamera").gameObject;
        PlayerCamera = transform.Find("PlayerCamera").gameObject;
        DummyCamera = transform.Find("DummyCamera").gameObject;
        Player = transform.Find("Player").gameObject;
        Map = transform.Find("MapCanvas").gameObject;

    }

    public static void ChangeCamera(Cameras cameras,bool canMove)
    {
        switch (cameras)
        {
            case Cameras.PlayerCamera:
                PlayerCamera.SetActive(true);
                Player.SetActive(true);
                ShowMap(true);

                SceneCamera.SetActive(false);
                SceneCamera.GetComponent<FreeCamera>().enabled = false;
                SceneCamera.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case Cameras.SceneCamera:
                SceneCamera.SetActive(true);
                if (canMove)
                {
                    SceneCamera.GetComponent<FreeCamera>().enabled = true;
                    SceneCamera.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    SceneCamera.GetComponent<FreeCamera>().enabled = false;
                    SceneCamera.transform.GetChild(0).gameObject.SetActive(false);
                }
                ShowMap(false);

                PlayerCamera.SetActive(false);
                Player.SetActive(false);
                break;
        }
    }

    static void ShowMap(bool bo)
    {
        Map.SetActive(bo);
        if (bo)
            DummyCamera.SetActive(true);
        else
            DummyCamera.SetActive(false);
    }
}
