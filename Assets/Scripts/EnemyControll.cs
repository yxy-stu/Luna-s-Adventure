using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll : MonoBehaviour
{
    public bool vertical;//轴向控制
    public float speed=3;
    private Rigidbody2D rb;
    //方向控制
    private int direction = 1;
    //方向改变时间间隔
    private float changeTime = 3;
    //计时器
    private float timer;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.enterBattle)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            direction =-direction;
            timer = changeTime;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.enterBattle)
        {
            return;
        }
        Vector3 pos = rb.position;
        if (vertical)//垂直轴向移动
        {
            animator.SetFloat("LookX",0);
            animator.SetFloat("LookY",direction);
            pos.y=pos.y+speed*direction*Time.fixedDeltaTime;
        }
        else//水平轴向移动
        {
            animator.SetFloat("LookX",direction);
            animator.SetFloat("LookY",0);
            pos.x=pos.x+speed*direction*Time.fixedDeltaTime;
        }
        rb.MovePosition(pos);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Luna"))
        {
            GameManager.Instance.SetMonster(gameObject);
            GameManager.Instance.EntryOrExitBattle();
            UIManager.Instance.ShowOrHideBattlePanel(true);     
        }
    }
}
