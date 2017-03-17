using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //================================================================================

    /*[SerializeField]
    private Text scoreText;*/

    private IMenuState _currentState;

    //================================================================================

    public MenuScript MenuState { get; private set; }
    public PlayMenuScript PlayMenuState { get; private set; }
    public PlayUiScript PlayUiState { get; private set; }
    public RecordMenuScript RecordMenuState { get; private set; }
    public RecordUiScript RecordUiState { get; private set; }

    //================================================================================


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("UIManager already instantiated");
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        MenuState = this.transform.Find("MenuPanel").GetComponent<MenuScript>();
        PlayMenuState = this.transform.Find("PlayMenuPanel").GetComponent<PlayMenuScript>();
        PlayUiState = this.transform.Find("PlayUiPanel").GetComponent<PlayUiScript>();
        RecordMenuState = this.transform.Find("RecordMenuPanel").GetComponent<RecordMenuScript>();
        RecordUiState = this.transform.Find("RecordUiPanel").GetComponent<RecordUiScript>();

        SwitchState(MenuState);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //================================================================================

    public void SwitchState(IMenuState newState)
    {
        if(_currentState != null)
        {
            _currentState.OnExit();
        }

        _currentState = newState;
        _currentState.OnEnter();
    }
}
