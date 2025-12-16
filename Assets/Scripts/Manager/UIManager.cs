using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main Menu panel")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button volumeBtn;
    [SerializeField] private Button infoBtn;

    [Header("GamePlay Panel")]
    [SerializeField] private GameObject gamePlayPanel;
    [Header("Pause Menu Panel")]
    [SerializeField]private GameObject pauseMenuPanel;

    [SerializeField] private Button pauseMenu_restartBtn;
    [SerializeField] private Button pauseMenu_homeBtn;
    
    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button gameOver_restartBtn;
    [SerializeField] private Button gameOver_homeBtn;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    private int volume = 1;

    //player prefs key
    private readonly string volumeKey = "Volume";
    private GameManger gameManger;
    public void SetRefrences(GameManger gameManger)
    {
        this.gameManger = gameManger;
    }

    private void Awake()
    {
        DeactivateAllPanels();
        mainMenuPanel.SetActive(true);
        //default volume
        volume = PlayerPrefs.GetInt(volumeKey, 1);
        musicSource.volume = volume;
        sfxSource.volume = volume;
        //settingUp button listeners
        playBtn.onClick.AddListener(()=>{
            gameManger.OnGameStart();
            mainMenuPanel.SetActive(false);
            gamePlayPanel.SetActive(true);
        });

        volumeBtn.onClick.AddListener(() =>
        {
            musicSource.volume = volume;
            sfxSource.volume = volume;
            PlayerPrefs.SetInt(volumeKey, volume);
        });
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
    }
    private void DeactivateAllPanels()
    {
        mainMenuPanel.SetActive(false);
        infoPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
}