using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDDOL : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
