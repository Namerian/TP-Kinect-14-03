using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Memorizer : MonoBehaviour
{

    private const char DELIMITER = ',';
    //private const string FILEPATH = "Assets/TP/M.txt";

    private Quaternion _initialRotation;
    private float _startTime;

    private bool _isRecording;
    private string _filePath;
    StreamWriter _streamWriter;

    // Update is called once per frame
    void Update()
    {
        if (!_isRecording)
        {
            return;
        }

        string bodyData = GetBodyData();

        if (bodyData != null && bodyData.Length > 0)
        {
            //*********************

            /*using (StreamWriter writer = File.AppendText(_filePath))
            {
                string sRelTime = string.Format("{0:F3}", (Time.time - _startTime));
                writer.WriteLine(sRelTime + "|" + bodyData);
            }*/

            string sRelTime = string.Format("{0:F3}", (Time.time - _startTime));
            _streamWriter.WriteLine(sRelTime + "|" + bodyData);

            //*********************
        }
        else
        {
            Debug.Log("Something went wrong!");
        }
    }

    public void StartRecording()
    {
        if (_isRecording)
        {
            return;
        }

        string path = Application.dataPath + GameManager.Instance.DanceFilePath;
        int index = 1;
        string name = "Danse";
        string fileName = name + index + ".txt";

        while (true)
        {
            if (File.Exists(path+fileName))
            {
                Debug.Log("File exists: " + fileName);
                index++;
                fileName = name + index + ".txt";
            }
            else
            {
                _filePath = path + fileName;
                _streamWriter = File.CreateText(_filePath);
                break;
            }
        }

        _startTime = Time.time;

        _isRecording = true;
    }

    public void StopRecording()
    {
        _streamWriter.Close();

        _isRecording = false;
    }

    private string GetBodyData()
    {
        KinectManager kinectManager = KinectManager.Instance;
        StringBuilder result = new StringBuilder();

        uint playerID = kinectManager.GetPlayer1ID();

        if (playerID <= 0)
        {
            Debug.Log("player1Id not found!");
            return null;
        }

        Vector3 posPointMan = kinectManager.GetUserPosition(playerID);

        //**********************

        for (int joint = 0; joint < Enum.GetValues(typeof(Bones)).Length; joint++)
        {

            if (kinectManager.IsJointTracked(playerID, joint))
            {

                Vector3 posJoint = kinectManager.GetJointPosition(playerID, joint);

                //Quaternion rotJoint = kinectManager.GetJointOrientation(playerID, joint, false);
                //rotJoint = initialRotation * rotJoint;

                posJoint -= posPointMan;

                //bones[i].transform.localPosition = posJoint;
                //bones[i].transform.rotation = rotJoint;

                result.AppendFormat("{0:F3}", posJoint.x).Append(DELIMITER);
                result.AppendFormat("{0:F3}", posJoint.y).Append(DELIMITER);
                result.AppendFormat("{0:F3}", posJoint.z).Append(DELIMITER);

            }
            else
            {
                result.AppendFormat("{0:F3}", 0).Append(DELIMITER);
                result.AppendFormat("{0:F3}", 0).Append(DELIMITER);
                result.AppendFormat("{0:F3}", 0).Append(DELIMITER);
            }

        }

        //**********************


        // remove the last delimiter
        if (result.Length > 0 && result[result.Length - 1] == DELIMITER)
        {
            result.Remove(result.Length - 1, 1);
        }


        return result.ToString();
    }
}

public enum Bones
{
    Hip_Center, Spine, Shoulder_Center, Head,  // 0 - 3
    Shoulder_Left, Elbow_Left, Wrist_Left, Hand_Left,  // 4 - 7
    Shoulder_Right, Elbow_Right, Wrist_Right, Hand_Right,  // 8 - 11
    Hip_Left, Knee_Left, Ankle_Left, Foot_Left,  // 12 - 15
    Hip_Right, Knee_Right, Ankle_Right, Foot_Right  // 16 - 19
}