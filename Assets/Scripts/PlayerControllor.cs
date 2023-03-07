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
    public bool canMove = false;
    public plyerState myState; //玩家的状态
    public float moveSpeed; //玩家的移动速度
    public float jumpSpeed; //玩家的跳跃速度
    public float maxXSpeed = 3f; //最大x轴的移动速度
    public float maxYSpeed = 3f; //最大y轴的移动速度

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
        Debug.Log("碰撞了");
        //地面检测
        if (collision2.gameObject.tag == "Ground")
        {
            myState = plyerState.OnGround;
        }  
    }
    private void OnCollisionExit2D(Collision2D collision2)
    {
        Debug.Log("离开了");
        //地面检测
        if (collision2.gameObject.tag == "Ground")
        {
            myState = plyerState.OnAir;
        }
    }

}
