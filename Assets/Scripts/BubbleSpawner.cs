using UnityEngine;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour
{
    [Header("UI Prefab")]
    [SerializeField] private BubbleMove tilePrefab;   // UI tile you want to spawn

    [Header("Spawn Points (3 Positions)")]
    [SerializeField] private RectTransform[] spawnPoints; // Assign 3 UI empty objects

    [Header("Parent Panel")]
    [SerializeField] private RectTransform parentPanel;   // Where tiles will be placed

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 1f;      // Time between spawns
    [SerializeField] private int maxBubbles = 10;        // Max number of bubbles to spawn (-1 for infinite)

    private List<BubbleMove> bubbles;
    private int bubblesSpawned = 0;
    [SerializeField]private bool isActive = false;
    private float nextSpawnTime = 0f;
    private GameManger gameManger;
    public void SetRefrences(GameManger gameManger)
    {
        this.gameManger = gameManger;
    }

    private void Start()
    {
        bubbles = new List<BubbleMove>();
        if (spawnPoints.Length != 3)
        {
            Debug.LogError("SpawnPoints must contain exactly 3 points!");
            return;
        }
    }

    private void Update()
    {
        if (!isActive) return;
        nextSpawnTime -= Time.deltaTime;

        // Check if it's time to spawn
        if ( nextSpawnTime <= 0)
        {
                SpawnSingleBubble();
                bubblesSpawned++;
                nextSpawnTime = spawnDelay;
        }
    }

    private void SpawnSingleBubble()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        RectTransform spawnPoint = spawnPoints[randomIndex];

        BubbleMove tile = Instantiate<BubbleMove>(tilePrefab, parentPanel);
        bubbles.Add(tile);
        tile.gameObject.transform.localPosition = spawnPoint.localPosition;
        tile.SetMovementState(true);
    }
    public void StartGame()
    {foreach (BubbleMove move in bubbles)
        {
            Destroy(move.gameObject);
        }
        bubbles.Clear();
        bubblesSpawned = 0;
        isActive = true;
        nextSpawnTime = 0;
    }
    public void GameResume()
    {
        isActive = true;
        foreach (BubbleMove move in bubbles)
        {
            move.SetMovementState(true);
        }
    }

    public void GamePause()
    {
        isActive = false;
        foreach (BubbleMove move in bubbles)
        {
            move.SetMovementState(false);
        }
    }
}