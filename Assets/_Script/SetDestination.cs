using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class DestinationPointer
{
    protected SetDestination _dest;
    public DestinationPointer(SetDestination _destination)
    {
        _dest = _destination;
    }
    public abstract Vector3 SetPointer();
    public abstract void UpdatePointer();
}
public class BasicPointer : DestinationPointer
{
    public BasicPointer(SetDestination _destination ) : base(_destination)
    {

    }
    public override Vector3 SetPointer()
    {
        return _dest.DestinationObj.position;
    }
    public override void UpdatePointer()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _dest.Send_MoveOrder();
    }
}
public class ExtensionPointer : DestinationPointer
{
    public float PressSpeed = 3.0f;
    public ExtensionPointer(SetDestination _destination) : base(_destination)
    {

    }
    public override Vector3 SetPointer()
    {
        return destination;
    }
    float dist_extend;
    Vector3 destination;
    public override void UpdatePointer()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            dist_extend += Time.deltaTime * PressSpeed;
            destination = _dest.DestinationObj.TransformPoint(0,0,dist_extend);
        }
        if(Input.GetKeyUp(KeyCode.Space))
            _dest.Send_MoveOrder();

    }
}
public class TwoStepPointer : DestinationPointer
{
    public float FloorHeight;
    Vector3 plane;

    Vector3 return_val;
   
    public TwoStepPointer(SetDestination _destination) : base(_destination)
    {
        plane = new Vector3(0, FloorHeight, 0);
    }
    public override Vector3 SetPointer()
    {
        return return_val;

    }
    public override void UpdatePointer()
    {
      /*  if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 destobj = _dest.DestinationObj.position;
            float dist = Vector3.Distance( destobj,);
            Vector3 plane_point = destobj + _dest.DestinationObj.forward* dist;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            
        }*/

    }
}
public class SetDestination : MonoBehaviour
{
    public Transform DestinationObj;
    
    private DestinationPointer _movecontrol;
    private void Start()
    {
        _movecontrol = new ExtensionPointer(this);
    }

    // Update is called once per frame
    void Update()
    {
        _movecontrol.UpdatePointer();
    }
    public void Send_MoveOrder() 
    {
        foreach (Unit_StateManager _unit in PC.Singleton.Selected_MovableObject)
            _unit.Set_MoveTarget(_movecontrol.SetPointer());
    }
}
