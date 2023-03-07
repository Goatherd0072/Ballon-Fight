using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonTrigger : MonoBehaviour
{
    private string _myTag; //获取自身的标签
    private string _opponentTag; //获取对手的标签
    void Awake()
    {
        //找到该气球所属的角色的标签
        _myTag = transform.parent.tag;
        _opponentTag = (_myTag == "Player") ? "Enemy" : "Player";
        //Debug.Log(_myTag);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == _opponentTag)
        {
           
        }
    }
}
