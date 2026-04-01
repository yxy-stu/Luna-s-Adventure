using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpArea : MonoBehaviour
{
    public Transform jumpPointA;
    public Transform jumpPointB;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Luna"))
        {
            LunaController lunaController=collision.GetComponent<LunaController>();
            lunaController.Jump(true);
            float distA=Vector3.Distance(lunaController.transform.position,jumpPointA.position);
            float distB = Vector3.Distance(lunaController.transform.position, jumpPointB.position);
            Transform targetTrans;
            //if (distA > distB)
            //{
            //    targetTrans = jumpPointA;
            //}
            //else
            //{
            //    targetTrans = jumpPointB;
            //}
            targetTrans=distA>distB?jumpPointA:jumpPointB;
            lunaController.transform.DOMove(targetTrans.position, 0.5f).SetEase(Ease.Linear).OnComplete(() => EndJump(lunaController));
            Transform lunalocalTrans=lunaController.transform.GetChild(0);
            Sequence sequence=DOTween.Sequence();
            sequence.Append(lunalocalTrans.DOLocalMoveY(1.2f, 0.25f).SetEase(Ease.InOutSine));
            sequence.Append(lunalocalTrans.DOLocalMoveY(0.5f, 0.25f).SetEase(Ease.InOutSine));
            sequence.Play();
        }
    }

    private void EndJump(LunaController lunaController)
    {
        lunaController.Jump(false);
    }
}
