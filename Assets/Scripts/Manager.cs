using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public bool isEnemyAllDie = false;
    public int currentScene;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        //气球奖励关
        if(currentScene==2) 
        {
            StartCoroutine(BubblesLevel());
        }
    }

    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && currentScene==1) NextLevel();
    }

    //奖励关倒计时
    IEnumerator BubblesLevel()
    {
        yield return new WaitForSeconds(20f);
        NextLevel();
    }
    public void NextLevel()
    {
        if(currentScene==1) 
        {
        //DontDestroyOnLoad(GameObject.Find("UIControllor"));
        SceneManager.LoadScene(2);
        }
        else if(currentScene==2) 
        {
           //Debug.Log("奖励关结束");
            GameObject.Find("UIControllor").GetComponent<UIControllor>().EndUI();
        }
    }
    
    //游戏结束
    public void GameOver()
    {
        GameObject.Find("UIControllor").GetComponent<UIControllor>().GameOverUI();
    }
}
