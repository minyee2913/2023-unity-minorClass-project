using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStep : Step
{
    private MoveComp moveComp;
    private Vector3Int from, to;

    public MoveStep(MoveComp moveComp, Vector3Int from, Vector3Int to)
    {
        this.moveComp = moveComp;
        this.from = from;
        this.to = to;
    }

    public override bool IsCanceled()
    {
        Thing t = ThingSystem.Instance.FindThing(to);
        return t != null && t != moveComp.Thing;
    }

    public override bool IsFinished()
    {
        return !moveComp.IsMoving;
    }

    public override void OnStart()
    {
        moveComp.Move(from, to);
    }
}