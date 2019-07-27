using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        HummingBird, BlackCow
    }

    public State state;         // 지금이 새냐 소냐
    public float health;        // 현재 체력
    [SerializeField]
    private float transformLazy, transformWork;                 // 이만큼 가만히 있으면 소로 변함, 이만큼 게이지가 차면 새로 변함
    private float currentTransformLazy, currentTransformWork;   // 현재 가만히 있는 게이지, 현재 일 게이지

    [SerializeField]
    private float birdWalkingSpeed, birdFlyingSpeed, birdFallingSpeed;
    [SerializeField]
    private float cowWalkingSpeed, cowFallingSpeed, cowDashDistance;
    [SerializeField]
    private GameObject hummingBird, blackCow;

    private float lastHorizontal = 0f;

    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isDashing;

    Animator animator;

    void Awake()
    {
        // 기본값은 새
        animator = hummingBird.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        bool isWorking; // 일하는 중인지, 즉 키보드 입력을 받고 있거나 대시 중인지

        #region 변신
        if (!(state == State.BlackCow && isDashing) && Mathf.Approximately(moveHorizontal, 0f) && (state == State.BlackCow || Mathf.Approximately(moveVertical, 0f)))
        {
            isWorking = false;
            if (state == State.HummingBird) currentTransformLazy += Time.fixedDeltaTime;
            Debug.Log("curLazy = " + currentTransformLazy);
        }
        else
        {
            isWorking = true;
            if (state == State.HummingBird) currentTransformLazy = 0f;
            if (state == State.BlackCow)
            {
                currentTransformWork += Time.fixedDeltaTime;
                if (isDashing) currentTransformWork += Time.fixedDeltaTime;
                Debug.Log("curWork = " + currentTransformWork);
            }
        }

        if (state == State.HummingBird && currentTransformLazy > transformLazy)
        {
            // 소로 변신
            currentTransformLazy = 0f;
            currentTransformWork = 0f;
            state = State.BlackCow;
            hummingBird.SetActive(false);
            blackCow.SetActive(true);
        }
        if (state == State.BlackCow && currentTransformWork > transformWork)
        {
            // 새로 변신
            currentTransformWork = 0f;
            currentTransformLazy = 0f;
            state = State.HummingBird;
            blackCow.SetActive(false);
            hummingBird.SetActive(true);
        }
        #endregion

        #region 움직임
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
        // TODO 소일 때 대시 기능
        #endregion

        #region 바닥에 닿았는지 감지
        Vector2 ray = new Vector2(0f, -1f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0f, 0.1f, 0f), ray, 0.5f);        //0.02만 올라가도 안 닿음
        if (hit)
        {
            isGrounded = true;
            Debug.Log(hit.collider.name);
        }
        else
        {
            isGrounded = false;
        }
        #endregion

        #region 애니메이션
        if (!Mathf.Approximately(moveHorizontal, 0f))
        {
            if (moveHorizontal > 0f)
            {
                hummingBird.GetComponent<SpriteRenderer>().flipX = false;
                blackCow.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                hummingBird.GetComponent<SpriteRenderer>().flipX = true;
                blackCow.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        if (state == State.HummingBird) {
            animator = hummingBird.GetComponent<Animator>();
            animator.SetBool("IsFly", !isGrounded);
            animator.SetBool("IsWalk", isWorking && (Mathf.Abs(moveHorizontal) - Mathf.Abs(lastHorizontal) >= 0f));
            
        }
        else
        {
            animator = blackCow.GetComponent<Animator>();
            animator.SetBool("IsDashing", isDashing);
            animator.SetBool("IsWalk", isWorking && (Mathf.Abs(moveHorizontal) - Mathf.Abs(lastHorizontal) >= 0f));
        }
        #endregion

        lastHorizontal = moveHorizontal;
    }
}
