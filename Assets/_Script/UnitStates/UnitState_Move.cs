using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Move : Base_UnitState
{
    public UnitState_Move(Unit_StateManager _input_owner) : base(_input_owner) 
    {

    }
    public override void EndState()
    {

    }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {
        Owner.transform.Translate(0, 0, Owner.UnitProperties.MovementSpeed * Time.deltaTime);
        Vector3 _relative_pos = Owner.transform.InverseTransformPoint( Owner.TargetPosition);
        if (_relative_pos.z < 0)
            Owner.To_State(Owner.Get_IdleState);
    }
}
