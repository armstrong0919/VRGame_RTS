using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_UnitState 
{
    protected Unit_StateManager Owner;
    public Base_UnitState(Unit_StateManager _unit_ower) 
    {
        Owner = _unit_ower;
    }
    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void EndState();

}
