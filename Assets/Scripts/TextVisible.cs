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
        if ((player.state == PlayerController.State.HummingBird && !forCow) ||
            (player.state == PlayerController.State.BlackCow && forCow))
        {
            GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
