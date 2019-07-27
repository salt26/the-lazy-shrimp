using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    public GameObject destroyed;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerController>().AddHealth(60f);   // 체력 60 회복
            Instantiate(destroyed, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            Destroy(this.gameObject);
        }
    }
}
