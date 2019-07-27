using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerController>().AddHealth(60f);   // 체력 60 회복
            Destroy(this.gameObject);
            // TODO 사라지는 애니메이션
        }
    }
}
