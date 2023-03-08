using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControllor : MonoBehaviour
{
    private TMP_Text _scoreText;
    private int _score;
    public GameObject floatScore;
    public GameObject canvas;
    public GameObject LoseText;
    public GameObject Menu;
    public GameObject Health;
    public Transform HealthPos;
    public GameObject endUI;
    public TMP_Text endUIGetBum;
    public TMP_Text endUISum;
    void Start()
    {
        _scoreText = GameObject.Find("Score_Text").GetComponent<TMP_Text>();
        _score = ScoreCounter._score;

        Menu.SetActive(false);
    }

    void Update()
    {
        UpdateUIScore();
        MenuControl();
        HealthNumUI();
    }

    //更新UI分数
    public void UpdateUIScore()
    {
        _score = ScoreCounter._score;
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


    //更新左上角的❤的数量
    void HealthNumUI()
    {
        int healthNum = PlayerControllor.healthNum;
        int healthNumUI= HealthPos.transform.childCount;

        if(healthNumUI == 0)
        {
            GameObject health = (GameObject)Instantiate(Health);
            health.transform.position = HealthPos.position;
            health.transform.SetParent(HealthPos);        
            healthNumUI= HealthPos.transform.childCount;
        }

        if(healthNumUI < healthNum)
        {
            for(int i = 0; i < healthNum - healthNumUI; i++)
            {
                GameObject health = (GameObject)Instantiate(Health);
                health.transform.SetParent(HealthPos.transform);
                //新❤的位置是最后一个❤的位置的x轴+50
                health.gameObject.GetComponent<RectTransform>().position = HealthPos.transform.GetChild(healthNumUI - 1).gameObject.GetComponent<RectTransform>().position + new Vector3(50, 0, 0);
            }
        }
        else if(healthNumUI > healthNum)
        {
            for(int i = healthNumUI; i > healthNum; i++)
            {
                Destroy(HealthPos.transform.GetChild(i-1).gameObject);
            }

        }

    }

    //                                          ESC键控制菜单
    //ESC键打开关闭菜单
    public void MenuControl()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(Menu.activeSelf ? false : true);
            Time.timeScale = Menu.activeSelf ? 0 : 1;
        }       
    }

    //游戏失败UI
    public void GameOverUI()
    {
        Menu.SetActive(true);
        LoseText.SetActive(true);
    }

    //恢复游戏
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Menu.SetActive(false);
    }

    //下一关
    public void NextLevel()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    //返回主菜单
    public void BackToMenu()
    {
        ScoreCounter._GetBubbleNum = 0;
        ScoreCounter._score = 0;
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }

    //                              主菜单：
    //开始游戏
    public void StartGame()
    {
        PlayerControllor.ballonNum = 2; //气球数量
        PlayerControllor.healthNum = 2;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    //                              奖励关
    //结束UI
    public void EndUI()
    {
        Time.timeScale = 0;
        int getBubbles = ScoreCounter._GetBubbleNum;
        endUIGetBum.text = getBubbles.ToString();

        this.GetComponent<ScoreCounter>().AddScore(getBubbles * 300);
        endUISum.text = (getBubbles * 300).ToString();

        endUI.SetActive(true);
        Destroy(Menu);
    }
}
