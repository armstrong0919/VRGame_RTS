using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    void Set_MoveTarget(Vector3 target_pos);
}

[RequireComponent(typeof(Unit))]
public class Unit_StateManager : MonoBehaviour
{

    public UnitProperties UnitProperties;

    [HideInInspector]
    public Vector3 TargetPosition;



    Base_UnitState currentstatus;
    UnitState_Retarget _retarget;
    public UnitState_Retarget Get_RetargetState { get { return _retarget; } }
    UnitState_Move _move;
    public UnitState_Move Get_MoveState { get { return _move; } }
    UnitState_Idle _idle;
    public UnitState_Idle Get_IdleState { get { return _idle; } }
    private void Start()
    {
        _retarget = new UnitState_Retarget(this);
        _move = new UnitState_Move(this);
        _idle = new UnitState_Idle(this);
        currentstatus = _idle;
    }

    public void Update() 
    {

        currentstatus.UpdateState();
    }
    public void To_State(Base_UnitState nextstate) 
    {
        Debug.Log(currentstatus.GetType() + ":" + nextstate.GetType());
        currentstatus.EndState();
        currentstatus = nextstate;
        currentstatus.StartState();
    }


    public void Set_MoveTarget(Vector3 target_pos)
    {
        TargetPosition = target_pos;
        To_State(Get_RetargetState);

    }
}
