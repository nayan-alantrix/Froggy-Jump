using UnityEngine;
using System.Collections.Generic;

public class LeafSpawner : MonoBehaviour
{
    [Header("UI Prefab")]
    [SerializeField] private LeafMove tilePrefab;   // UI tile you want to spawn

    [Header("Spawn Points (3 Positions)")]
    [SerializeField] private RectTransform[] spawnPoints; // Assign 3 UI empty objects
    [SerializeField] private RectTransform[] initSpanPoint;

    [Header("Parent Panel")]
    [SerializeField] private RectTransform parentPanel;   // Where tiles will be placed

    [Header("Spawn Settings")]
    [SerializeField] private float baseSpawnDelay = 1f;      // Initial spawn delay
    [SerializeField] private float minSpawnDelay = 0.3f;     // Minimum spawn delay

    [Header("Speed Settings")]
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float speedIncreaseRate = 0.1f; // Speed increase per second
    [SerializeField] private float maxSpeed = 8f;

    private List<LeafMove> bubbles;
    [SerializeField] private bool isActive = false;
    private float nextSpawnTime = 0f;
    private float currentSpeed;
    private float currentSpawnDelay;
    private float gameTime;
    private GameManger gameManger;

    public void SetRefrences(GameManger gameManger)
    {
        this.gameManger = gameManger;
    }

    private void Start()
    {
        bubbles = new List<LeafMove>();
        if (spawnPoints.Length != 3)
        {
            Debug.LogError("SpawnPoints must contain exactly 3 points!");
            return;
        }
        Reset();
    }

    private void Update()
    {
        if (!isActive) return;

        // Increase speed over time
        gameTime += Time.deltaTime;
        currentSpeed = Mathf.Min(baseSpeed + (speedIncreaseRate * gameTime), maxSpeed);

        // Calculate spawn delay based on speed (inversely proportional)
        // As speed increases from baseSpeed to maxSpeed, spawn delay decreases from baseSpawnDelay to minSpawnDelay
        float speedRatio = (currentSpeed - baseSpeed) / (maxSpeed - baseSpeed);
        currentSpawnDelay = Mathf.Lerp(baseSpawnDelay, minSpawnDelay, speedRatio);

        // Update all existing bubbles to current speed
        foreach (LeafMove move in bubbles)
        {
            move.SetSpeed(currentSpeed);
        }

        nextSpawnTime -= Time.deltaTime;
        // Check if it's time to spawn
        if (nextSpawnTime <= 0)
        {
            SpawnSingleBubble();
            nextSpawnTime = currentSpawnDelay;
        }
    }

    private void SpawnSingleBubble()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        RectTransform spawnPoint = spawnPoints[randomIndex];

        LeafMove tile = Instantiate<LeafMove>(tilePrefab, parentPanel);
        bubbles.Add(tile);
        tile.SetRefrence(this);
        tile.SetSpeed(currentSpeed);
        tile.gameObject.transform.localPosition = spawnPoint.localPosition;
        tile.SetMovementState(true);
    }

    public void StartGame()
    {
        Reset();
        gameTime = 0f;
        currentSpeed = baseSpeed;
        currentSpawnDelay = baseSpawnDelay;
        GameResume();
    }

    public void Reset()
    {
        foreach (LeafMove move in bubbles)
        {
            Destroy(move.gameObject);
        }
        bubbles.Clear();
        for (int i = 0; i < initSpanPoint.Length; i++)
        {
            LeafMove b = Instantiate<LeafMove>(tilePrefab, parentPanel);
            bubbles.Add(b);
            b.SetRefrence(this);
            b.SetSpeed(baseSpeed);
            b.gameObject.transform.localPosition = initSpanPoint[i].localPosition;
            if (i == 0)
            {
                gameManger.GetPlayerTransform().SetParent(b.gameObject.transform);
                gameManger.GetPlayerTransform().localPosition = Vector3.zero;
            }
        }
        nextSpawnTime = 0;
        gameTime = 0f;
        currentSpeed = baseSpeed;
        currentSpawnDelay = baseSpawnDelay;
    }

    public void GameResume()
    {
        isActive = true;
        foreach (LeafMove move in bubbles)
        {
            move.SetSpeed(currentSpeed);
            move.SetMovementState(true);
        }
    }

    public void GamePause()
    {
        isActive = false;
        foreach (LeafMove move in bubbles)
        {
            move.SetMovementState(false);
        }
    }

    public void RemoveBubble(LeafMove bubbleMove)
    {
        bubbles.Remove(bubbleMove);
        Destroy(bubbleMove.gameObject);
    }
}