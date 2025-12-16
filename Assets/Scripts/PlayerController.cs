using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private float arcHeight = 50f;  // Height of jump arc
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("References")]
    [SerializeField] private RectTransform playerTransform;
    [SerializeField] private Canvas canvas;

    private bool isMoving = false;
    private bool isActive = false;
    private RectTransform currentBubble;
    private Transform originalParent;  // Store canvas parent for movement
    private GameManger gameManger;
    public void SetRefrences(GameManger gameManger)
    {
        this.gameManger = gameManger;
    }

    private void Start()
    {
        if (playerTransform == null)
            playerTransform = GetComponent<RectTransform>();
        
        originalParent = canvas.transform;
    }

    private void Update()
    {
        if (!isActive) return;
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            TryMoveToBubble();
        }
    }

    public void Reset()
    {
        isActive = true;
    }

    void TryMoveToBubble()
    {
        RectTransform clickedBubble = GetClickedBubble();

        if (clickedBubble != null && clickedBubble != currentBubble)
        {
            StartCoroutine(MoveToTarget(clickedBubble));
        }
    }

    RectTransform GetClickedBubble()
    {
        UnityEngine.EventSystems.PointerEventData pointerData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        pointerData.position = Input.mousePosition;

        System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult> results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointerData, results);

        foreach (UnityEngine.EventSystems.RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Bubble"))
            {
                return result.gameObject.GetComponent<RectTransform>();
            }
        }

        return null;
    }

    IEnumerator MoveToTarget(RectTransform targetBubble)
    {
        isMoving = true;

        // Get world positions BEFORE changing parent
        Vector3 startWorldPos = playerTransform.position;
        Vector3 endWorldPos = targetBubble.position;

        // Temporarily detach from parent to move in world space
        playerTransform.SetParent(originalParent, true);

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float curveValue = moveCurve.Evaluate(t);

            // Linear interpolation for horizontal movement
            Vector3 currentPos = Vector3.Lerp(startWorldPos, endWorldPos, curveValue);

            // Add parabolic arc (jump effect)
            float arcProgress = Mathf.Sin(t * Mathf.PI); // Creates a smooth arc (0 -> 1 -> 0)
            currentPos.y += arcProgress * arcHeight;

            playerTransform.position = currentPos;

            yield return null;
        }

        // Snap to final position
        playerTransform.position = endWorldPos;

        // Now attach to target bubble
        currentBubble = targetBubble;
        playerTransform.SetParent(targetBubble, true);
        playerTransform.localPosition = Vector3.zero;

        isMoving = false;
    }

    public void SetCurrentBubble(RectTransform bubble)
    {
        currentBubble = bubble;
        playerTransform.SetParent(bubble);
        playerTransform.localPosition = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D other) {
       
        if(other.gameObject.CompareTag("Spikes"))
        {
            gameManger.OnGameOver();
            isActive = false;
            Debug.Log("Game Over");
        }
    }

    public RectTransform GetCurrentBubble()
    {
        return currentBubble;
    }

    public void GamePause()
    {
        isActive = false;
    }

    public void GameResume()
    {
        isActive = true;
    }

    public void GameOver()
    {
        isActive = false;
    }
}