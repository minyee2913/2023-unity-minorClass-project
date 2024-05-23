using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public override string Name => "player";
    public override void OnInstantiate()
    {
        base.OnInstantiate();

        AddComp(new HpComp(100));
        AddComp(new InvComp());
    }
}
