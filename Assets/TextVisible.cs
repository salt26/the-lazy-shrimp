using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextVisible : MonoBehaviour
{
    [SerializeField]
    private bool forCow;
    PlayerController player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.state == PlayerController.State.HummingBird)
        {
            GetComponent<Text>().enabled = !forCow;
        }
        else
        {
            GetComponent<Text>().enabled = forCow;
        }
    }
}
