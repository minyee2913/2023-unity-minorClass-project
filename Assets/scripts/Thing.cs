using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Thing : MonoBehaviour, ISavable
{
    protected List<ThingComp> ths = new();

    // Variables
    public abstract string Name { get; }
    private Vector3Int pos;
    public Vector3Int Pos {
        get => pos;
        set
        {
            ThingSystem.Instance.Move(pos, value);
            pos = value;
        }
    }

    public string SavableName => $"{name}-{{0}}";

    public List<ThingComp> comps = new();
    public bool stop = false;

    public string GetJSON()
    {
        return $"{Pos}";
    }

    public IEnumerable<ISavable> GetChilds()
    {
        foreach (ThingComp comp in comps) {
            yield return comp;
        }
    }

    // Methods
    public ThingComp AddComp(ThingComp comp)
    {
        comps.Add(comp);
        ths.Add(comp);
        comp.OnAdded();
        
        return comp;
    }
    public void InitPos(Vector3Int pos)
    {
        this.pos = pos;
        transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
    public bool HasComp(Type type_)
    {
        return ths.Any(th => th.GetType() == type_);
    }

    public ThingComp GetComp(Type type_)
    {
        return ths.Find(th => th.GetType() == type_);
    }

    // LifeCycle
    public virtual void OnInstantiate() { }

    public virtual void OnUpdate()
    {
        if (stop) return;
        foreach (ThingComp th in ths)
            th.Update();
    }

    public virtual void PreTick()
    {
        if (stop) return;
        foreach (ThingComp th in ths)
            th.PreTick();
    }

    public virtual void Tick()
    {
        if (stop) return;
        foreach (ThingComp th in ths)
            th.Tick();
    }

    public virtual void PostTick()
    {
        if (stop) return;
        foreach (ThingComp th in ths)
            th.PostTick();
    }

    public virtual void OnStart()
    {
        if (stop) return;
        foreach (ThingComp th in ths)
            th.OnStart();
    }

    public virtual void OnFinish()
    {
        if (stop) return;
        foreach (ThingComp th in ths)
            th.OnFinish();
    }
}