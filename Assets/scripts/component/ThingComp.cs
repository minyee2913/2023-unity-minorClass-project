using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThingComp
{
    public virtual void PreTick() {}
    public virtual void Tick() {}
    public virtual void PostTick() {}
    public virtual void OnAdded() {}
    public virtual void OnStart() {}
    public virtual void OnFinish() {}
}
