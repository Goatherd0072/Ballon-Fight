using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("敌人参数")] 
    public plyerState myState;
    public float upMoveSpeed;
    public bool isAttachTop = false; //是否碰到顶部
    public bool isUpMove = true; //是否向上移动
    public int ballonNum = 1; //气球数量
    public float upDistance; //上升的距离

     [Header("判断检测点")]
    public Transform checkPoint;
    public Transform bottomPoint;

    [Header("角色外观")]
    public GameObject oneBallon;
    private GameObject _Player;
    private Rigidbody2D _myRigidbody;
    private float _startPositon;
    private float _endPositon;
    void Start()
    {
        _Player = GameObject.FindWithTag("Player");
        _myRigidbody = GetComponent<Rigidbody2D>();
        _startPositon = transform.position.y;
        _endPositon = _startPositon + upDistance;
    }

    void Update()
    {
        if (transform.position.y >= _endPositon || isAttachTop)
        {
            isUpMove = false;
        }
        else if (transform.position.y <= _startPositon+0.1f)
        {
            isUpMove = true;
        }

        CheckBallonNum();
    }
    void FixedUpdate()
    {
        FaceToPlyer();

        if (isUpMove && isAttachTop == false && ballonNum>0)
        {
            EnemyUpMove();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2)
    {
        //地面检测
        if (collision2.gameObject.tag == "Ground")
        {
            myState = plyerState.OnGround;
            //Debug.Log("Enemy"+"碰撞了"+collision2.gameObject.tag);
        }
        
        //碰撞到敌人，检测是否踩到气球
        if(collision2.gameObject.tag == "Player")
        {
            if(bottomPoint.position.y >= collision2.gameObject.GetComponent<PlayerControllor>().checkPoint.position.y)
            {
                collision2.gameObject.GetComponent<PlayerControllor>().ballonNum--;
            }
        }
           
    }

    // 使敌人朝向玩家
    void FaceToPlyer()
    {
        //计算敌人和玩家间的坐标点积
        float Seta = Vector3.Dot(_Player.transform.position, transform.position);
        transform.rotation = Quaternion.Euler(0, Seta > 0 ? 180 : 0, 0);
    }

    //敌人向上行动
    void EnemyUpMove()
    {
        myState = plyerState.OnAir;
        _myRigidbody.AddForce(Vector2.up * upMoveSpeed);

    }

    //消灭敌人
    public void KillEnemy()
    {
        Destroy(this.GetComponent<Rigidbody2D>());
        Destroy(this.GetComponent<CapsuleCollider2D>());

        //坠落效果
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, -7, transform.position.z);
        if (Vector3.Distance(startPos, endPos) > 0.001f)
        {
            float speed = 0.01f;
            speed = Mathf.Min(0.1f, speed);
            Vector3 pos = Vector3.Lerp(startPos, endPos, speed);
            transform.position = pos;
        }
        else
        {
            Destroy(gameObject);
        }
    }

        //根据气球数量变化外貌
    void CheckBallonNum()
    {
        switch(ballonNum)
        {
            case 0:
                oneBallon.SetActive(false);
                KillEnemy();
                break;
            case 1:
                oneBallon.SetActive(true);;
                break;
        }
    }


}
