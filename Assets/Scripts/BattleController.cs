using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleController : MonoBehaviour
{
    public Animator lunaAnimator;
    public Animator monsterAnimator;
    public Transform lunaTras;
    public Transform monsterTras;
    private Vector3 monsterInitPos;
    private Vector3 lunaInitPos;
    public SpriteRenderer monsterSr;
    public SpriteRenderer lunaSr;
    public GameObject skillEffectGo;
    public GameObject recoverEffectGo;
    //音效
    public AudioClip attackSound;
    public AudioClip lunaAttackSound;
    public AudioClip monsterAttackSound;
    public AudioClip skillSound;
    public AudioClip recoverSound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public AudioClip monsterDieSound;

    private void Awake()
    {
        monsterInitPos = monsterTras.localPosition;
        lunaInitPos = lunaTras.localPosition;
    }

    private void OnEnable()
    {
        monsterSr.DOFade(1, 0.01f);
        lunaSr.DOFade(1, 0.01f);
        lunaTras.localPosition = lunaInitPos;
        monsterTras.localPosition= monsterInitPos;
    }
    public void LunaAttack()
    {
        StartCoroutine(PerformAttackLogic());

    }
    IEnumerator PerformAttackLogic()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaAnimator.SetBool("MoveState",true);
        lunaAnimator.SetFloat("MoveValue", -1f);
        lunaTras.DOLocalMove(monsterInitPos+new Vector3(1.5f, 0, 0), 0.5f).OnComplete
            (
                () => 
                {
                    GameManager.Instance.PlaySound(attackSound);
                    GameManager.Instance.PlaySound(lunaAttackSound);
                    lunaAnimator.SetBool("MoveState", false);
                    lunaAnimator.SetFloat("MoveValue", 0);
                    lunaAnimator.CrossFade("Attack",0);
                    monsterSr.DOFade(0.3f, 0.2f).OnComplete(()=> { JudgeMonsterHP(-20); });
                }
            );
        yield return new WaitForSeconds(1.2f);
        lunaAnimator.SetBool("MoveState", true);
        lunaAnimator.SetFloat("MoveValue", 1f);
        lunaTras.DOLocalMove(lunaInitPos, 0.5f).OnComplete
            (
                () =>
                {
                    lunaAnimator.SetBool("MoveState", false);
                }
            );
        yield return new WaitForSeconds(0.5f); 
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        monsterTras.DOLocalMove(lunaInitPos - new Vector3(1.5f, 0, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        monsterTras.DOLocalMove(lunaInitPos, 0.2f).OnComplete
            (
                () =>
                {
                    GameManager.Instance.PlaySound(monsterAttackSound);
                    monsterTras.DOLocalMove(lunaInitPos - new Vector3(1.5f, 0, 0), 0.2f);
                    lunaAnimator.CrossFade("Hit", 0);
                    GameManager.Instance.PlaySound(hitSound);
                    lunaSr.DOFade(0.3f, 0.2f).OnComplete(() => { lunaSr.DOFade(1, 0.2f); });
                    JudgeLunaHP(-20);
                }
            );
        yield return new WaitForSeconds(0.4f);
        monsterTras.DOLocalMove(monsterInitPos, 0.5f).OnComplete
            (
                () =>
                {
                    UIManager.Instance.ShowOrHideBattlePanel(true);
                }
            );
    }

    public void  LunaDefind()
    {
        StartCoroutine(PerformDefendLogic());
    }

    IEnumerator PerformDefendLogic()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaAnimator.SetBool("Defend", true);
        monsterTras.DOLocalMove(lunaInitPos - new Vector3(1.5f, 0, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        monsterTras.DOLocalMove(lunaInitPos, 0.2f).OnComplete
            (
                () =>
                {
                    monsterTras.DOLocalMove(lunaInitPos - new Vector3(1.5f, 0, 0), 0.2f);
                    lunaTras.DOLocalMove(lunaInitPos + new Vector3(1, 0, 0), 0.2f).OnComplete(() => { lunaTras.DOLocalMove(lunaInitPos, 0.2f); });
                }
            );
        yield return new WaitForSeconds(0.4f);
        monsterTras.DOLocalMove(monsterInitPos, 0.5f).OnComplete
            (
                () =>
                {
                    UIManager.Instance.ShowOrHideBattlePanel(true);
                    GameManager.Instance.PlaySound(monsterAttackSound);
                    lunaAnimator.SetBool("Defend", false);
                }
            );
    }
    public void LunaUseSkill()
    {
        if(!GameManager.Instance.CanUsePlayerMP(30))
        { 
            return; 
        }
        StartCoroutine(PerformSkillLogic());
    }

    IEnumerator PerformSkillLogic()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaAnimator.CrossFade("Skill", 0);
        GameManager.Instance.AddOrDeleteMP(-30);
        yield return new WaitForSeconds(0.35f);
        GameObject go =Instantiate(skillEffectGo, monsterTras);
        go.transform.localPosition = Vector3.zero;
        GameManager.Instance.PlaySound(lunaAttackSound);
        GameManager.Instance.PlaySound(skillSound);
        yield return new WaitForSeconds(0.4f);
        monsterSr.DOFade(0.3f, 0.2f).OnComplete(() => 
        {
            JudgeMonsterHP(-40);
        });
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(MonsterAttack());
    }

    public void LunaRecoverHP()
    {
        if (!GameManager.Instance.CanUsePlayerMP(50))
        {
            return;
        }
        StartCoroutine(PerformRecoverHPLogic());
    }
    IEnumerator PerformRecoverHPLogic()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaAnimator.CrossFade("RecoverHP", 0);
        GameManager.Instance.AddOrDeleteMP(-50);
        GameManager.Instance.PlaySound(lunaAttackSound);
        GameManager.Instance.PlaySound(recoverSound);
        yield return new WaitForSeconds(0.1f);
        GameObject go = Instantiate(recoverEffectGo, lunaTras);
        go.transform.localPosition = Vector3.zero;
        GameManager.Instance.AddOrDeleteHP(40);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MonsterAttack());
    }
    /// <summary>
    /// 改变玩家血量
    /// </summary>
    /// <param name="value"></param>
    public void JudgeLunaHP(int value)
    {
        GameManager.Instance.AddOrDeleteHP(value);
        if (GameManager.Instance.lunaCurrentHP <= 0)
        {
            GameManager.Instance.PlaySound(dieSound);
            lunaAnimator.CrossFade("Die", 0);
            lunaSr.DOFade(0, 0.8f).OnComplete(() => {
                UIManager.Instance.ShowOrHideBattlePanel(false);
                GameManager.Instance.EntryOrExitBattle(false); });
        }

    }
    /// <summary>
    /// 改变怪物血量
    /// </summary>
    /// <param name="value"></param>
    public void JudgeMonsterHP(int value)
    {
        if (GameManager.Instance.AddOrDeleteMonsterHP(value)<= 0)
        {
            GameManager.Instance.PlaySound(monsterDieSound);
            monsterSr.DOFade(0,0.5f).OnComplete(() => { GameManager.Instance.EntryOrExitBattle(false,1); });
        }
        else
        {
            monsterSr.DOFade(1, 0.2f);
        }
    }

    public void LunaEscape()
    {
        UIManager.Instance.ShowOrHideBattlePanel(false);
        lunaTras.DOLocalMove(lunaInitPos + new Vector3(5, 0, 0), 0.5f).OnComplete(() => { GameManager.Instance.EntryOrExitBattle(false); });
        lunaAnimator.SetBool("MoveState", true);
        lunaAnimator.SetFloat("MoveValue", 1f);
    }
}
