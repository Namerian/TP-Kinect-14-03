using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private static UIManager instance;
    [SerializeField]
    private Text scoreText;

    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Debug.Log("UIManager already instantiated");
            Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetScoreText(int _score)
    {
        scoreText.text = "Score : " + _score;
    }
}
