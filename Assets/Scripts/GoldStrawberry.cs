﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldStrawberry : MonoBehaviour
{
    public GameObject destroyed;
    public GameObject youWin;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponentInParent<PlayerController>().health > 0f)
        {
            other.gameObject.GetComponentInParent<PlayerController>().AddHealth(3000f);   // 체력 3000 회복
            Instantiate(destroyed, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            GameManager.gm.NextLevel();
            youWin.SetActive(true);
            youWin.GetComponent<Animator>().SetTrigger("Win");
            Destroy(this.gameObject);
        }
    }
}
