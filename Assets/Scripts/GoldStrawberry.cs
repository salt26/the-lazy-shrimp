using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldStrawberry : MonoBehaviour
{
    public GameObject destroyed;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerController>().AddHealth(3000f);   // 체력 3000 회복
            Instantiate(destroyed, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            // GameManager.gm.NextLevel();
            Destroy(this.gameObject);
        }
    }
}
