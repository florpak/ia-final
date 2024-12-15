using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionStates
{
    public abstract void OnEnter(Vector3 target);
    public abstract void OnUpdate();
    public abstract void OnExit();
    public MinionFSM fsm;
    public MinionNPC minion;
}
