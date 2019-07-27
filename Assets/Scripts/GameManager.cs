﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public int level;   // 스테이지

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
