using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class BagManager : MonoBehaviour
{
    public static BagManager instance;
    public List<ItemData> itemlist = new List<ItemData>();
    public Transform content;//背包UI父物体
    public GameObject packageUiItem;//道具格子预制体
    //道具详情面板
    public GameObject itemDetailPanel;
    public Image detaillcon;
    public Text detailType;
    public Text detailDesc;//详情描述
    public ItemDetailConfig detailConfig;//道具详情配置
    //追踪和使用道具
    public GameObject seekButton;
    public GameObject useButton;
    private int id;

    private void Awake()
    {
        instance = this;
        // 注释掉：启动不读存档
        // LoadBagData();
    }
    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="newItem"></param>
    public void AddItem(ItemData newItem)
    {
        var exit = itemlist.Find(item => item.id == newItem.id);//比较道具id，若存在相同道具，道具数量加一
        if (exit != null)
        {
            exit.count++;//道具数量加一
        }
        else
        {
            itemlist.Add(newItem);
        }

        RefreshUi();

        // 注释掉：测试时不保存
        // SaveBagData();
    }
    /// <summary>
    /// 更新背包UI
    /// </summary>
    public void RefreshUi()
    {
        //清空UI
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        //添加道具UI
        List<ItemData> tempList = new List<ItemData>(itemlist);
        foreach (var item in tempList)
        {
            if (item == null) continue;
            if (item.count <= 0) continue; // 数量为0直接不生成

            var go = Instantiate(packageUiItem, content);
            var slot = go.GetComponent<ItemSlots>();
            if (slot != null)
            {
                slot.SetItemData(item);
            }
        }
    }
    public void ShowItemDetail(int id)
    {
        var detail = GetItemDetail(id);
        if (detail == null)
        {
            Debug.LogWarning("找不到详情信息");
            return;
        }
        itemDetailPanel.SetActive(true);
        detaillcon.sprite = detail.fullImage;
        detailType.text = detail.type;
        detailDesc.text = detail.description;
        this.id=detail.itemId;
        if (detail.itemId == 1)
        {
            seekButton.SetActive(true);
            useButton.SetActive(false);
        }
        else if(detail.itemId == 2)
        {
            useButton.SetActive(true);
            seekButton.SetActive(true);
        }
        else
        {
            useButton.SetActive(false);
            seekButton.SetActive(false);
        }
    }
    public ItemDetailData GetItemDetail(int id)
    {
        if(detailConfig == null || detailConfig.itemDetails == null) { return null; }
        foreach(var detail in detailConfig.itemDetails)
        {
            if(detail.itemId == id)
            {
                return detail;
            }
        }
        return null;
    }

    /// <summary>
    /// 保存背包道具信息 —— 已注释，测试用
    /// </summary>
    //public void SaveBagData()
    //{
    //    string json = JsonUtility.ToJson(new BagWrapper { items = itemlist }, true);
    //    string path = Path.Combine(Application.persistentDataPath + "/BagData.json");
    //    File.WriteAllText(path, json);
    //}

    /// <summary>
    /// 读取背包数据 —— 已注释，测试用
    /// </summary>
    //public void LoadBagData()
    //{
    //    string path = Path.Combine(Application.persistentDataPath + "/BagData.json");
    //    if (File.Exists(path))
    //    {
    //        string json = File.ReadAllText(path);
    //        var wrapper = JsonUtility.FromJson<BagWrapper>(json);
    //        itemlist = wrapper.items;
    //        RefreshUi();
    //    }
    //}
    public void SeekButton()
    {
        UIManager.Instance.ReturnToMainPanel();
        string parentName=null;
        if (id == 1)
        {
            parentName = "candles";
        }else if (id == 2)
        {
            parentName = "potions";
        }
        GameObject parent = GameObject.Find(parentName);
        if (parent == null || parent.transform.childCount == 0)
            return;
        Vector3 pos = parent.transform.GetChild(0).position;
        LunaController.instance.SetSeekTarget(pos);
    }
    public void UseButton()
    {
        if (GameManager.Instance.lunaCurrentHP < GameManager.Instance.lunaHP)
        {
            GameManager.Instance.AddOrDeleteHP(40);

            var item = itemlist.Find(i => i.id == id);
            if (item != null)
            {
                item.count--;
                // 如果道具数量 ≤0，从背包里删掉
                if (item.count <= 0)
                {
                    itemlist.Remove(item);
                }
                RefreshUi();
                CloseDetailPanel(); // 使用完关闭详情面板
            }
        }
    }
    public void ClearItem(int id)
    {
        var item = itemlist.Find(i => i.id == id);
        if (item != null)
        {
            item.count=0;
            itemlist.Remove(item);
            RefreshUi();
        }
    }
    public void CloseDetailPanel()
    {
        itemDetailPanel.SetActive(false);
    }
    [System.Serializable]
    public class BagWrapper
    {
        public List<ItemData> items;
    }
}