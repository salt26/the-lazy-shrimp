using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basalt : MonoBehaviour
{
    public GameObject destroyed;
    private bool isDestroyed = false;
    private AudioSource audioSource;
    public AudioClip clip;

    void Start()
    {
        audioSource = GameObject.Find("Shrimp").GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDestroyed && collision.collider.tag == "Player" && collision.collider.gameObject.GetComponentInParent<PlayerController>().health > 0f)
        {
            if (collision.collider.gameObject.GetComponentInParent<PlayerController>().IsDashing)
            {
                audioSource.PlayOneShot(clip);
                isDestroyed = true;
                Instantiate(destroyed, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
                Destroy(this.gameObject);
            }
        }
    }
}
