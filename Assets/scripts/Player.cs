using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public override string Name => "Player";

    public override void OnInstantiate()
    {
        AddComp(new HpComp(100));
    }

    public override void Tick()
    {
        base.Tick();
        Debug.Log("tick!");
    }
}