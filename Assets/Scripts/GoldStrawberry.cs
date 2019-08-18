using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                    StartCoroutine(WinUIText());
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
            other.gameObject.GetComponentInParent<PlayerController>().Win();
            other.gameObject.GetComponentInParent<PlayerController>().AddHealth(3000f);   // 체력 3000 회복
            Instantiate(destroyed, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            youWin.SetActive(true);
            youWin.GetComponent<Animator>().SetTrigger("Win");
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    IEnumerator WinUIText()
    {
        RectTransform textImage = youWinUI.GetComponentInChildren<Image>().rectTransform;
        float timer = 0.0f;
        textImage.anchoredPosition = new Vector2(0.0f, 405.0f);
        while(timer < 2.25f)
        {
            timer += Time.deltaTime * 2;
            textImage.anchoredPosition = new Vector2(0.0f, Mathf.Max(405.0f - 80 * timer * timer, 0.0f));
            yield return null;
        }

        timer = -1.0f;
        while(timer < 1.0f)
        {
            timer += Time.deltaTime * 2;
            textImage.anchoredPosition = new Vector2(0.0f, Mathf.Max(80.0f - 80 * timer * timer, 0.0f));
            yield return null;
        }
    }
}
