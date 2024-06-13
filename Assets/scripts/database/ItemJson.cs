
[System.Serializable]
public class ItemJson {
    public string name;
    public string id;
    public string[] tags;
}

[System.Serializable]
public class ItemWrapper {
    public ItemJson[] datas;
}