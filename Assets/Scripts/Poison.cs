using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField]
    private int widthEven;  // 너비, 반드시 짝수여야 함
    [SerializeField]
    private int height;

    void Awake()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule em = ps.emission;
        em.rateOverTime = new ParticleSystem.MinMaxCurve(widthEven * 10);
        ParticleSystem.ShapeModule sm = ps.shape;
        sm.radius = widthEven / 2;
        ParticleSystem.MainModule mm = ps.main;
        mm.startSpeed = height;
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        c.offset = new Vector2(0f, height / 2f);
        c.size = new Vector2(widthEven, height);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerController>().AddHealth(-2f);   // 초당 체력 100씩 감소
        }
    }
}
