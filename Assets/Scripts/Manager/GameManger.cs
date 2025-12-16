using UnityEngine;

public class GameManger : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private BubbleSpawner bubbleSpawner;
    [SerializeField] private PlayerController playerController;
    [Header("Managers")]
    private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        levelManager = new LevelManager();

        levelManager.SetRefrences(this, bubbleSpawner, playerController);
        uiManager.SetRefrences(this);
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

}