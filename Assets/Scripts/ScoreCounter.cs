using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private Text _scoreCounter;
    private int _score;
    
    private void Update()
    {
        _scoreCounter.text = $"SCORE\n{_score}";
    }

    public void SetScore(int change)
    {
        _score += change;
        if (_score < 0)
        {
            _score = 0;
        }
    }
}
