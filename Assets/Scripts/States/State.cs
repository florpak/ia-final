using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void OnEnter(Vector3 target);
    public abstract void OnUpdate();
    public abstract void OnExit();
    public FiniteStateMachine fsm;
    public Enemy enemy;
}
