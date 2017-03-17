using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenuScript : MonoBehaviour, IMenuState
{
    private CanvasGroup _canvasGroup;
    private bool _active = false;

    private List<string> _danceFiles = new List<string>();
    private Transform _danceListContentTransform;
    private List<GameObject> _danceListElements = new List<GameObject>();

    void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _danceListContentTransform = this.transform.Find("Scroll View/Viewport/Content");
    }

    public void OnEnter()
    {
        if (!_active)
        {
            _active = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _danceFiles.Clear();
            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/" + GameManager.Instance.DanceFilePath);

            foreach (FileInfo danceFile in dirInfo.GetFiles("*.txt"))
            {
                _danceFiles.Add(danceFile.Name);

                GameObject listElement = Instantiate(Resources.Load<GameObject>("Prefabs/DanceSelectionListElement"), _danceListContentTransform);
                listElement.GetComponent<Button>().onClick.AddListener(delegate { OnDanceListElementPressed(danceFile.Name); });
                listElement.transform.Find("Text").GetComponent<Text>().text = danceFile.Name;
            }
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

    private void OnDanceListElementPressed(string danceName)
    {
        Debug.Log("element button pressed: " + danceName);

        _canvasGroup.interactable = false;
        GameManager.Instance.StartPlaying(danceName);
    }
}
