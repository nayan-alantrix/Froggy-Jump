using System;
using TMPro;
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
    [SerializeField] private Button closeInfoPanelsBtn;
    [SerializeField] private Button quit;

    [Header("GamePlay Panel")]
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button gameplay_pauseBtn;
    [Header("Pause Menu Panel")]
    [SerializeField]private GameObject pauseMenuPanel;

    [SerializeField] private Button pauseMenu_restartBtn;
    [SerializeField] private Button pauseMenu_resumeBtn;
    [SerializeField] private Button pauseMenu_homeBtn;
    
    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button gameOver_restartBtn;
    [SerializeField] private Button gameOver_homeBtn;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private Sprite soundSprite;
    [SerializeField] private Sprite soundMuteSprite;

    private int volume = 1;
    //Score
    private int score = 0;

    //player prefs key
    private readonly string volumeKey = "Volume";
    private GameManger gameManger;
    public void SetRefrences(GameManger gameManger)
    {
        this.gameManger = gameManger;
    }

    private void Awake()
    {
        //default volume
        volume = PlayerPrefs.GetInt(volumeKey, 1);
        musicSource.volume = volume;
        sfxSource.volume = volume;
        if(volume == 1) volumeBtn.image.sprite = soundSprite;
        else volumeBtn.image.sprite = soundMuteSprite;
        //settingUp button listeners
        //main menu
        playBtn.onClick.AddListener(OnGameStart);
        volumeBtn.onClick.AddListener(() =>
        {
            if(volume == 0)
            {
                volume = 1;
                volumeBtn.image.sprite = soundSprite;
            }
            else
            {
                volume = 0;
                volumeBtn.image.sprite = soundMuteSprite;
            }
            musicSource.volume = volume;
            sfxSource.volume = volume;
            PlayerPrefs.SetInt(volumeKey, volume);
        });
        infoBtn.onClick.AddListener(()=>infoPanel.SetActive(true));
        closeInfoPanelsBtn.onClick.AddListener(() => infoPanel.SetActive(false));
        quit.onClick.AddListener(Application.Quit);
        //gameplay
        gameplay_pauseBtn.onClick.AddListener(OnGamePause);
        //pause menu
        pauseMenu_homeBtn.onClick.AddListener(OnHomeBtnClick);
        pauseMenu_restartBtn.onClick.AddListener(()=>{
            OnGameStart();
            pauseMenuPanel.SetActive(false);
        });
        pauseMenu_resumeBtn.onClick.AddListener(OnGameResume);
        
        //game over
        gameOver_homeBtn.onClick.AddListener(OnHomeBtnClick); //game over
        gameOver_restartBtn.onClick.AddListener(()=>{
            OnGameStart();
            gameOverPanel.SetActive(false);
        });

    }

    private void Start()
    {
        OnHomeBtnClick();
    }

    private void OnHomeBtnClick()
    {
        DeactivateAllPanels();
        mainMenuPanel.SetActive(true);
        gameManger.Reset();
    }

    private void OnGameStart()
    {        
        gameManger.OnGameStart();
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }

    private void OnGamePause()
    {
        gameManger.OnGamePause();
        pauseMenuPanel.SetActive(true);
    }

    public void OnGameResume()
    {
        gameManger.OnGameResume();
        pauseMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
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