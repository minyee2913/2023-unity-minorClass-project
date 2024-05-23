using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSystem : MonoBehaviour
{
    public static ThingSystem Instance { get; private set;}
    private List<Thing> newThings = new();
    private List<Thing> delThings = new();
    private List<Thing> things = new();
    public int tickRate = 1;
    private float tickTime = 0;
    public Player player;
    void Awake() {
        Instance = this;
    }

    void Start() {
        InstantiateThing(player.gameObject, Vector3Int.one);
    }

    void Update() {
        tickTime += Time.deltaTime;

        if (tickTime > 1f / tickRate) {
            tickTime -= 1f / tickRate;
            InstateThings();
            PreTick();
            Tick();
            PostTick();
            DestroyThings();
        }
    }

    public void DestroyThings()
    {
        foreach (Thing th in delThings)
            delThings.Remove(th);
        foreach (Thing th in delThings)
            th.OnFinish();
    }
    public void DestroyThing(Thing th)
    {
        delThings.Add(th);
    }

    public void PostTick()
    {
        foreach (Thing th in things) {
            th.PostTick();
        }
    }

    public void Tick()
    {
        foreach (Thing th in things) {
            th.Tick();
        }
    }

    public void PreTick()
    {
        foreach (Thing th in things) {
            th.PreTick();
        }
    }
    public Thing InstantiateThing(GameObject prefab, Vector3Int pos) {
        GameObject obj = Instantiate(prefab);
        Thing thing = obj.GetComponent<Thing>();

        obj.transform.position = new Vector3(pos.x, pos.y, pos.z);

        thing.OnInstantiate();
        newThings.Add(thing);

        return thing;
    }

    public void InstateThings()
    {
        foreach (Thing th in newThings) 
            things.Add(th);
        foreach (Thing th in newThings) 
            th.OnStart();
        newThings.Clear();
    }
}
