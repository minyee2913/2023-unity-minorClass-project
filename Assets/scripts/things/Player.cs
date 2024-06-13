using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player Instance {get; private set;}
    public override string Name => "Player";
    public GameObject highlight;

    public int stone = 0;
    public int pork = 0;

    private BehaviorComp behaviorComp;

    public override void OnInstantiate()
    {
        base.OnInstantiate();

        Instance = this;

        AddComp(new HpComp(this, 100));
        AddComp(new MoveComp(this, 10));

        var inv = new InvComp(this);
        //inv.Inventory.OnInventoryUpdate += inv => Debug.Log("Changed Inventory");
        AddComp(new InvComp(this));
    }

    public override void OnStart()
    {
        base.OnStart();
        behaviorComp = (BehaviorComp)GetComp(typeof(BehaviorComp));

        //behaviorComp.SetBehavior(new MoveBehavior(this, Pos, Pos + new Vector3Int(0, 0, -10)));
    }

    public override void Tick()
    {
        base.Tick();
        if (Input.GetMouseButtonDown(0)) {
            if (behaviorComp.CurBehavior != null) {
                behaviorComp.StopBehavior();
            }
        }
        if (Input.GetMouseButton(0) && behaviorComp.CurBehavior == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3Int target = new Vector3Int((int)hit.point.x, 0, (int)hit.point.z);

                var highligh = Instantiate(highlight, target, Quaternion.identity);

                var th = ThingSystem.Instance.FindThing(target);
                if (th == null) {
                    behaviorComp.SetBehavior(new MoveBehavior(this, Pos, target));
                } else if (th.Name == "Wall") {
                    if (Vector3Int.Distance(Pos, target) < 1.5f) {
                        behaviorComp.SetBehavior(new BreakBehavior(this, Pos, target));
                    } else {
                        behaviorComp.SetBehavior(new MoveBehavior(this, Pos, target));
                    }

                    highligh.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                }

                Destroy(highligh, 0.3f);
            }
        }

        InvComp comp = (InvComp)GetComp(typeof(InvComp));
        Inventory inventory = comp.Inventory;
        ItemData data = Database<ItemData>.ConditionData(data => data.Id == "item_a");
    }
}