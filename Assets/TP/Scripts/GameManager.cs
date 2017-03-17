using System;
using System.Collections;
using System.Collections.Generic;
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
    private GameObject _avatarPrefab;

    [Header("Play")]
    [SerializeField]
    private Vector3 _replayCharPos;
    [SerializeField]
    private Vector3 _avatarCharPos;

    //====================================================================

    private bool _isPlaying = false;
    private string _currentDance;
    public bool _playerDetected = false;
    private MyCharacterController _replayCharacter;
    private AvatarController _avatarCharacter;

    //====================================================================

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            KinectManager.Instance.GestureListeners.Add(this);
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
        _currentDance = danceName;

        Debug.Log("eee");

        _replayCharacter = Instantiate(_replayCharacterPrefab).GetComponent<MyCharacterController>();
        _replayCharacter.transform.position = _replayCharPos;

        _avatarCharacter = Instantiate(_avatarPrefab).GetComponent<AvatarController>();
        _avatarCharacter.transform.position = _avatarCharPos;

        UIManager.Instance.SwitchState(UIManager.Instance.PlayUiState);

        Debug.Log("rrr");

        Invoke("Play", 0.5f);
    }

    //====================================================================


    void KinectGestures.GestureListenerInterface.UserDetected(uint userId, int userIndex)
    {
        if(userId == 1)
        {
            _playerDetected = true;
        }
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
            Invoke("Play", 0.5f);
            return;
        }

        Debug.Log("ttt");

        _replayCharacter.StartPlaying(Application.dataPath + _danceFileFolderPath + _currentDance);
    }
}
