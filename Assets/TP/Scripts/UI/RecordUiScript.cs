using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordUiScript : MonoBehaviour, IMenuState
{

    private CanvasGroup _canvasGroup;
    private bool _active = false;

    private Button _startButton;
    private Button _stopButton;

    void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _startButton = this.transform.Find("StartRecordingButton").GetComponent<Button>();
        _stopButton = this.transform.Find("StopRecordingButton").GetComponent<Button>();
    }

    //=====================================================================================

    public void OnEnter()
    {
        if (!_active)
        {
            _active = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _startButton.interactable = true;
            _stopButton.interactable = false;
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

    //=====================================================================================

    public void OnStartButtonPressed()
    {
        GameManager.Instance.OnStartButtonPressed();

        _startButton.interactable = false;
        _stopButton.interactable = true;
    }

    public void OnStopButtonPressed()
    {
        GameManager.Instance.OnStopButtonPressed();
    }
}
