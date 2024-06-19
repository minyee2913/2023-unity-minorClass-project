using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BreakBehavior : Behavior
{
    private MoveComp moveComp;
    private Vector3Int from, to;

    public BreakBehavior(Thing thing, Vector3Int from, Vector3Int to)
    {
        moveComp = (MoveComp)thing.GetComp(typeof(MoveComp));
        this.from = from;
        this.to = to;
    }

    public override void InitSteps()
    {
    }

    public override void OnFinish()
    {
        Thing thing = ThingSystem.Instance.FindThing(to);
        if (thing != null) {
            ThingSystem.Instance.DestroyThing(thing);
            Player.Instance.stone++;

            SoundsLab.Instance.Play("dig", 0.3f);
        }
    }
}