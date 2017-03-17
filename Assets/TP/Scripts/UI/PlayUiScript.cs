using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUiScript : MonoBehaviour, IMenuState
{
    private CanvasGroup _canvasGroup;
    private bool _active = false;

    private Text _scoreText;

    void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _scoreText = this.transform.Find("ScoreText").GetComponent<Text>();
    }

    public void OnEnter()
    {
        if (!_active)
        {
            _active = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnExit()
    {
        if (_active)
        {
            _active = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public void SetScore(int score)
    {
        Debug.Log("xxx");

        if(_active && _scoreText != null)
        {
            _scoreText.text = "Score: " + score;
        }
    }
}
