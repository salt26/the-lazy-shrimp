using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("St");
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().AddHealth(60f);   // 체력 60 회복
            Destroy(this);
            // TODO 사라지는 애니메이션
        }
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("St");
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().AddHealth(60f);   // 체력 60 회복
            Destroy(this);
            // TODO 사라지는 애니메이션
        }
    }
}
