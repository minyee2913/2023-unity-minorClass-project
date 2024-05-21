using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Thing
{
    public Player() {
        AddComp(new InvComp());
    }
}
