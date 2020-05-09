using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class UnitSelector : MonoBehaviour
{
    public static UnitSelector Singleton;
    [HideInInspector]
    public bool Selecting = false;
    

    public Transform SelectorRay;
    public delegate void On_UnitSelected(GameObject Target);
    public event On_UnitSelected On_UnitSelected_Event;
    void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(this);
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.B))
            Selecting = true;
        else
            Selecting = false;
#endif

       if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            Selecting = true;
        else
            Selecting = false;


        if (Selecting)
            OnSelecting();

    }

    void OnSelecting() 
    {
        RaycastHit hit;
        Ray new_ray = new Ray(SelectorRay.position, SelectorRay.forward);
        if (Physics.Raycast(new_ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<Unit>() != null && On_UnitSelected_Event != null)
                On_UnitSelected_Event.Invoke(hit.collider.gameObject);
        }

    }
}
