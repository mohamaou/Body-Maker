using System;
using UnityEngine;


[Serializable]
public class PlayerCamera
{
    [SerializeField] private GameObject playCamera, winCamera;


    public void SetCamera(bool win)
    {
        playCamera.SetActive(!win);
        winCamera.SetActive(win);
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; protected set;}
    public static GameState State;
    public static int Level;
    [SerializeField] private new PlayerCamera camera;
    [HideInInspector] public int enemiesCount;


    private void Awake()
    {
        Instance = this;
        State = GameState.Start;
        Level = PlayerPrefs.GetInt("Level", 1);
        enemiesCount = FindObjectsOfType<Enemy>().Length;
        camera.SetCamera(false);
        TinySauce.OnGameStarted(Level.ToString());
    }


    private void Play()
    {
        State = GameState.Play;
        UiManager.Instance.panels.SetPanel(State);
        foreach (var h in FindObjectsOfType<HealthBar>())
        {
            h.Show();
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && State == GameState.Start) Play();
        if(enemiesCount <= 0 && State == GameState.Play) EndGame(true);
        KeyBoard();
    }
    
    private void KeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.N)) EndGame(true);
    }

    public void EndGame(bool win)
    {
        State = win ? GameState.Win : GameState.Lose;
        TinySauce.OnGameFinished(win, 0, Level.ToString());
        camera.SetCamera(win);
        UiManager.Instance.panels.SetPanel(State);
        if (win)
        {
            Player.Instance.winEffect.Play();
            Player.Instance.Dance();
        }
        else
        {
            foreach (var enemies in FindObjectsOfType<Enemy>())
            {
                enemies.Dance();
            }
        }
        foreach (var h in FindObjectsOfType<HealthBar>())
        {
            h.Hide();
        }
    }
}
