using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.gameObject.GetComponentInParent<PlayerController>().AddHealth(-3600f);   // 체력 3600 감소
        }
    }
}
