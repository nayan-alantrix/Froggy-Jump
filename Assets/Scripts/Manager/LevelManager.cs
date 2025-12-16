public class LevelManager
{
    private BubbleSpawner bubbleSpawner;
    private PlayerController playerController;
    private GameManger gameManger;
    public void SetRefrences(GameManger gameManger, BubbleSpawner bubbleSpawner, PlayerController playerController)
    {
        this.gameManger = gameManger;
        this.bubbleSpawner = bubbleSpawner;
        this.playerController = playerController;

        //set refrences
        bubbleSpawner.SetRefrences(gameManger);
        playerController.SetRefrences(gameManger);
    }

    public void OnGameStart()
    {
        bubbleSpawner.StartGame();
        playerController.Reset();
    } 
    public void OnGamePause()
    {
    }

    public void OnGameResume()
    {
        
    }

    public void OnGameOver()
    {
        bubbleSpawner.GamePause();
    }
}