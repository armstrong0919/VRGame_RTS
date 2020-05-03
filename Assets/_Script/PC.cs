using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    public static PC Singleton;
    public List<Unit_StateManager> Selected_MovableObject;
    // Start is called before the first frame update
    void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}