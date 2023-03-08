using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public int _score { set; get; }
    private int _maxScore;
    public int MaxScore 
    {
        set { _maxScore = value > _maxScore ? value : _maxScore; }
        get { return _maxScore; } 
    }

    void Update()
    {
        //AddScore(1);
    }

    //加分数
    public void AddScore(int score)
    {
        _score += score;
    }
}
