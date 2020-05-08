using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusPlayerController : MonoBehaviour
{
    public static OculusPlayerController Singleton;
    public List<Unit_StateManager> Selected_MovableObject;
    // Start is called before the first frame update
    void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        UnitSelector.Singleton.On_UnitSelected_Event += Add_Unit;


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Add_Unit(GameObject new_unit) 
    {
        Unit_StateManager _new_unit = new_unit.GetComponent<Unit_StateManager>();
        if (!Selected_MovableObject.Contains(_new_unit))
            Selected_MovableObject.Add(_new_unit);
    }
    private void OnDestroy()
    {
        UnitSelector.Singleton.On_UnitSelected_Event -= Add_Unit;


    }
}