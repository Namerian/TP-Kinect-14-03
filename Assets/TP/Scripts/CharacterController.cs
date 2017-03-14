using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private const char DELIMITER = ',';

    //===================================================================
    // public variables
    //===================================================================

    [SerializeField]
    private bool MoveVertically = false;
    [SerializeField]
    private bool MirroredMovement = false;

    //public GameObject debugText;
    [SerializeField]
    private GameObject Hip_Center;
    [SerializeField]
    private GameObject Spine;
    [SerializeField]
    private GameObject Shoulder_Center;
    [SerializeField]
    private GameObject Head;
    [SerializeField]
    private GameObject Shoulder_Left;
    [SerializeField]
    private GameObject Elbow_Left;
    [SerializeField]
    private GameObject Wrist_Left;
    [SerializeField]
    private GameObject Hand_Left;
    [SerializeField]
    private GameObject Shoulder_Right;
    [SerializeField]
    private GameObject Elbow_Right;
    [SerializeField]
    private GameObject Wrist_Right;
    [SerializeField]
    private GameObject Hand_Right;
    [SerializeField]
    private GameObject Hip_Left;
    [SerializeField]
    private GameObject Knee_Left;
    [SerializeField]
    private GameObject Ankle_Left;
    [SerializeField]
    private GameObject Foot_Left;
    [SerializeField]
    private GameObject Hip_Right;
    [SerializeField]
    private GameObject Knee_Right;
    [SerializeField]
    private GameObject Ankle_Right;
    [SerializeField]
    private GameObject Foot_Right;

    [SerializeField]
    private LineRenderer SkeletonLine;

    //===================================================================
    // private variables
    //===================================================================

    private GameObject[] _bones;
    private LineRenderer[] _lines;
    private int[] _parentIndexes;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Vector3 _initialPosOffset = Vector3.zero;
    private uint _initialPosUserID = 0;

    private bool _isRecording = false;
    private bool _isPlaying = false;

    private StreamReader _fileReader = null;
    private float _playTime = 0f;
    private string _playLine = string.Empty;

    private float _startTime = 0f;
    private float _currentTime = 0f;
    private int _currentFrame = 0;

    private KinectManager _kinectManager = null;

    //===================================================================
    // monobehaviour methods
    //===================================================================

    // Use this for initialization
    void Start()
    {
        //store bones in a list for easier access
        _bones = new GameObject[] {
            Hip_Center, Spine, Shoulder_Center, Head,  // 0 - 3
			Shoulder_Left, Elbow_Left, Wrist_Left, Hand_Left,  // 4 - 7
			Shoulder_Right, Elbow_Right, Wrist_Right, Hand_Right,  // 8 - 11
			Hip_Left, Knee_Left, Ankle_Left, Foot_Left,  // 12 - 15
			Hip_Right, Knee_Right, Ankle_Right, Foot_Right  // 16 - 19
		};

        _parentIndexes = new int[] {
            0, 0, 1, 2,
            2, 4, 5, 6,
            2, 8, 9, 10,
            0, 12, 13, 14,
            0, 16, 17, 18
        };

        //******************************************************************
        //manager stuff

        if (!_kinectManager)
        {
            _kinectManager = KinectManager.Instance;
        }

        //******************************************************************
        //rendering stuff

        // array holding the skeleton lines
        _lines = new LineRenderer[_bones.Length];

        if (SkeletonLine)
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i] = Instantiate(SkeletonLine) as LineRenderer;
                _lines[i].transform.parent = transform;
            }
        }

        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        //transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        //**********************************************************************
        //playing
        if (_isPlaying)
        {
            // wait for the right time
            _currentTime = Time.time;
            float deltaTime = _currentTime - _startTime;

            if (_playLine != null && deltaTime >= _playTime)
            {
                // then play the line
                if (_kinectManager && _playLine.Length > 0)
                {
                    SetBodyData(_playLine);
                }

                // and read the next line
                ReadLineFromFile();
            }

            if (_playLine == null)
            {
                // finish playing, if we reached the EOF
                StopRecordingOrPlaying();
            }
        }



        //**********************************************************************
        //rendering
        if (SkeletonLine)
        {
            for (int i = 0; i < _bones.Length; i++)
            {
                bool bLineDrawn = false;

                if (_bones[i] != null)
                {
                    if (_bones[i].gameObject.activeSelf)
                    {
                        Vector3 posJoint = _bones[i].transform.position;

                        int parI = _parentIndexes[i];
                        Vector3 posParent = _bones[parI].transform.position;

                        if (_bones[parI].gameObject.activeSelf)
                        {
                            _lines[i].gameObject.SetActive(true);

                            //lines[i].SetVertexCount(2);
                            _lines[i].SetPosition(0, posParent);
                            _lines[i].SetPosition(1, posJoint);

                            bLineDrawn = true;
                        }
                    }
                }

                if (!bLineDrawn)
                {
                    _lines[i].gameObject.SetActive(false);
                }
            }
        }

    }

    //===================================================================
    // public methods
    //===================================================================

    public bool StartPlaying(string filePath)
    {
        if (_isPlaying)
            return false;

        _isPlaying = true;

        // avoid recording an playing at the same time
        if (_isRecording && _isPlaying)
        {
            _isRecording = false;
            Debug.Log("Recording stopped.");
        }

        // stop playing if there is no file name specified
        if (filePath.Length == 0 || !File.Exists(filePath))
        {
            _isPlaying = false;
            Debug.LogError("No file to play.");
        }

        if (_isPlaying)
        {
            Debug.Log("Playing started.");

            // initialize times
            _startTime = _currentTime = Time.time;
            _currentFrame = -1;

            // open the file and read a line
            _fileReader = new StreamReader(filePath);

            ReadLineFromFile();

            // enable the play mode
            /*if (_kinectManager)
            {
                _kinectManager.EnablePlayMode(true); //this methods does not exist for Kinect 1!
            }*/
        }

        return _isPlaying;
    }

    // stops recording or playing
    public void StopRecordingOrPlaying()
    {
        if (_isRecording)
        {
            _isRecording = false;

            Debug.Log("Recording stopped.");
        }

        if (_isPlaying)
        {
            // close the file, if it is playing
            CloseFile();
            _isPlaying = false;

            Debug.Log("Playing stopped.");
        }
    }

    //===================================================================
    // private methods
    //===================================================================

    // reads a line from the file
    private bool ReadLineFromFile()
    {
        if (_fileReader == null)
            return false;

        // read a line
        _playLine = _fileReader.ReadLine();
        if (_playLine == null)
            return false;

        // extract the unity time and the body frame
        char[] delimiters = { '|' };
        string[] sLineParts = _playLine.Split(delimiters);

        if (sLineParts.Length >= 2)
        {
            float.TryParse(sLineParts[0], out _playTime);
            _playLine = sLineParts[1];
            _currentFrame++;

            return true;
        }

        return false;
    }

    // close the file and disable the play mode
    private void CloseFile()
    {
        // close the file
        if (_fileReader != null)
        {
            _fileReader.Dispose();
            _fileReader = null;
        }

        // disable the play mode
        /*if (_kinectManager)
        {
            _kinectManager.EnablePlayMode(false); //this methods does not exist for Kinect 1!
        }*/
    }

    private void SetBodyData(string bodyData)
    {
        string[] parsedBodyData = bodyData.Split(DELIMITER);

        for (int joint = 0; joint < Enum.GetValues(typeof(Bones)).Length; joint++)
        {
            float x, y, z;

            float.TryParse(parsedBodyData[joint * 3], out x);
            float.TryParse(parsedBodyData[joint * 3 + 1], out y);
            float.TryParse(parsedBodyData[joint * 3 + 2], out z);

            _bones[joint].transform.localPosition = new Vector3(x, y, z);
        }
    }
}
