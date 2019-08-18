using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public GameObject basalt1;  // should not be destroyed
    public GameObject basalt2;  // should be destroyed
    public float showAfterTime;
    private float showTimer = -1f;

    // Update is called once per frame
    void Update()
    {
        if (showTimer < 0f && basalt1 == null && basalt2 != null)
        {
            showTimer = 0f;
        }

        if (showTimer >= 0f && showTimer < showAfterTime && basalt1 == null && basalt2 != null)
        {
            showTimer += Time.deltaTime;   
        }
        else if (showTimer >= showAfterTime)
        {
            GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
