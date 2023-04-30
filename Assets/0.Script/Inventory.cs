using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //slot
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform parent;

    [SerializeField] private SaveItemData itemPrefab;
    //item's parent is bgBox's Transform

    [SerializeField] private Transform parentTemp;
    [SerializeField] private Toggle[] toggles;

    public int sizeSlot { get; set; }
    public List<GameObject> slots = new List<GameObject>();

    public int countItem { get; set; }
    public List<SaveItemData> items = new List<SaveItemData>();
    public List<Sprite> sprites = new List<Sprite>();

    void Start()
    {
        //아이템 칸수 지정
        sizeSlot = 25;
        AddInventory(sizeSlot);
        countItem = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AddInventory(3);
            sizeSlot += 3;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            AddTest();
        }
    }

    public void AddInventory(int count)
    {
        //프리팹 생성
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(slotPrefab, parent);
            slots.Add(obj);
        }
    }

    public void AddTest()
    {
        //아이템 생성 테스트
        if (countItem < sizeSlot)
        {
            //int rand = Random.Range(0, ItemData.Instance.jsonData.data.Count);
            int rand = Random.Range(0, sprites.Count);
            SaveItemData item = Instantiate(itemPrefab, slots[countItem].transform);
            item.Data = itemData.Instance.jsonData.data[rand];
            item.GetComponent<Image>().sprite = sprites[rand];
            items.Add(item);
            countItem++;
        }
    }

    public void SortItem()
    {
        //정렬
        //items.Sort((typeA, typeB) => typeA.Data.type.CompareTo(typeB.Data.type));
        //items.Sort((typeA, typeB) => typeA.Data.id.CompareTo(typeB.Data.id));
        items.Sort(delegate (SaveItemData a, SaveItemData b)
        {
            if (a.Data.type < b.Data.type)
                return -1;
            else if (a.Data.type > b.Data.type)
                return 1;
            else
            {
                if (a.Data.id < b.Data.id)
                    return -1;
                else if (a.Data.id > b.Data.id)
                    return 1;
            }
            return 0;
        }
        );

        //데이터 순서대로 슬롯리스트에서 비교하기 -> 일치하면 임시부모에게 전달
        foreach (var i in items)
        {
            i.transform.SetParent(parentTemp.transform);
        }
        Debug.Log("이동");
        //임시부모에게서 다시 슬롯으로 이동
        for (int i = 0; i < items.Count; i++)
        {
            items[i].transform.SetParent(slots[i].transform);
            items[i].transform.localPosition = Vector3.zero;
        }
    }

    public void Tab(Toggle toggle)
    {
        foreach (var t in toggles)
        {
            if (t == toggle && t.isOn)
            {
                //임시부모로
                foreach (var i in items)
                {
                    i.transform.SetParent(parentTemp.transform);
                }

                int type = toggle.GetComponent<IndexToggle>().index;
                if (type == -1)
                {
                    SetParentSlot_all();
                }
                else
                {
                    SetParentSlot(type);
                }
            }
        }
    }

    void SetParentSlot_all()
    {
        for (int i = parentTemp.childCount - 1; i >= 0; i--)
        {
            SaveItemData item = parentTemp.GetChild(i).GetComponent<SaveItemData>();
            item.transform.SetParent(slots[i].transform);
            item.transform.localPosition = Vector3.zero;
        }
    }

    void SetParentSlot(int type)
    {
        int itemCnt = 0;
        for (int i = parentTemp.childCount - 1; i >= 0; i--)
        {
            SaveItemData iItem = parentTemp.GetChild(i).GetComponent<SaveItemData>();
            if (iItem.Data.type == type)
            {
                iItem.transform.SetParent(slots[itemCnt].transform);
                iItem.transform.localPosition = Vector3.zero;
                itemCnt++;
            }
        }
    }
}
