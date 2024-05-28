using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Thing : MonoBehaviour
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

    // Methods
    public ThingComp AddComp(ThingComp comp)
    {
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
        foreach (ThingComp th in ths)
            th.Update();
    }

    public virtual void PreTick()
    {
        foreach (ThingComp th in ths)
            th.PreTick();
    }

    public virtual void Tick()
    {
        foreach (ThingComp th in ths)
            th.Tick();
    }

    public virtual void PostTick()
    {
        foreach (ThingComp th in ths)
            th.PostTick();
    }

    public virtual void OnStart()
    {
        foreach (ThingComp th in ths)
            th.OnStart();
    }

    public virtual void OnFinish()
    {
        foreach (ThingComp th in ths)
            th.OnFinish();
    }
}