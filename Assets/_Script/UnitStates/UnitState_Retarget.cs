using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState_Retarget : Base_UnitState
{
    float timer;
    float dist;
    public UnitState_Retarget(Unit_StateManager _input_owner) : base(_input_owner)
    {

    }
    public override void EndState()
    {

    }
    Vector3 direction;
    Quaternion target_rot;
    float angle;
    public override void StartState()
    {
        timer = 0.0f;
    }

    public override void UpdateState()
    {
        Owner.transform.Translate(0, 0, Owner.UnitProperties.MovementSpeed * Time.deltaTime);
        timer += Time.deltaTime * Owner.UnitProperties.RotationSpeed;
        direction = Owner.TargetPosition - Owner.transform.position;
        target_rot = Quaternion.LookRotation(direction);
        Owner.transform.rotation = Quaternion.Lerp(Owner.transform.rotation, target_rot, timer);
        //Owner.transform.rotation = Quaternion.RotateTowards(Owner.transform.rotation, target_rot, Owner.UnitProperties.RotationSpeed * direction.magnitude * 2);

        if (target_rot == Owner.transform.rotation)
            Owner.To_State(Owner.Get_MoveState);

        if(Vector3.Distance(Owner.transform.position, Owner.TargetPosition) < Owner.transform.localScale.x / 2)
            Owner.To_State(Owner.Get_IdleState);

    }
}