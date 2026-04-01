using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlots : MonoBehaviour,IPointerClickHandler
{
    public Text nameText;
    public Image icon;
    private int countData;
    private ItemData itemData;

    public void SetItemData(ItemData item)
    {
        if (item == null || item.count <= 0)
        {
            Destroy(gameObject);
            return;
        }

        itemData = item;
        nameText.text = item.itemName + " x" + item.count;

        Sprite sprite = Resources.Load<Sprite>("Items/" + item.iconName);
        if (sprite != null)
            icon.sprite = sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(itemData==null) { return; }
        BagManager.instance.ShowItemDetail(itemData.id);
    }
}
