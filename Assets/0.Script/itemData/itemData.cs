using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum itemTypeDataEnum
{
    Equipment, Expendable, ETC
}
[System.Serializable]
public struct JsonItemData
{
    public string name;
    public int type;
    public int id;
}

public class JsonItemDatas
{
    public List<JsonItemData> data = new List<JsonItemData>();
}

public class itemData : MonoBehaviour
{
    public static itemData Instance;
    [SerializeField] private TextAsset getJsonScript;

    public JsonItemDatas jsonData;

    void Start()
    {
        Instance = this;
        jsonData = JsonUtility.FromJson<JsonItemDatas>(getJsonScript.text);          
    }
}
