using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThingSystem : MonoBehaviour
{
    public static ThingSystem Instance { get; private set; }

    private List<(Vector3Int, Thing)> newThings = new();
    private List<Thing> delThings = new();
    private List<Thing> things = new();
    private Dictionary<Vector3Int, Thing> map = new();
    private float tickTime = 0;

    public GameObject player;

    void Start()
    {
        Instance = this;
        InstantiateThing(player, Vector3Int.zero);
    }

    void Update()
    {
        tickTime += Time.deltaTime;
        if (tickTime > 1f / 60)
        {
            tickTime -= 1f / 60;

            InstantiateThings();
            PreTick();
            Tick();
            PostTick();
            DestroyThings();
        }

        UpdateThings();
    }

    // Find
    public List<Thing> FindThingsWithComp(Type clazz)
    {
        List<Thing> result = new();
        foreach (Thing thing in things)
            if (thing.HasComp(clazz))
                result.Add(thing);

        return result;
    }

    public Thing FindThing(Vector3Int pos)
    {
        Thing value;
        if (map.TryGetValue(pos, out value))
            return value;
        return null;
    }

    // Thing
    public void InstantiateThing(GameObject prefab, Vector3Int pos)
    {
        GameObject obj = Instantiate(prefab);
        Thing thing = obj.GetComponent<Thing>();

        obj.transform.position = new Vector3(pos.x, pos.y);
        thing.OnInstantiate();

        newThings.Add((pos, thing));
    }

    private void InstantiateThings()
    {
        foreach ((Vector3Int, Thing) pair in newThings)
        {
            things.Add(pair.Item2);
            map.Add(pair.Item1, pair.Item2);
        }
        foreach ((Vector3Int, Thing) pair in newThings)
            pair.Item2.OnStart();
        newThings.Clear();
    }

    public void DestroyThing(Thing thing)
    {
        delThings.Add(thing);
    }

    private void DestroyThings()
    {
        foreach (Thing thing in delThings)
        {
            things.Remove(thing);
            map.Remove(map.First(pair => pair.Value == thing).Key);
        }
        foreach (Thing thing in delThings)
            thing.OnFinish();
        delThings.Clear();
    }

    private void UpdateThings()
    {
        foreach (Thing thing in things)
            thing.OnUpdate();
    }

    // LifeCycle
    private void PreTick()
    {
        foreach (Thing thing in things)
            thing.PreTick();
    }

    private void Tick()
    {
        foreach (Thing thing in things)
            thing.Tick();
    }

    private void PostTick()
    {
        foreach (Thing thing in things)
            thing.PostTick();
    }

    public void Move(Vector3Int from, Vector3Int to)
    {
        map.TryAdd(to, map[from]);
        map.Remove(from);
    }

    private static readonly Vector3Int[] Directions = new Vector3Int[]
{
        new Vector3Int(0, 1),
        new Vector3Int(1, 0),
        new Vector3Int(0, -1),
        new Vector3Int(-1, 0)
};

    public bool PathFind(Vector3Int from, Vector3Int to, out List<Vector3Int> path)
    {
        path = new();

        // A*
        Dictionary<Vector3Int, float> openList = new(); // pos: G
        Dictionary<Vector3Int, float> closedList = new();
        Dictionary<Vector3Int, Vector3Int> pre = new();

        openList.Add(from, 0);
        while (openList.Count > 0)
        {
            Vector3Int pos = openList.First().Key;
            foreach (Vector3Int key in openList.Keys)
                if (openList[pos] + Vector3Int.Distance(pos, to) > openList[key] + Vector3Int.Distance(key, to))
                    pos = key;
            float G = openList[pos];

            if (pos == to)
            {
                Vector3Int cur = from;
                while (cur != to)
                {
                    path.Add(cur);
                    cur = pre[cur];
                }

                path.Add(to);
                path.Reverse();
                return true;
            }

            closedList.TryAdd(pos, G);
            foreach (Vector3Int direction in Directions)
            {
                Vector3Int neighborPos = new Vector3Int(pos.x + direction.x, pos.y + direction.y);
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos))
                    continue;

                float neighborG = G + 1; // °Å¸® °è»ê ÇÊ¿ä
                if (openList.ContainsKey(neighborPos))
                {
                    if (openList[neighborPos] > neighborG)
                    {
                        openList[neighborPos] = neighborG;
                        pre[neighborPos] = pos;
                    }
                }
                else
                {
                    openList.Add(neighborPos, neighborG);
                    pre.Add(neighborPos, pos);
                }
            }
        }

        return false;
    }
}