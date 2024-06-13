using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    string SavableName {get;}
    string GetJSON();
    IEnumerable<ISavable> GetChilds();
}
