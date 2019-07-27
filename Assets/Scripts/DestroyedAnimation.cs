using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedAnimation : MonoBehaviour
{
    private float lifetime = 0f;

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > 1f) Destroy(this.gameObject);
    }
}
