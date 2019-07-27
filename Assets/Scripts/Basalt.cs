using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basalt : MonoBehaviour
{
    public GameObject destroyed;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (collision.collider.gameObject.GetComponentInParent<PlayerController>().IsDashing)
            {
                // TODO 파괴되는 애니메이션
                Instantiate(destroyed, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
                Destroy(this.gameObject);
            }
        }
    }
}
