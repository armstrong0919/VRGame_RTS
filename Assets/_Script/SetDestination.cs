using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;


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
        return _dest.ControlObj.position;
    }
    public override void UpdatePointer()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
            _dest.Send_MoveOrder();
#endif

        if(OVRInput.GetDown( OVRInput.Button.SecondaryIndexTrigger))
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
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space))
        {
            dist_extend += Time.deltaTime * PressSpeed;
            destination = _dest.ControlObj.TransformPoint(0,0,dist_extend);
            _dest.DebugObj.position = destination;
        }
        if(Input.GetKeyUp(KeyCode.Space))
            _dest.Send_MoveOrder();
#endif

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            dist_extend += Time.deltaTime * PressSpeed;
            destination = _dest.ControlObj.TransformPoint(0, 0, dist_extend);
            _dest.DebugObj.position = destination;
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            _dest.Send_MoveOrder();
            destination = Vector3.zero;
            dist_extend = 0.0f;
        }

    }
}
public class TwoStepPointer : DestinationPointer
{
    bool has_hit;
    Vector3 return_val;
    Vector3 first_hit_point;
   
    public TwoStepPointer(SetDestination _destination) : base(_destination)
    {
        
    }
    public override Vector3 SetPointer()
    {
        return return_val;

    }
    public override void UpdatePointer()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            Ray new_ray = new Ray(_dest.ControlObj.position, _dest.ControlObj.forward);
            if (Physics.Raycast(new_ray,out hit))
            {
                if (hit.collider.gameObject == HorizontalFloor.Singleton.gameObject)
                {
                    first_hit_point = hit.point;
                    _dest.DebugObj.position = first_hit_point;
                    has_hit = true;
                }
            }
        }
        if(Input.GetKey(KeyCode.Space))
        {
            Vector3 hitpoint_noheight = new Vector3(first_hit_point.x, 0, first_hit_point.z);
            Vector3 controller_noheight = new Vector3(_dest.ControlObj.position.x, 0, _dest.ControlObj.position.z);
            float _distance = Vector3.Distance(hitpoint_noheight, controller_noheight);
            float euler_y =  _dest.ControlObj.eulerAngles.x;
            float rad = euler_y * Mathf.Deg2Rad;
            float height = euler_y > 0 ? Mathf.Tan(rad) * _distance * -1 : Mathf.Tan(rad) * _distance;

            Debug.Log(_distance + ":" + euler_y + ":" + height + ":" +  _dest.ControlObj.position.y);
            return_val = new Vector3(first_hit_point.x, _dest.ControlObj.position.y + height, first_hit_point.z);

            _dest.DebugObj.position = return_val;
            //return_val = new Vector3(first_hit_point.x, _dest.DestinationObj.position.y, first_hit_point.z);
        }
        if (Input.GetKeyUp(KeyCode.Space) && has_hit)
        {
            _dest.DebugObj.position = return_val;
            _dest.Send_MoveOrder();
            has_hit = false;
        }
#endif
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            RaycastHit hit;
            Ray new_ray = new Ray(_dest.ControlObj.position, _dest.ControlObj.forward);
            if (Physics.Raycast(new_ray, out hit))
            {
                if (hit.collider.gameObject == HorizontalFloor.Singleton.gameObject)
                {
                    first_hit_point = hit.point;
                    _dest.DebugObj.position = first_hit_point;
                    has_hit = true;
                }
            }
        }
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {

            Vector3 hitpoint_noheight = new Vector3(first_hit_point.x, 0, first_hit_point.z);
            Vector3 controller_noheight = new Vector3(_dest.ControlObj.position.x, 0, _dest.ControlObj.position.z);
            float _distance = Vector3.Distance(hitpoint_noheight, controller_noheight);
            float euler_y = _dest.ControlObj.eulerAngles.x;
            float rad = euler_y * Mathf.Deg2Rad;
            float height = euler_y > 0 ? Mathf.Tan(rad) * _distance * -1 : Mathf.Tan(rad) * _distance;

            Debug.Log(_distance + ":" + euler_y + ":" + height + ":" + _dest.ControlObj.position.y);
            return_val = new Vector3(first_hit_point.x, _dest.ControlObj.position.y + height, first_hit_point.z);

            _dest.DebugObj.position = return_val;
            //return_val = new Vector3(first_hit_point.x, _dest.DestinationObj.position.y, first_hit_point.z);
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            _dest.DebugObj.position = return_val;
            _dest.Send_MoveOrder();
            has_hit = false;
        }
    }
}
public class SetDestination : MonoBehaviour
{
    public Transform ControlObj;
    public Transform DebugObj;
    
    private DestinationPointer _movecontrol;
    private void Start()
    {
        _movecontrol = new TwoStepPointer(this);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(ControlObj.position, ControlObj.forward * 100, Color.green);
        _movecontrol.UpdatePointer();
    }
    public void Send_MoveOrder() 
    {
        foreach (Unit_StateManager _unit in OculusPlayerController.Singleton.Selected_MovableObject)
            _unit.Set_MoveTarget(_movecontrol.SetPointer());
    }
}
