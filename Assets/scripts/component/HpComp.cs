using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpComp : ThingComp
{
    public int min, now, max;

    public HpComp(Thing thing, int max_) : base(thing) {
        now = max = max_;
        min = 0;
    }

    public float Rate() => (float)now / max;

    // public override void OnAdded() => Debug.Log("OnAdded()");
    // public override void OnStart() => Debug.Log("OnStart()");
    // public override void OnFinish() => Debug.Log("OnFinish()");
    // public override void PreTick() => Debug.Log("PreTick()");
    // public override void Tick() => Debug.Log("Tick()");
    // public override void PostTick() => ;
}
