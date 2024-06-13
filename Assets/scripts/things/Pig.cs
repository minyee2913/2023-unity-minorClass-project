using System.Collections.Generic;
using UnityEngine;

public class Pig : NonPlayer
{
    public override string Name => "pig";

    public override void OnInstantiate()
    {
        base.OnInstantiate();
        AddComp(new MoveComp(this, 30));
    }

    protected override void InitState()
    {
        Vector3Int? dest = ThingSystem.Instance.GetRandomEmptyTile(Pos, 30);
        if (dest == null || Random.Range(0, 1f) <= 0.3f) {
            curState = 0;
            BehaviorComp.SetBehavior(new IdleBehavior(Random.Range(10, 40)));
        } else {
            curState = 1;
            BehaviorComp.SetBehavior(new MoveBehavior(this, Pos, dest.Value));
        }
    }

    protected override void NextState()
    {
        if (curState == 1) {
            curState = 0;
            BehaviorComp.SetBehavior(new IdleBehavior(Random.Range(30, 180)));
        } else {
            InitState();
        }
    }
}