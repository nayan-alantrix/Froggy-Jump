public class LevelManager
{
    private LeafSpawner bubbleSpawner;
    private PlayerController playerController;
    private GameManger gameManger;
    public void SetRefrences(GameManger gameManger, LeafSpawner bubbleSpawner, PlayerController playerController)
    {
        this.gameManger = gameManger;
        this.bubbleSpawner = bubbleSpawner;
        this.playerController = playerController;
        //set refrences
        bubbleSpawner.SetRefrences(gameManger);
        playerController.SetRefrences(gameManger);
    }

    public void Reset()
    {
        playerController.transform.SetParent(playerController.originalParent);
        playerController.Reset();
        bubbleSpawner.Reset();
    }
    public void OnGameStart()
    {
        playerController.transform.SetParent(playerController.originalParent);
        bubbleSpawner.StartGame();
        playerController.OnGameStart();
    } 
    public void OnGamePause()
    {
        bubbleSpawner.GamePause();
        playerController.GamePause();
    }

    public void OnGameResume()
    {
        bubbleSpawner.GameResume();
        playerController.GameResume();        
    }

    public void OnGameOver()
    {
        bubbleSpawner.GamePause();
        playerController.GameOver();
    }


}