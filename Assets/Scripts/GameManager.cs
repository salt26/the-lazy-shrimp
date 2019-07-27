using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    // Start is called before the first frame update
    void Awake()
    {
        if (gm != null)
        {
            Destroy(this);
            return;
        }
        gm = this;
        DontDestroyOnLoad(this);
    }
    
}
