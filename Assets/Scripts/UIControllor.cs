using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControllor : MonoBehaviour
{
    private TMP_Text _scoreText;
    private int _score;
    private GameObject _Menu;
    public GameObject floatScore;
    public GameObject canvas;
    void Start()
    {
        _scoreText = GameObject.Find("Score_Text").GetComponent<TMP_Text>();
        _score = this.GetComponent<ScoreCounter>()._score;

        _Menu = GameObject.Find("Menu");
        _Menu.SetActive(false);
    }

    void Update()
    {
        UpdateUIScore();
        MenuControl();
    }

    //更新UI分数
    public void UpdateUIScore()
    {
        _score = this.GetComponent<ScoreCounter>()._score;
        _scoreText.text = _score.ToString();
    }
    
    //敌人死亡时的分数浮现
    public void FloatScore (Transform p, int score) 
    {
        this.GetComponent<ScoreCounter>().AddScore(score);

        //获取分数对应的文字组件，并进行坐标转换
        GameObject popupScore = (GameObject)Instantiate (floatScore);
        popupScore.GetComponent<TextMeshProUGUI>().text = score.ToString();
        
        popupScore.transform.position = Camera.main.WorldToScreenPoint(p.position);    
        popupScore.transform.SetParent (canvas.transform,true);

        //一段时间销毁
        StartCoroutine (DestroyFloatScore (popupScore));
    }

    IEnumerator DestroyFloatScore (GameObject popupScore) 
    {
        yield return new WaitForSeconds (1f);
        Destroy (popupScore);
    }


    //ESC键打开关闭菜单
    public void MenuControl()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _Menu.SetActive(_Menu.activeSelf ? false : true);
            Time.timeScale = _Menu.activeSelf ? 0 : 1;
        }       
    }

    //恢复游戏
    public void ResumeGame()
    {
        Time.timeScale = 1;
        _Menu.SetActive(false);
    }

    //下一关
    public void NextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    //重新开始游戏
    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }

    //主菜单：
    //开始游戏
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
