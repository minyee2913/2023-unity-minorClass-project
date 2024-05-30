using UnityEngine;

public class IdleBehavior : Behavior
{
    private int waitTick;
    public IdleBehavior(int waitTick) => this.waitTick = waitTick;
    public override void InitSteps()
    {
        steps = new(){
            new WaitStep(waitTick),
        };
    }
}