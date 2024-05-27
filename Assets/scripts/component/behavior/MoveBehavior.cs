
using System.Collections.Generic;
using UnityEngine;

public class MoveBehavior : Behavior {
    private MoveComp moveComp;
    private Vector3Int from, to;

    public MoveBehavior(Thing thing, Vector3Int from, Vector3Int to)
    {
        moveComp = (MoveComp)thing.GetComp(typeof(MoveComp));
        this.from = from;
        this.to = to;
    }

    public override void InitSteps()
    {
        steps = new List<Step>()
        {
            new MoveStep(moveComp, new Vector3Int(0, 0, 0), new Vector3Int(1, 0, 0)),
            new MoveStep(moveComp, new Vector3Int(1, 0, 0), new Vector3Int(2, 0, 0)),
            new MoveStep(moveComp, new Vector3Int(2, 0, 0), new Vector3Int(3, 0, 0)),
            new MoveStep(moveComp, new Vector3Int(3, 0, 0), new Vector3Int(4, 0, 0)),
            new MoveStep(moveComp, new Vector3Int(4, 0, 0), new Vector3Int(5, 0, 0)),
            new MoveStep(moveComp, new Vector3Int(5, 0, 0), new Vector3Int(6, 0, 0)),
            new MoveStep(moveComp, new Vector3Int(6, 0, 0), new Vector3Int(7, 0, 0)),
        };
    }
}