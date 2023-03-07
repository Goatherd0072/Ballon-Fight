using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTrigger : MonoBehaviour
{
    //防止敌人越过顶部
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("碰到了");
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyBehavior>().isAttachTop = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("离开了");
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyBehavior>().isAttachTop = false;
        }
    }
}
