using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candle : MonoBehaviour
{
    public GameObject effectGo;
    public AudioClip pickClip;
    private string itemName = "¿Ø÷Ú";
    private string iconName = "Candle";
    private int id = 1;
    private int count = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Luna"))
        {
            ItemData itemData = new ItemData
            {
                id = id,
                itemName = itemName,
                iconName = iconName,
                count = count
            };
            BagManager.instance.AddItem(itemData);

            LunaController lunaController = collision.GetComponent<LunaController>();
            if (lunaController != null)
            {
                GameManager.Instance.candleNum++;
                if (GameManager.Instance.candleNum >= 5)
                {
                    GameManager.Instance.SetContentIndix();
                }
            }
            else
            {
                Debug.LogWarning("LunaController Œ¥’“µΩ£¨«ÎºÏ≤ÈΩ≈±æ «∑Òπ“‘ÿµΩPlayer…œ£°");
            }
            if (effectGo != null)
            {
                Instantiate(effectGo, transform.position, Quaternion.identity);
            }
            if (pickClip != null)
            {
                GameManager.Instance.PlaySound(pickClip);
            }
            Destroy(gameObject);
        }
    }
}