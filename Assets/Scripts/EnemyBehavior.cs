using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject Player;
    public float upMoveSpeed;
    public bool isAttachTop = false; //是否碰到顶部
    public bool isUpMove = true; //是否向上移动
    public float upDistance; //上升的距离
    private Rigidbody2D _myRigidbody;
    private float _startPositon;
    private float _endPositon;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
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

    }
    void FixedUpdate()
    {
        FaceToPlyer();

        if (isUpMove && isAttachTop == false)
        {
            EnemyUpMove();
        }
    }

    // 使敌人朝向玩家
    void FaceToPlyer()
    {
        //计算敌人和玩家间的坐标点积
        float Seta = Vector3.Dot(Player.transform.position, transform.position);
        transform.rotation = Quaternion.Euler(0, Seta > 0 ? 180 : 0, 0);
    }

    //敌人向上行动
    void EnemyUpMove()
    {
        _myRigidbody.AddForce(Vector2.up * upMoveSpeed);

    }
}
