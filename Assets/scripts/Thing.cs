using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Thing : MonoBehaviour
{
    private List<ThingComp> comps;

    public void AddComp(ThingComp comp) => comps.Add(comp);
    public void RemoveComp(ThingComp comp) => comps.Remove(comp);
    public bool HasComp(ThingComp comp) => comps.Exists((c)=> c == comp);
}
