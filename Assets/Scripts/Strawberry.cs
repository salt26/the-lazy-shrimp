using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    public GameObject destroyed;
    private AudioSource audioSource;
    public AudioClip clip;

    void Start()
    {
        audioSource = GameObject.Find("Shrimp").GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponentInParent<PlayerController>().health > 0f)
        {
            audioSource.PlayOneShot(clip);
            other.gameObject.GetComponentInParent<PlayerController>().AddHealth(60f);   // 체력 60 회복
            Instantiate(destroyed, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            Destroy(this.gameObject);
        }
    }
}
