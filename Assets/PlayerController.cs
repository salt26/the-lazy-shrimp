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
    [SerializeField]
    private float transformTime, transformWork;     //이만큼 가만히 있으면 소로 변함, 이만큼 게이지가 차면 새로 변함

    [SerializeField]
    private float birdWalkingSpeed, birdFlyingSpeed, birdFallingSpeed;
    [SerializeField]
    private float cowWalkingSpeed, cowFallingSpeed, cowDashDistance;

    [HideInInspector]
    public bool isGrounded;

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(0f, 0f);
        if ( state == State.HummingBird )
        {
            movement.x = moveHorizontal * birdWalkingSpeed;
            movement.y = moveVertical * birdFlyingSpeed -  birdFallingSpeed;
        } else
        {
            movement.x = moveHorizontal * cowWalkingSpeed;
            movement.y = isGrounded ? 0f : -cowFallingSpeed;
        }
        GetComponent<Rigidbody2D>().velocity = movement;

        Vector2 ray = new Vector2(0f, -1f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0f, 0.1f, 0f), ray, 0.5f);        //0.02만 올라가도 안 닿음
        if ( hit ) Debug.Log(hit.collider.name);
    }
}
