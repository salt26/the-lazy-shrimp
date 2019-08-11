using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldStrawberry : MonoBehaviour
{
    public GameObject destroyed;
    public GameObject youWin;
    public GameObject youWinUI;
    private bool isTriggered = false;
    private float waitTime = 1.2f, currentWaitTime = 0f;

    void Update()
    {
        if (isTriggered)
        {
            if (currentWaitTime > waitTime)
            {
                if(youWinUI.activeSelf == false)
                {
                    youWinUI.SetActive(true);
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
                    {
                        GameManager.gm.NextLevel();
                        Destroy(this.gameObject);
                    }
                }
                
            }
            currentWaitTime += Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponentInParent<PlayerController>().health > 0f && !isTriggered)
        {
            isTriggered = true;
            other.gameObject.GetComponentInParent<PlayerController>().AddHealth(3000f);   // 체력 3000 회복
            Instantiate(destroyed, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            youWin.SetActive(true);
            youWin.GetComponent<Animator>().SetTrigger("Win");
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
