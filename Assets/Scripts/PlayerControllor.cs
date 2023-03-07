using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum plyerState //玩家的两种状态，地面和空中
{
    OnGround,
    OnAir
}

public class PlayerControllor : MonoBehaviour
{
    [Header("玩家参数")]
    public bool canMove = false;
    public plyerState myState; //玩家的状态
    public float moveSpeed; //玩家的移动速度
    public float jumpSpeed; //玩家的跳跃速度
    public float maxXSpeed = 3f; //最大x轴的移动速度
    public float maxYSpeed = 3f; //最大y轴的移动速度
    public float boundaryDistance; //屏幕的边界的距离
    public int ballonNum = 1; //气球数量
    public int healthNum = 2; //生命数量
    public float respawnTime = 1f; //复活时间

    [Header("判断检测点")]
    public Transform checkPoint;//气球检测点
    public Transform bottomPoint;//底部检测点
    public Transform respawnPoint;//复活点

    [Header("角色外观")]
    public GameObject oneBallon;//一个气球
    public GameObject twoBallon;//两个气球

    private Rigidbody2D _myRigidbody; //刚体
    void Awake()
    {
        //检测并赋值Rigidbody
        if(gameObject.GetComponent<Rigidbody2D>())
        {        
            _myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        }
        else
        {
            _myRigidbody = gameObject.AddComponent<Rigidbody2D>();
            _myRigidbody.bodyType = RigidbodyType2D.Dynamic;
            _myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        BoundaryCheck();
        CheckBallonNum();
    }
    private void FixedUpdate()
    {
        Movement();
        Jump();
        // 限制移动速度
        if(Mathf.Abs(_myRigidbody.velocity.x)>maxXSpeed || Mathf.Abs(_myRigidbody.velocity.y)>maxYSpeed)
        {

            // _myRigidbody.velocity = _myRigidbody.velocity.normalized * maxYSpeed;
        }
    }

    // x轴的移动
    void Movement()
    {
        float faceDir = Input.GetAxisRaw("Horizontal"); //面朝的方向  Input.GetAxisRaw("Horizontal")直接获取-1，0，1
        if (faceDir != 0)
        {
            transform.rotation = Quaternion.Euler(0, faceDir > 0 ? 0 : 180, 0);
        } 

        if(canMove || myState==plyerState.OnGround)
        {
            _myRigidbody.AddForce(Vector2.right* moveSpeed*Input.GetAxisRaw("Horizontal"));
            //_myRigidbody.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), 0);
        }
    }
    //y轴的移动
    void Jump()
    {
        //长按w键或空格跳跃
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            _myRigidbody.AddForce(Vector2.up* jumpSpeed);
            //_myRigidbody.velocity = new Vector2(0,  jumpSpeed);
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    //碰撞检测
    private void OnCollisionEnter2D(Collision2D collision2)
    {
        //地面检测
        if (collision2.gameObject.tag == "Ground")
        {
            myState = plyerState.OnGround;
            Debug.Log("Player"+"碰撞了"+collision2.gameObject.tag);
        }
        
        //碰撞到敌人，检测是否踩到气球
        if(collision2.gameObject.tag == "Enemy")
        {
            if(bottomPoint.position.y >= collision2.gameObject.GetComponent<EnemyBehavior>().checkPoint.position.y)
            {
                collision2.gameObject.GetComponent<EnemyBehavior>().ballonNum--;
            }
        }
        
        
    }
    private void OnCollisionExit2D(Collision2D collision2)
    {
        //地面检测
        if (collision2.gameObject.tag == "Ground")
        {
            myState = plyerState.OnAir;
            Debug.Log("Player"+"离开了"+collision2.gameObject.tag);
        }
    }
    
    //边界检测,防止玩家超过屏幕
    void BoundaryCheck()
    {
        if(transform.position.x>boundaryDistance)
        {
            transform.position = new Vector3(-boundaryDistance, transform.position.y, transform.position.z);
        }
        else if(transform.position.x<-boundaryDistance)
        {
            transform.position = new Vector3(boundaryDistance, transform.position.y, transform.position.z);
        }
    }

    //玩家的死亡检测
    void CheckDead()
    {
        if(healthNum >0)
        {
            ballonNum = 2;
            healthNum--;
            transform.position = new Vector3(10, 10, 10);
            StartCoroutine(Respawn());
        }
        else 
        {
            PlayerDead();
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        transform.position = respawnPoint.position;
    }

    public void PlayerDead()
    {
        Debug.Log("Player死亡");
    }

    //根据气球数量变化外貌
    void CheckBallonNum()
    {
        switch(ballonNum)
        {
            case 0:
                oneBallon.SetActive(false);
                twoBallon.SetActive(false);
                CheckDead();
                break;
            case 1:
                oneBallon.SetActive(true);
                twoBallon.SetActive(false);
                break;
            case 2:
                oneBallon.SetActive(false);                
                twoBallon.SetActive(true);
                break;
        }
    }
}
