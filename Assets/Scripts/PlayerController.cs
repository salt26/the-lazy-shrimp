using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        HummingBird, BlackCow
    }
    
    public State state;         // 지금이 새냐 소냐
    public float health;        // 현재 체력
    public RectTransform UIHealth, UILazy, UIWork; //UI용
    public Text UIHealthText, UIHealthText2;                    //UI용
    public float UIMaxWidth;                                    //UI용, 체력바의 최대 길이
    [SerializeField]
    private float transformLazy, transformWork;                 // 이만큼 가만히 있으면 소로 변함, 이만큼 게이지가 차면 새로 변함
    private float currentTransformLazy, currentTransformWork;   // 현재 가만히 있는 게이지, 현재 일 게이지

    [SerializeField]
    private float birdWalkingSpeed, birdFlyingSpeed, birdFallingSpeed;
    [SerializeField]
    private float cowWalkingSpeed, cowFallingSpeed, cowDashDistance, cowDashTime;
    [SerializeField]
    private float birdMaxHealth, cowMaxHealth, maxHealth;   // maxHealth: 현재 모드에서의 최대 체력
    [SerializeField]
    private GameObject hummingBird, blackCow, youDied;

    private float lastHorizontal = 0f;
    private float currentDashTime = -1f;    // -1f이면 대시 중이 아님, 0f 이상이면 대시 중, IsDashing으로 확인 가능
    private float currentDashDirection = 0f;
    private int dashInputFrame = 0;
    private bool isDead = false;
    private bool isContactLeft, isContactRight, isContactUp;

    [HideInInspector]
    public bool isGrounded;

    public bool IsDashing
    {
        // 읽기 전용 프로퍼티
        get
        {
            return state == State.BlackCow && currentDashTime >= 0f;
        }
    }

    /// <summary>
    /// 게으름 게이지입니다.
    /// 새일 때 안 움직이면 증가하고, 100이 되면 소로 변신합니다.
    /// 소일 때 움직이면 감소하고, 0이 되면 새로 변신합니다.
    /// </summary>
    public float LazyWork
    {
        get
        {
            if (state == State.HummingBird)
            {
                return 100f * currentTransformLazy / transformLazy;
            }
            else
            {
                return 100f * (transformWork - currentTransformWork) / transformWork;
            }
        }
    }

    Animator animator;

    void Awake()
    {
        // 기본값은 새
        currentTransformWork = 0f;
        currentTransformLazy = 0f;
        state = State.HummingBird;
        blackCow.SetActive(false);
        hummingBird.SetActive(true);
        maxHealth = birdMaxHealth;
        health = Mathf.Clamp(health, 0f, maxHealth);
        animator = hummingBird.GetComponent<Animator>();

        // UI 기본값
        UIHealth.sizeDelta = new Vector2(UIMaxWidth, UIHealth.sizeDelta.y);
        UILazy.sizeDelta = new Vector2(0.0f, UILazy.sizeDelta.y);
        UIWork.sizeDelta = new Vector2(0.0f, UIWork.sizeDelta.y);
        UIHealthText.text = UIHealthText2.text = (int)health + "/" + (int)birdMaxHealth;
    }

    void FixedUpdate()
    {
        if (GameManager.gm == null)
        {
            Debug.LogError("Title 씬부터 실행하세요!");
        }
        if (isDead) return;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        bool isWorking; // 일하는 중인지, 즉 키보드 입력을 받고 있거나 대시 중인지

        #region 변신
        if (!(state == State.BlackCow && IsDashing) && Mathf.Approximately(moveHorizontal, 0f) &&
            (state == State.BlackCow || Mathf.Approximately(moveVertical, 0f)))
        {
            isWorking = false;
            if (state == State.HummingBird) currentTransformLazy += Time.fixedDeltaTime;
        }
        else
        {
            isWorking = true;
            if (state == State.HummingBird) currentTransformLazy = 0f;
            if (state == State.BlackCow)
            {
                if (!IsDashing) currentTransformWork += 5f * Time.fixedDeltaTime;
                else currentTransformWork += 20f / cowDashTime * Time.fixedDeltaTime;                 // 대시 시 게으름 게이지 2배로 감소
            }
        }
        //Debug.Log("LazyWork = " + LazyWork);

        if (state == State.HummingBird && currentTransformLazy > transformLazy)
        {
            // 소로 변신
            currentTransformLazy = 0f;
            currentTransformWork = 0f;
            state = State.BlackCow;
            hummingBird.SetActive(false);
            blackCow.SetActive(true);
            maxHealth = cowMaxHealth;
            health = Mathf.Clamp(health * (cowMaxHealth / birdMaxHealth), 0f, maxHealth);   // 체력 비례
        }
        if (state == State.BlackCow && currentTransformWork > transformWork && !IsDashing)
        {
            // 새로 변신
            currentTransformWork = 0f;
            currentTransformLazy = 0f;
            state = State.HummingBird;
            blackCow.SetActive(false);
            hummingBird.SetActive(true);
            maxHealth = birdMaxHealth;
            health = maxHealth;         // 항상 풀피
        }
        #endregion


        #region 바닥에 닿았는지 감지
        Bounds bounds = (state == State.HummingBird) ? hummingBird.GetComponent<Collider2D>().bounds : blackCow.GetComponent<Collider2D>().bounds;
        float errorDistance = 0.02f;

        RaycastHit2D hitBelowL = Physics2D.Raycast(bounds.center + new Vector3(-bounds.extents.x + errorDistance, 0f, 0f),
            new Vector2(0f, -1f), bounds.extents.y + errorDistance, 1 << 11);
        RaycastHit2D hitBelowC = Physics2D.Raycast(bounds.center,
            new Vector2(0f, -1f), bounds.extents.y + errorDistance, 1 << 11);
        RaycastHit2D hitBelowR = Physics2D.Raycast(bounds.center + new Vector3(bounds.extents.x - errorDistance, 0f, 0f),
            new Vector2(0f, -1f), bounds.extents.y + errorDistance, 1 << 11);

        RaycastHit2D hitAboveL = Physics2D.Raycast(bounds.center + new Vector3(-bounds.extents.x + errorDistance, 0f, 0f),
            new Vector2(0f, 1f), bounds.extents.y + errorDistance, 1 << 11);
        RaycastHit2D hitAboveC = Physics2D.Raycast(bounds.center,
            new Vector2(0f, 1f), bounds.extents.y + errorDistance, 1 << 11);
        RaycastHit2D hitAboveR = Physics2D.Raycast(bounds.center + new Vector3(bounds.extents.x - errorDistance, 0f, 0f),
            new Vector2(0f, 1f), bounds.extents.y + errorDistance, 1 << 11);

        RaycastHit2D hitLeftD = Physics2D.Raycast(bounds.center + new Vector3(0f, -bounds.extents.y + errorDistance, 0f),
            new Vector2(-1f, 0f), bounds.extents.x + errorDistance, 1 << 11);
        RaycastHit2D hitLeftC = Physics2D.Raycast(bounds.center,
            new Vector2(-1f, 0f), bounds.extents.x + errorDistance, 1 << 11);
        RaycastHit2D hitLeftU = Physics2D.Raycast(bounds.center + new Vector3(0f, bounds.extents.y - errorDistance, 0f),
            new Vector2(-1f, 0f), bounds.extents.x + errorDistance, 1 << 11);

        RaycastHit2D hitRightD = Physics2D.Raycast(bounds.center + new Vector3(0f, -bounds.extents.y + errorDistance, 0f),
            new Vector2(1f, 0f), bounds.extents.x + errorDistance, 1 << 11);
        RaycastHit2D hitRightC = Physics2D.Raycast(bounds.center,
            new Vector2(1f, 0f), bounds.extents.x + errorDistance, 1 << 11);
        RaycastHit2D hitRightU = Physics2D.Raycast(bounds.center + new Vector3(0f, bounds.extents.y - errorDistance, 0f),
            new Vector2(1f, 0f), bounds.extents.x + errorDistance, 1 << 11);

        if (hitBelowC || hitBelowL || hitBelowR)
        {
            isGrounded = true;
            //Debug.Log("isGrounded");
        }
        else
            isGrounded = false;

        if (hitAboveC || hitAboveL || hitAboveR)
        {
            isContactUp = true;
            //Debug.Log("isContactUp");
        }
        else
            isContactUp = false;

        if (hitLeftC || hitLeftD || hitLeftU)
        {
            isContactLeft = true;
            //Debug.Log("isContactLeft");
        }
        else
            isContactLeft = false;

        if (hitRightC || hitRightD || hitRightU)
        {
            isContactRight = true;
            //Debug.Log("isContactRight");
        }
        else
            isContactRight = false;

        if (state == State.BlackCow && (hitAboveC && hitBelowC || hitLeftC && hitRightC))
        {
            // 끼어서 사망 (판정은 캐릭터의 중심 기준)
            //Debug.Log("You zipped!");
            if (isGrounded && isContactUp) GetComponent<Transform>().localScale = new Vector3(GetComponent<Transform>().localScale.x, 0.5f, 1f);
            else if (isContactLeft && isContactRight) GetComponent<Transform>().localScale = new Vector3(0.4f, GetComponent<Transform>().localScale.y, 1f);
            health = 0f;
        }
        #endregion


        #region 움직임(새일 경우 체력 감소 포함)
        Vector2 movement = new Vector2(0f, 0f);
        if ( state == State.HummingBird )
        {
            // 새의 움직임
            if (!(Mathf.Approximately(moveHorizontal, 0f) && Mathf.Approximately(moveVertical, 0f)))
                health -= 4f * Time.fixedDeltaTime;                     // 이동 중일 때에만 체력 감소
            movement.x = moveHorizontal * birdWalkingSpeed;
            movement.y = moveVertical * birdFlyingSpeed -  birdFallingSpeed;

            if (movement.x > 0f && isContactRight) movement.x = 0f;
            if (movement.x < 0f && isContactLeft) movement.x = 0f;
            if (movement.y > 0f && isContactUp) movement.y = 0f;
            if (movement.y < 0f && isGrounded) movement.y = 0f;
        }
        else
        {
            // 소의 움직임
            if (IsDashing)
            {
                // 대시 중
                currentDashTime += Time.fixedDeltaTime;
                movement.x = currentDashDirection * (2 * cowDashDistance / cowDashTime) * (1 - currentDashTime / cowDashTime);
                movement.y = 0f;
            }
            else
            {
                // 대시 중이 아님
                movement.x = moveHorizontal * cowWalkingSpeed;
                movement.y = isGrounded ? 0f : -cowFallingSpeed;
            }

            if (Input.GetKeyDown(KeyCode.Z) && !IsDashing && isGrounded)
            {
                dashInputFrame = 5;
            }
            if (dashInputFrame > 0 && Mathf.Abs(moveHorizontal) - Mathf.Abs(lastHorizontal) >= 0f && !Mathf.Approximately(moveHorizontal, 0f))
            {
                // 움직이던 방향으로 대시 발동
                dashInputFrame = 0;
                currentDashTime = 0f;
                currentDashDirection = Mathf.Sign(moveHorizontal);
            }
            if (dashInputFrame > 0)
            {
                dashInputFrame--;
            }

            if (currentDashTime > cowDashTime)
            {
                // 대시 끝내기
                // 벽에 충돌 시 즉시 끝나게 해야 함
                currentDashTime = -1f;
                currentDashDirection = 0f;
            }
        }
        GetComponent<Rigidbody2D>().velocity = movement;
        #endregion


        #region 애니메이션
        if (!Mathf.Approximately(moveHorizontal, 0f) && !IsDashing)
        {
            if (moveHorizontal > 0f)
            {
                hummingBird.GetComponent<SpriteRenderer>().flipX = false;
                blackCow.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                hummingBird.GetComponent<SpriteRenderer>().flipX = true;
                blackCow.GetComponent<SpriteRenderer>().flipX = true;
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
            animator.SetBool("IsDashing", IsDashing);
            animator.SetBool("IsWalk", isWorking && (Mathf.Abs(moveHorizontal) - Mathf.Abs(lastHorizontal) >= 0f));
        }
        #endregion

        lastHorizontal = moveHorizontal;    // 깔끔한 애니메이션 재생과 대시를 위해 필요


        #region 죽음
        if (health <= 0f)
        {
            isDead = true;
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(0.8f, 0.2f, 0.2f);
            }
            if (state == State.HummingBird)
            {
                animator = hummingBird.GetComponent<Animator>();
                animator.SetBool("IsFly", false);
                animator.SetBool("IsWalk", false);
            }
            StartCoroutine("Death");
        }
        #endregion


        #region UI
        if(state == State.HummingBird)
        {
            UIHealth.sizeDelta = new Vector2(UIMaxWidth * health / birdMaxHealth, UIHealth.sizeDelta.y);
            UILazy.sizeDelta = new Vector2(UIMaxWidth * LazyWork / 100.0f, UILazy.sizeDelta.y);
            UIWork.sizeDelta = new Vector2(0.0f, UIWork.sizeDelta.y);
            UIHealthText.text = UIHealthText2.text = (int)health + "/" + (int)birdMaxHealth;
        }
        else if(state == State.BlackCow)
        {
            UIHealth.sizeDelta = new Vector2(UIMaxWidth * health / cowMaxHealth, UIHealth.sizeDelta.y);
            UILazy.sizeDelta = new Vector2(0.0f, UILazy.sizeDelta.y);
            UIWork.sizeDelta = new Vector2(UIMaxWidth * LazyWork / 100.0f, UIWork.sizeDelta.y);
            UIHealthText.text = UIHealthText2.text = (int)health + "/" + (int)cowMaxHealth;
        }
        #endregion
    }

    public void AddHealth(float value)
    {
        health += value;
        health = Mathf.Clamp(health, 0f, maxHealth);
    }
    
    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.7f);
        youDied.SetActive(true);
    }
}
