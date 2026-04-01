using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "ItemDetail", menuName = "Config/ItemDetailConfig")]
public class ItemDetailConfig : ScriptableObject
{
    public List<ItemDetailData> itemDetails;
}

[Serializable]
public class ItemDetailData
{
    public int itemId;           // ±àºÅ
    public string itemName;      // Ãû×Ö
    public Sprite fullImage;     // Í¼Æ¬
    public string description;   // ÏêÏ¸ÃèÊö
    public string type;          // ÖÖÀà
}
