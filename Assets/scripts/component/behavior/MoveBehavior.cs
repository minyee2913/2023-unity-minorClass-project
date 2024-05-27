
using UnityEngine;

public class MoveBehavior : Behavior {
    private MoveComp moveComp;
    private Vector3Int from, to;

    public MoveBehavior(Thing thing, Vector3Int from, Vector3Int to)
    {
        moveComp = (MoveComp)thing.GetComp(typeof(MoveComp));
        this.from = from;
        this.to = to;
    }

    public override void InitSteps()
    {
        //List<Vector2Int> path;
        //bool result = ThingSystem.Instance.PathFind(from, to, out path);
        //if (result)
        //{
        //    foreach (Vector2Int tile in path)
        //        Debug.Log(tile);
        //}
    }
}