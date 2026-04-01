using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject battleGO;//战斗场景
    //Luna属性
    public int lunaHP;//最大生命值
    public float lunaCurrentHP;//Luna当前生命值
    public int lunaMP;//最大蓝值
    public float lunaCurrentMP;
    //Monster属性
    public int monsterCurrentHP;//Monster当前生命值
    public int dialogInfoIndex;
    public bool canControlLuna;

    public bool hasPetTheDog;
    public int candleNum;
    public int killNum;
    public GameObject monsterGo;
    public NPCDialogic npc;
    public bool enterBattle;

    public GameObject battleMonsterGo;
    public AudioSource audioSource;//获取音频
    public AudioClip normalClip;
    public AudioClip battleClip;

    private void Awake()
    {
        Instance = this;
        lunaCurrentMP = 100;
        lunaCurrentHP = 100;
        lunaHP = 100;
        lunaMP = 100;
        monsterCurrentHP = 80;
    }
    private void Update()
    {
        if (!enterBattle)
        {
            if (lunaCurrentMP <= 100)
            {
                AddOrDeleteMP(Time.deltaTime);
            }
            if (lunaCurrentHP <= 100)
            {
                AddOrDeleteHP(Time.deltaTime);
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        lunaCurrentHP = Mathf.Clamp(lunaCurrentHP + amount, 0, lunaHP);
        Debug.Log(lunaCurrentHP + "/" + lunaHP);
    }

    public void EntryOrExitBattle(bool enter=true,int addKillNum =0)
    {
        battleGO.SetActive(enter);
        enterBattle = enter;
        if (!enter)//非战斗或战斗结束
        {
            killNum+=addKillNum;
            if (addKillNum > 0)
            {
                DestroyMonster();
            }
            monsterCurrentHP = 80;
            PlayMusic(normalClip);
            if (lunaCurrentHP == 0)
            {
                lunaCurrentHP = 100;
                lunaCurrentMP = 0;
                battleMonsterGo.transform.position += new Vector3(-1, 0, 0);
            }
            if (killNum >= 4)
            {
                SetContentIndix();
            }
        }
        else
        {
            PlayMusic(battleClip);
        }
            enterBattle = enter;
    }
    /// <summary>
    /// Luna血量变化
    /// </summary>
    /// <param name="value"></param>
    public void AddOrDeleteHP(float value)
    {
        lunaCurrentHP += value;
        if( lunaCurrentHP > lunaHP )
        {
            lunaCurrentHP = lunaHP;
        }
        else if( lunaCurrentHP <=0 )
        {
            lunaCurrentHP = 0;
        }
        UIManager.Instance.SetHPValue(lunaCurrentHP/lunaHP);
    }

    public void AddOrDeleteMP(float value)
    {
        lunaCurrentMP += value;
        if (lunaCurrentMP > lunaMP)
        {
            lunaCurrentMP = lunaMP;
        }
        else if (lunaCurrentMP <= 0)
        {
            lunaCurrentMP = 0;
        }
        UIManager.Instance.SetMPValue(lunaCurrentMP / lunaMP);
    }
    /// <summary>
    /// Luna蓝量是否足够使用技能
    /// </summary>
    /// <param name="value">所耗蓝量</param>
    /// <returns></returns>
    public bool CanUsePlayerMP(int value)
    {
        return lunaCurrentMP >= value;
    }

    /// <summary>
    /// Monster血量变化
    /// </summary>
    /// <param name="value"></param>
    public int AddOrDeleteMonsterHP(int value)
    {
        monsterCurrentHP += value;
        return monsterCurrentHP;
    }
    public void ShowMonsters()
    {
        if (!monsterGo.activeSelf)
        {
            monsterGo.SetActive(true);
        }
    }
    public void SetContentIndix()
    {
        npc.SetContentIndex();
    }
    public void SetMonster(GameObject go)
    {
        battleMonsterGo = go;
    }
    public void DestroyMonster()
    {
        Debug.Log(battleMonsterGo.name);
        Destroy(battleMonsterGo);
    }
    public void PlayMusic(AudioClip audioClip)
    {
        if (audioSource.clip != audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
    public void StartTheGame()
    {
        UIManager.Instance.ShowAndStartGame();
    }
    public void ExitTheGame()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
