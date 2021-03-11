using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManagers : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
