using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bubbles : MonoBehaviour
{
    private Vector3 _starPos;
    private Vector3 _moveSpeed;

    void Start()
    {
        _starPos = this.transform.position;
        _moveSpeed = RandomGenerator();
        //Debug.Log(_starPos);
    }

    void Update()
    {
        MoveRandom();

        if(transform.position.y > 6.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ScoreCounter._GetBubbleNum++;
            Destroy(gameObject);
        }
    }

    //气泡向上随机移动
    void MoveRandom()
    {
        //水平向左右随机摆动
        if(transform.position.x - _starPos.x >0.2f || transform.position.x - _starPos.x < -0.2f)
        {
            //Debug.Log(_moveSpeed);
            _moveSpeed.x *= -1;
        }
        
        transform.Translate(_moveSpeed * Time.deltaTime);

    }

    //生成随机的随机数
    Vector3 RandomGenerator()
    {
        float x = Random.Range(-2f, 2f);
        float y = Random.Range(0.5f, 2f);

        return new Vector3(x + 0.5f, y, 0);
    }
}
