using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class LunaController : MonoBehaviour
{
    private Rigidbody2D rd;
    [SerializeField] private float moveSpeed = 8f; // 建议先设8试试，不够再调大
    private Animator animator ;
    private Vector2 lookDirection = new Vector2(0, -1);
    private float moveScale;
    private Vector2 move;
    public NavMeshAgent navMeshAgent;
    private Vector3 target;
    public bool isSeekRoad;//判断是否正在寻路
    public GameObject navMeshObj;//自动寻路物体
    public static LunaController instance;
    private void Awake()
    {
        instance = this;
    }

    [Serializable]
    public class PortalPair
    {
        public Transform entry;
        public Transform exit;
    }
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        target = new Vector3(0f, 0f, 0);
        isSeekRoad = false;
    }

    void Update()
    {
        if (GameManager.Instance.enterBattle)
        {
            return;
        }
        if (!GameManager.Instance.canControlLuna)
        {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        move=new Vector2(horizontal,vertical);
        //Debug.Log(move);
        //animator.SetFloat("MoveValue", 0);
        //判断是否存在看向变化
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            lookDirection.Set(move.x,move.y);
            lookDirection.Normalize();
            //animator.SetFloat("MoveValue", 1);
        }
        //动画播放
        animator.SetFloat("Look X",lookDirection.x);
        animator.SetFloat("Look Y",lookDirection.y);
        moveScale=move.magnitude;
        if (move.magnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveScale = 2;
                moveSpeed = 4f;
            }
            else
            {
                moveScale = 1;
                moveSpeed = 2.5f;
            }
        }
        animator.SetFloat("MoveValue", moveScale);
        //检测与NPC对话
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Talk();
        }
        // 只有当 agent 启用并且在NavMesh上时，才执行寻路逻辑
        if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            if (isSeekRoad)
            {
                navMeshAgent.isStopped = false;
                SetDestination(target);
            }
            else
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.ResetPath();
            }
        }
        UpdateSeekAnimation();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.enterBattle)
        {
            return;
        }
        Vector2 targetPos = rd.position + move * moveSpeed * Time.fixedDeltaTime;
        rd.MovePosition(targetPos);
    }
    public void Climb(bool start)
    {
        animator.SetBool("Climb",start);
    }
    public void Jump(bool start)
    {
        animator.SetBool("Jump", start);
        rd.simulated = !start;
        navMeshAgent.enabled = !start;//禁用agent组件
    }

    public void Talk()
    {
        Collider2D collider = Physics2D.OverlapCircle(rd.position, 0.5f, LayerMask.GetMask("NPC"));
        if(collider != null)
        {
            if (collider.name == "Nala")
            {
                GameManager.Instance.canControlLuna=false;
                collider.GetComponent<NPCDialogic>().DisplayDialog();
            }
            if(collider.name == "Dog" && !GameManager.Instance.hasPetTheDog && GameManager.Instance.dialogInfoIndex==2)
            {
                PetTheDog();
                GameManager.Instance.canControlLuna = false;
                collider.GetComponent<Dog>().BeHappy();
            }
        }
    }

    private void PetTheDog()
    {
        animator.CrossFade("PetTheDog", 0);
        transform.position = new Vector3(-3.07f, -7.61f, 0);
    }
    private void SetDestination(Vector3 pos)
    {
        float agentOffSet = 0.0001f;
        Vector3 agentPos = (Vector3)(agentOffSet * UnityEngine.Random.insideUnitCircle) + pos;//随机生成一个半径在0.0001f以内的向量，并加上目标点,作为agent的位置,避免agent和目标点重合
        agentPos.z = 0;
        navMeshAgent.SetDestination(agentPos);
    }
    public void SetSeekTarget(Vector3 targetPos)
    {
        target = targetPos;
        isSeekRoad = true;
    }
    private void UpdateSeekAnimation()
    {
        if (!isSeekRoad || navMeshAgent == null || !navMeshAgent.enabled)
        {
            return;
        }
        if (navMeshAgent.hasPath && navMeshAgent.remainingDistance <= 0.1f)
        {
            isSeekRoad = false;          // 停止寻路
            animator.SetFloat("MoveValue", 0); // 停止移动动画
            return;
        }
        // 获取寻路方向
        Vector2 dir = navMeshAgent.velocity.normalized;
        if (dir.magnitude > 0.1f)
        {
            // 设置朝向
            lookDirection = dir;
            animator.SetFloat("Look X", dir.x);
            animator.SetFloat("Look Y", dir.y);
            // 播放跑步动画
            animator.SetFloat("MoveValue", 1.5f);
        }
    }
}