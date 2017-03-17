using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordUiScript : MonoBehaviour, IMenuState
{

    private CanvasGroup _canvasGroup;
    private bool _active = false;

    void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
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

    }

    public void OnStopButtonPressed()
    {

    }
}
