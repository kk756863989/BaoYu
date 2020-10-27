using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointCollider : MonoBehaviour
{
    public Transform rayPoint;
    Vector3 colliderPos;
    Transform ylzs;

    private void Start()
    {
        if (!rayPoint) return;

        ylzs = GameObject.Find("Scene/YLZs").transform;
        Ray ray = new Ray(rayPoint.position, -Vector3.up);
        //Debug.DrawRay(rayPoint.position, -Vector3.up, Color.blue, 1000);
        RaycastHit hitinfo;

        if(Physics.Raycast(ray,out hitinfo,2000,LayerMask.GetMask("dixing")))
        {
            colliderPos = hitinfo.point;
            Debug.Log(colliderPos);
            GameObject obj = Instantiate(Resources.Load("yuliangzhan") as GameObject);
            obj.transform.position = colliderPos;
            obj.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            obj.transform.SetParent(ylzs);
            MainManager.GetInstance().ylzObjList.Add(obj);
        }
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StepUI_1.GetInstance().startstep5();
            transform.parent.gameObject.SetActive(false);
        }
    }
}
