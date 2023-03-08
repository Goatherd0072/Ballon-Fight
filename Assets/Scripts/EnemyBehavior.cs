using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    [Header("敌人参数")] 
    public plyerState myState;
    public float upMoveSpeed;
    public bool isAttachTop = false; //是否碰到顶部
    public bool isUpMove = true; //是否向上移动
    public bool isReset = false; //是否在重整
    public int ballonNum = 1; //气球数量
    public float upDistance; //上升的距离
    public float resetTime = 5f; //气球破了落地，但未被杀死，进行重整的时间

     [Header("判断检测点")]
    public Transform checkPoint;
    public Transform bottomPoint;

    [Header("角色外观")]
    public GameObject oneBallon;
    private GameObject _Player;
    private Rigidbody2D _myRigidbody;
    private float _startPositon;
    private float _endPositon;

    void Awake()
    {
       
    }
    void Start()
    {
        _Player = GameObject.FindWithTag("Player");
        _myRigidbody = GetComponent<Rigidbody2D>();
        _startPositon = transform.position.y;
        _endPositon = _startPositon + upDistance;
    }

    void Update()
    {
        if(isReset == false)
        {   
            //简单的上下移动
            if (transform.position.y >= _endPositon || isAttachTop)
            {
                isUpMove = false;
            }
            else if (transform.position.y <= _startPositon+0.1f)
            {
                isUpMove = true;
            }
        }
        CheckBallonNum();
        BoundaryCheck();
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
                PlayerControllor.ballonNum--;
            }
        }
           
    }
    private void OnCollisionExit2D(Collision2D collision2)
    {
        //地面检测
        if (collision2.gameObject.tag == "Ground")
        {
            myState = plyerState.OnAir;
        }
    }

    // 使敌人朝向玩家
    void FaceToPlyer()
    {
        // //计算敌人和玩家间的坐标点积
        // float Seta = Vector3.Dot(_Player.transform.position, transform.position);
        // transform.rotation = Quaternion.Euler(0, Seta > 0 ? 180 : 0, 0);
        transform.rotation = Quaternion.Euler(0, ((_Player.transform.position.x - transform.position.x) > 0) ? 180 : 0, 0);
    }
    void BoundaryCheck()
    {
        if(transform.position.x>_Player.GetComponent<PlayerControllor>().boundaryDistance)
        {
            transform.position = new Vector3(-_Player.GetComponent<PlayerControllor>().boundaryDistance, transform.position.y, transform.position.z);
        }
        else if(transform.position.x<-_Player.GetComponent<PlayerControllor>().boundaryDistance)
        {
            transform.position = new Vector3(_Player.GetComponent<PlayerControllor>().boundaryDistance, transform.position.y, transform.position.z);
        }
    }

    //敌人向上行动
    void EnemyUpMove()
    {
        myState = plyerState.OnAir;
        _myRigidbody.AddForce(Vector2.up * upMoveSpeed);

    }

    //在地面上重整
    void GetReset()
    {
        isReset = true;
        isUpMove = false;

        //到地面上了
        if(myState == plyerState.OnGround && ballonNum == 0)
        {
            //重整时间过后，复活敌人
            StartCoroutine(ResetEnemy());
        }
        
    }
    IEnumerator ResetEnemy()
    {
        yield return new WaitForSeconds(resetTime);
        isReset = false;
        //isUpMove = true;
        ballonNum = 1;
    }

    //消灭敌人
    public void KillEnemy()
    {
        ballonNum = -1;
        isAttachTop = false; 
        isUpMove = false; 
        isReset = false;
        //Debug.Log("敌人死亡");
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
        if(transform.position.y <= -6f)
        {
            Destroy(gameObject);
        }
    }

    //根据气球数量变化外貌
    void CheckBallonNum()
    {
        switch(ballonNum)
        {
            case -1:
                oneBallon.SetActive(false);
                KillEnemy();
                break;
            case 0:
                oneBallon.SetActive(false);
                GetReset();
                break;
            case 1:
                oneBallon.SetActive(true);;
                break;
        }
    }

    public void ScoreAdd(int s)
    {
        //在空中击杀多 +500分
        if(ballonNum == 0 && myState == plyerState.OnAir)
        {
            s += 500;
        }

        GameObject.Find("UIControllor").GetComponent<UIControllor>().FloatScore(transform,s);
    }
   
}
