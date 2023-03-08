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
    }

  
    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && currentScene==1) NextLevel();
    }

    public void NextLevel()
    {
        //DontDestroyOnLoad(GameObject.Find("UIControllor"));
        SceneManager.LoadScene(currentScene + 1);
    }

    //游戏结束
    public void GameOver()
    {
        Destroy( GameObject.Find("Plyer"));
        GameObject.Find("UIControllor").GetComponent<UIControllor>().GameOverUI();
    }
}
