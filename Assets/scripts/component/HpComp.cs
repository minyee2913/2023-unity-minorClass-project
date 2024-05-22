using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpComp : ThingComp
{
    public int min, now, max;

    public HpComp(int max_){
        now = max = max_;
        min = 0;
    }

    public float Rate() => (float)now / max;
}
