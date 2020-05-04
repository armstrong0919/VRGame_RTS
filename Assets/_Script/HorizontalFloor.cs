using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class HorizontalFloor : MonoBehaviour
{
    public static HorizontalFloor Singleton;

    // Start is called before the first frame update
    void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(Singleton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
