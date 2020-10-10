using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointCollider : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StepUI_1.GetInstance().startstep5();
            transform.parent.gameObject.SetActive(false);
        }
    }
}
