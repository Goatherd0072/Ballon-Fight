using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverTrigger : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyBehavior>().KillEnemy();
        }
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerControllor>().PlayerDead();
        }
    }
}
