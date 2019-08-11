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
            if (collision.collider.gameObject.GetComponentInParent<PlayerController>().state == PlayerController.State.BlackCow)
            {
                collision.collider.gameObject.GetComponent<Transform>().localPosition = new Vector3(0f, -0.25f, 0f);
                collision.collider.offset = new Vector2(-0.015f, 0.225f);
            }
        }
    }
}
