using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potion : MonoBehaviour
{
    public GameObject effectGo;
    public AudioClip pickClip;
    private string itemName = "血瓶";
    private string iconName = "RecoverHP";
    private int id = 2;
    private int count = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("血瓶被" + collision + "吃到了!");
        //LunaController lunaController = collision.GetComponent<LunaController>();
        //if(lunaController != null)
        //{
        //    if (lunaController.Health < lunaController.lunaHP)
        //    {
        //        lunaController.ChangeHealth(1);
        //        Instantiate(effectGo, transform.position, Quaternion.identity);//生成物体，位置在potion的位置，朝向为默认
        //        //Destroy(collision.gameObject);
        //        Destroy(gameObject);
        //    }  
        //} 
        if (collision.CompareTag("Luna"))
           {
            if (GameManager.Instance.lunaCurrentHP < GameManager.Instance.lunaHP)
            {
                GameManager.Instance.PlaySound(pickClip);
                GameManager.Instance.AddOrDeleteHP(40);
                Instantiate(effectGo, transform.position, Quaternion.identity);//生成物体，位置在potion的位置，朝向为默认
                Destroy(gameObject);
            }
            else
            {
                ItemData itemData = new ItemData
                {
                    id = id,
                    itemName = itemName,
                    iconName = iconName,
                    count = count
                };
                BagManager.instance.AddItem(itemData);
                Instantiate(effectGo, transform.position, Quaternion.identity);//生成物体，位置在potion的位置，朝向为默认
                Destroy(gameObject);
            }
        }

    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("trigger进来啦！");
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Debug.Log("trigger出去啦！");
    //}
}
