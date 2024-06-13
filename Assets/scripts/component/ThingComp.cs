using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThingComp : ISavable
{
    public Thing Thing { get; private set; }

    public string SavableName => "comp-{0}";

    public ThingComp(Thing thing) => Thing = thing;
    public virtual void PreTick() {}
    public virtual void Tick() {}
    public virtual void PostTick() {}
    public virtual void OnAdded() {}
    public virtual void OnStart() {}
    public virtual void Update() {}
    public virtual void OnFinish() {}

    public string GetJSON()
    {
        return "";
    }

    public IEnumerable<ISavable> GetChilds()
    {
        yield break;
    }
}