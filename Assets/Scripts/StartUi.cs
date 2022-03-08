using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUi : MonoBehaviour
{
    public static StartUi Instance {get; protected set;}

    [SerializeField] private DOTweenAnimation playButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        KeyBoard();
    }
    
    private void KeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.R)) Restart();
        if (Input.GetKeyDown(KeyCode.P)) Play();
    }

    #region Button
    public void Play()
    {
        var level = PlayerPrefs.GetInt("Level", 1);
        if (SceneManager.sceneCountInBuildSettings <= level)
            level = Random.Range(1, SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene(level);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    public void ShowPlayButton()
    {
        playButton.DOPlay();
    }
}
