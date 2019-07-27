using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && collision.collider.gameObject.GetComponentInParent<PlayerController>().health > 0f)
        {
            collision.collider.gameObject.GetComponentInParent<PlayerController>().AddHealth(-3600f);   // 체력 3600 감소
            collision.collider.gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
    }
}
