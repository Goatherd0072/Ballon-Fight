using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    public GameObject bubble;
    public Transform bubblePoint; //气泡生成点

    void Start()
    {
        bubblePoint = gameObject.transform.GetChild(0);
        StartCoroutine(CheckGen());
    }

    //随机生成泡泡
    void BubblesGenerator()
    {
        GameObject bubbleObj = Instantiate(bubble, bubblePoint.position, Quaternion.identity);

        StartCoroutine(CheckGen());
    }


    IEnumerator CheckGen()
    {
        float time = Random.Range(1f, 5f);
        yield return new WaitForSeconds(time);
        BubblesGenerator();
    }
}
