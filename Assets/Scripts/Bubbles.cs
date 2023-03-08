using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bubbles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            Destroy(gameObject);
        }
    }

    //气泡向上随机移动
    void MoveUpRandom()
    {

    }

    //生成随机的随机数
    Vector2 RandomGenerator()
    {
        System.Random random = new System.Random();
        int x = random.Next(-1, 1);
        int y = random.Next(1, 2);

        return new Vector2(x, y);
    }
}
