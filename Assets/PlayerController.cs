using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        HummingBird, BlackCow
    }

    public State state;         //지금이 새냐 소냐
    public float health;        //현재 체력
    private int work;           //일 게이지

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        
    }
}
