using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public override string Name => "Player";

    public override void OnInstantiate()
    {
        base.OnInstantiate();

        AddComp(new HpComp(this, 100));
        AddComp(new MoveComp(this, 60));
    }

    public override void OnStart()
    {
        base.OnStart();

        MoveComp moveComp = (MoveComp)GetComp(typeof(MoveComp));
        BehaviorComp behaviorComp = (BehaviorComp)GetComp(typeof(BehaviorComp));
        //behaviorComp.SetBehavior(new MoveBehavior(this, Vector3Int.zero, Vector2Int.one));
    }
}