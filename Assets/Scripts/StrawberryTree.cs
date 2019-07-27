using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryTree : MonoBehaviour
{
    public GameObject strawberry;
    float genTime = 10f, currentTime = 10f;
    Strawberry m_Strawberry;
    void Update()
    {
        if (m_Strawberry == null && currentTime > genTime)
        {
            m_Strawberry = Instantiate(strawberry, GetComponent<Transform>().position + new Vector3(Random.Range(-0.8f, 0.8f), -0.1f, 0f), Quaternion.identity).GetComponent<Strawberry>();
            currentTime = 0f;
        }
        if (m_Strawberry == null)
            currentTime += Time.deltaTime;
    }
}
