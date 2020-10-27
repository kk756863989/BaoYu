using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CC;

public class MoudleSystem : MonoBehaviour
{
    public Camera camera;
    public Text nameText, infoText;
    public Transform createPoint;
    GameObject Modle;
    Vector3 originalpos = new Vector3();
    Quaternion originalrot = new Quaternion();

    private void Start()
    {
        originalpos = camera.transform.localPosition;
        originalrot = camera.transform.localRotation;
    }

    void Update()
    {
        if (Modle)
        {
            ControlCamera();
        }
    }

    float mouse_x, mouse_y;
    public float distance_z, maxdistance, mindistance, moveSpeed;
    Vector3 tempVec;
    void ControlCamera()
    {
        if (Input.GetMouseButton(0))
        {
            mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴增量
            mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴增量

            camera.transform.RotateAround(createPoint.transform.position, Vector3.up, mouse_x * 5);
        }

        if (Input.GetMouseButton(1))
        {
            mouse_x = -Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
            mouse_y = -Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
            camera.transform.position += new Vector3(mouse_x, mouse_y, 0);

        }

        distance_z = Input.GetAxis("Mouse ScrollWheel");
        if (distance_z < 0)
        {
            camera.GetComponent<Camera>().fieldOfView *= 1.1F;
            if (camera.GetComponent<Camera>().fieldOfView > 50)
                camera.GetComponent<Camera>().fieldOfView = 50;
        }
        if (distance_z > 0)
        {
            camera.GetComponent<Camera>().fieldOfView *= 0.9F;
            if (camera.GetComponent<Camera>().fieldOfView < 10)
                camera.GetComponent<Camera>().fieldOfView = 10;
        }
      
    }

    public void Show3DModle(string name)
    {
        if (Modle != null)
        {
            Destroy(Modle);
            Modle = null;
        }
        ReSetCAMERA();

        Modle = Instantiate(Resources.Load(name)) as GameObject;
        Modle.transform.SetParent(createPoint);
        Modle.transform.localPosition = Modle.transform.position;
        nameText.text = Modle.GetComponent<EqInfo>().nameStr;
        infoText.text = Modle.GetComponent<EqInfo>().infoStr;

        gameObject.SetActive(true);
    }

    public void ReSetCAMERA()
    {
        camera.transform.localPosition = originalpos;
        camera.transform.localRotation = originalrot;
    }

    public void Close()
    {
        Destroy(Modle);
        Modle = null;
        ReSetCAMERA();

        gameObject.SetActive(false);
    }
}
