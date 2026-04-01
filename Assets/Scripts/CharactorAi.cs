using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharactorAi : MonoBehaviour
{
    public NavMeshAgent meshAgent;
    public Transform target;
    void Start()
    {
        meshAgent.updateRotation=false;
        meshAgent.updateUpAxis=false;
    }

    // Update is called once per frame
    void Update()
    {
        //插件包存在问题，竖向移动可能因为y值无偏差导致敌人不跟随移动，调用函数生成细微位置偏差，确保敌人跟随移动
        SetDestination(target.position);
    }
    private void SetDestination(Vector3 pos)
    {
        float agentOffSet = 0.0001f;
        Vector3 agentPos = (Vector3)(agentOffSet * Random.insideUnitCircle) + pos;//随机生成一个半径在0.0001f以内的向量，并加上目标点,作为agent的位置,避免agent和目标点重合
        meshAgent.SetDestination(agentPos);
    }
}
