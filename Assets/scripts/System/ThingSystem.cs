using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThingSystem : MonoBehaviour, ISavable
{
    public string Name => "things";
    public string GetJSON() { return null; }
    public IEnumerable<ISavable> GetChilds() {
        foreach (Thing thing in things)
            yield return thing;
    }
    public static ThingSystem Instance { get; private set; }

    public string SavableName => Name;

    private List<(Vector3Int, Thing)> newThings = new();
    private List<Thing> delThings = new();
    private List<Thing> things = new();
    private Dictionary<Vector3Int, Thing> map = new();
    private float tickTime = 0;

    public GameObject pig;

    public Vector3Int startPos;

    void Awake()
    {
        Instance = this;

        TextAsset items = Resources.Load<TextAsset>("items");
        ItemWrapper itemsWrapper = JsonUtility.FromJson<ItemWrapper>(items.text);

        foreach (ItemJson data in itemsWrapper.datas) {
            Database<ItemData>.Load(
                new ItemData(data.name, data.id, data.tags)
            );
        }

        Debug.Log(Database<ItemData>.ConditionData((i)=>i.Id == "2913:stone").Name);
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

    public Thing FindRangeThing(Vector3Int pos, float distance)
    {
        foreach (Thing th in things) {
            if (Vector3Int.Distance(pos, th.Pos) <= distance) {
                return th;
            }
        }
        
        return null;
    }

    // Thing
    public void InstantiateThing(GameObject prefab, Vector3Int pos)
    {
        GameObject obj = Instantiate(prefab);
        Thing thing = obj.GetComponent<Thing>();

        thing.InitPos(pos);
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

    public void DestroyThingForce(Thing thing)
    {
        var obj = thing.gameObject;
        things.Remove(thing);
        Destroy(thing);
        Destroy(obj);
    }

    private void DestroyThings()
    {
        foreach (Thing thing in delThings)
        {
            things.Remove(thing);
            if (thing.Name != "pig") map.Remove(map.First(pair => pair.Value == thing).Key);
        }
        foreach (Thing thing in delThings)
        {
            thing.OnFinish();
            if (thing != null) Destroy(thing.gameObject);
        }
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
        //InputSystem.Instance.PreTick();
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

    private static readonly Vector3Int[] Directions = new[]
    {
        new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(-1, 0, 0)
    };

    private static readonly (Vector3Int, Vector3Int, Vector3Int)[] Directions2 = new[]
    {
        (new Vector3Int(0, 0, 1), new Vector3Int(1, 0), new Vector3Int(1, 0, 1)),
        (new Vector3Int(0, 0, 1), new Vector3Int(-1, 0), new Vector3Int(-1, 0, 1)),
        (new Vector3Int(0, 0, -1), new Vector3Int(1, 0), new Vector3Int(1, 0, -1)),
        (new Vector3Int(0, 0, -1), new Vector3Int(-1, 0), new Vector3Int(-1, 0, -1)),
    };

    public bool PathFind(Vector3Int from, Vector3Int to, out List<Vector3Int> path, float maxDist = 20f)
    {
        path = new();

        Dictionary<Vector3Int, float> openList = new();
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

            if (G + Vector3Int.Distance(pos, to) > maxDist)
                break;

            if (pos == to)
            {
                Vector3Int cur = to;
                while (cur != from)
                {
                    path.Add(cur);
                    cur = pre[cur];
                }
                path.Add(from);
                path.Reverse();

                return true;
            }

            closedList.TryAdd(pos, G);
            openList.Remove(pos);

            // ���� �� ���� �̵�
            foreach (Vector3Int direction in Directions)
            {
                Vector3Int neighborPos = new Vector3Int(pos.x + direction.x, pos.y + direction.y, pos.z + direction.z);
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos))
                    continue;

                float neighborG = G + 1;
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
            // �밢�� �̵�
            foreach (var direction in Directions2)
            {
                Vector3Int neighborPosA = pos + direction.Item1;
                Vector3Int neighborPosB = pos + direction.Item2;
                Vector3Int neighborPos = pos + direction.Item3;
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos) || map.ContainsKey(neighborPosA) || map.ContainsKey(neighborPosB))
                    continue;

                float neighborG = G + 1;
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

    public bool PathFindNeighbor(Vector3Int from, Vector3Int to, out List<Vector3Int> path, float maxDist = 20f)
    {
        path = new();

        Dictionary<Vector3Int, float> openList = new();
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

            if (G + Vector3Int.Distance(pos, to) > maxDist)
                break;

            foreach (Vector3Int direction in Directions)
            {
                if (pos + direction == to)
                {
                    Vector3Int cur = pos;
                    while (cur != from)
                    {
                        path.Add(cur);
                        cur = pre[cur];
                    }
                    path.Add(from);
                    path.Reverse();

                    return true;
                }
            }

            closedList.TryAdd(pos, G);
            openList.Remove(pos);

            // ���� �� ���� �̵�
            foreach (Vector3Int direction in Directions)
            {
                Vector3Int neighborPos = new Vector3Int(pos.x + direction.x, pos.y + direction.y, pos.z + direction.z);
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos))
                    continue;

                float neighborG = G + 1;
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
            // �밢�� �̵�
            foreach (var direction in Directions2)
            {
                Vector3Int neighborPosA = pos + direction.Item1;
                Vector3Int neighborPosB = pos + direction.Item2;
                Vector3Int neighborPos = pos + direction.Item3;
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos) || map.ContainsKey(neighborPosA) || map.ContainsKey(neighborPosB))
                    continue;

                float neighborG = G + 1;
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

    public Vector3Int? GetRandomEmptyTile(Vector3Int center, int size)
    {
        HashSet<Vector3Int> tiles = new();
        Queue<Vector3Int> q = new();
        
        foreach (var dir in Directions)
        {
            if (!map.ContainsKey(center + dir))
            {
                q.Enqueue(center + dir);
                tiles.Add(center + dir);
            }
        }

        while (tiles.Count < size && q.Count > 0)
        {
            Vector3Int tile = q.Dequeue();
            foreach (var dir in Directions)
            {
                if (!tiles.Contains(tile + dir) && !map.ContainsKey(tile + dir))
                {
                    q.Enqueue(tile + dir);
                    tiles.Add(tile + dir);
                }
            }
        }

        if (tiles.Count == 0)
            return null;
        return tiles.ToList()[UnityEngine.Random.Range(0, tiles.Count)];
    }
}