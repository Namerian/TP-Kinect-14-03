using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    public static GameManager Instance { get; private set; }

    //====================================================================

    [SerializeField]
    private string _danceFileFolderPath;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject _replayCharacterPrefab;
    [SerializeField]
    private GameObject _cubemanPrefab;
    [SerializeField]
    private GameObject _memorizeCharPrefab;

    [Header("Play")]
    [SerializeField]
    private Vector3 _replayCharPos;
    [SerializeField]
    private Vector3 _avatarCharPos;
    [SerializeField]
    private Vector3 _memCharPos;

    //====================================================================

    private bool _isPlaying = false;
    private string _currentDance;
    public bool _playerDetected = false;
    private MyCharacterController _replayCharacter;
    private CubemanController _cubemanCharacter;
    private Memorizer _memorizeCharacter;

    //====================================================================

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //KinectManager.Instance.GestureListeners.Add(this);
        }
        else
        {
            Debug.Log("GameManager already exists");
            Destroy(this);
        }
    }

    //====================================================================

    public string DanceFilePath { get { return _danceFileFolderPath; } }

    //====================================================================

    public void StartPlaying(string danceName)
    {
        ClearCharacters();

        _currentDance = danceName;

        _replayCharacter = Instantiate(_replayCharacterPrefab).GetComponent<MyCharacterController>();
        _replayCharacter.transform.position = _replayCharPos;

        _cubemanCharacter = Instantiate(_cubemanPrefab).GetComponent<CubemanController>();
        _cubemanCharacter.transform.position = _avatarCharPos;

        UIManager.Instance.SwitchState(UIManager.Instance.PlayUiState);

        Invoke("Play", 0.5f);
    }

    public void StartRecording()
    {
        ClearCharacters();

        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/" + GameManager.Instance.DanceFilePath);

        _memorizeCharacter = Instantiate(_memorizeCharPrefab).GetComponent<Memorizer>();
        _memorizeCharacter.transform.position = _memCharPos;

        UIManager.Instance.SwitchState(UIManager.Instance.RecordUiState);
    }

    //====================================================================


    void KinectGestures.GestureListenerInterface.UserDetected(uint userId, int userIndex)
    {
        Debug.Log("UserDetected: userId=" + userId);

        _playerDetected = true;
    }

    void KinectGestures.GestureListenerInterface.UserLost(uint userId, int userIndex)
    {

    }

    void KinectGestures.GestureListenerInterface.GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
    }

    bool KinectGestures.GestureListenerInterface.GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        return true;
    }

    bool KinectGestures.GestureListenerInterface.GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint)
    {
        return true;
    }

    //====================================================================

    private void Play()
    {
        if (!_playerDetected)
        {
            Debug.Log("player 1 missing");
            Invoke("Play", 0.5f);
            return;
        }

        Debug.Log("ttt");

        _replayCharacter.StartPlaying(Application.dataPath + _danceFileFolderPath + _currentDance);
    }

    private void ClearCharacters()
    {
        if (_replayCharacter != null)
        {
            Destroy(_replayCharacter.gameObject);
        }

        if (_cubemanCharacter != null)
        {
            Destroy(_cubemanCharacter.gameObject);
        }

        if (_memorizeCharacter != null)
        {
            Destroy(_memorizeCharacter);
        }
    }

    //====================================================================

    public void OnStartButtonPressed()
    {
        _memorizeCharacter.StartRecording();
    }

    public void OnStopButtonPressed()
    {
        _memorizeCharacter.StopRecording();
        UIManager.Instance.SwitchState(UIManager.Instance.MenuState);
    }
}
