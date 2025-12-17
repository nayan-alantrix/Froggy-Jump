using UnityEngine;

public class GameManger : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private LeafSpawner bubbleSpawner;
    [SerializeField] private PlayerController playerController;
    [Header("Managers")]
    private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioManager audioManager;

    private void Awake()
    {
        levelManager = new LevelManager();

        levelManager.SetRefrences(this, bubbleSpawner, playerController);
        uiManager.SetRefrences(this);
    }
    public void Reset()
    {
        levelManager.Reset();
    }

    public void OnGameStart()
    {
        levelManager.OnGameStart();
    }

    public void OnGamePause()
    {
        levelManager.OnGamePause();
    }

    public void OnGameResume()
    {
        levelManager.OnGameResume();
    }

    public void OnGameOver()
    {
        levelManager.OnGameOver();
        uiManager.OnGameOver();
    }

    public Transform GetPlayerTransform()
    {
        return playerController.transform;
    }

    public AudioManager GetAudioManager()=> audioManager;

}