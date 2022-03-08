using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Start, Play, Lose, Win
}

[Serializable]
public class UIPanels
{
    [SerializeField] private GameObject startPanel, playPanel, winPanel, losePanel;
    public void SetPanel(GameState state = GameState.Start)
    {
        startPanel.SetActive(false);
        playPanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        switch (state)
        {
            case GameState.Start:
                startPanel.SetActive(true);
                break;
            case GameState.Play:
                playPanel.SetActive(true);
                break;
            case GameState.Lose:
                losePanel.SetActive(true);
                break;
            case GameState.Win:
                winPanel.SetActive(true);
                break;
        }
    }
}

public class UiManager : MonoBehaviour
{
    public static UiManager Instance {get; protected set;}
    public UIPanels panels;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private Image enemyLogo;


    void Start()
    {
        Instance = this;
        panels.SetPanel();
        level.text = "Level " + GameManager.Level;
        SetEnemiesLogo();
    }


    private void SetEnemiesLogo()
    {
        var enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].myLogo = Instantiate(enemyLogo, enemyLogo.transform.parent).transform.GetChild(0).GetComponent<Image>();
        }

        Destroy(enemyLogo.gameObject);
    }
    


    #region Button
    public void Play()
    {
        
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RemakeBody()
    {
        SceneManager.LoadScene(0);
    }
    public void Next()
    {
        var level = GameManager.Level + 1;
        PlayerPrefs.SetInt("Level",level);
        RemakeBody();
    }
    #endregion
}
