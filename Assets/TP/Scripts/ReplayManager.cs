using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    [SerializeField]
    private GameObject _replayCharPrefab;
    [SerializeField]
    private string _dataFile;

    private MyCharacterController _charController;

    // Use this for initialization
    void Start()
    {
        GameObject obj = Instantiate<GameObject>(_replayCharPrefab);
        _charController = obj.GetComponent<MyCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint)
    {
        return true;
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        return true;
    }

    public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
    }

    public void UserDetected(uint userId, int userIndex)
    {
        _charController.StartPlaying(_dataFile);
    }

    public void UserLost(uint userId, int userIndex)
    {
    }
}
