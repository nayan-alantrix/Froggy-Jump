using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private float arcHeight = 50f;  // Height of jump arc
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("References")]
    [SerializeField] private RectTransform playerTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas canvas;

    [SerializeField] private bool isMoving = false;
    [SerializeField]private bool isActive = false;
    private RectTransform currentBubble;
    public Transform originalParent {get; private set;} // Store canvas parent for movement
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
        if(!isActive)return;
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            TryMoveToBubble();
        }
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
    PointerEventData pointerData = new PointerEventData(EventSystem.current);
    pointerData.position = Input.mousePosition;
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerData, results);
    
    foreach (RaycastResult result in results)
    {
        GameObject hitObject = result.gameObject;
        
        // If we hit a bubble first, it's clickable
        if (hitObject.CompareTag("Bubble"))
        {
            return hitObject.GetComponent<RectTransform>();
        }
        
        // If we hit a blocking layer/UI element before the bubble
        // (adjust the tag/layer check based on your setup)
        if (hitObject.layer == LayerMask.NameToLayer("BlockingUI") || 
            hitObject.CompareTag("BlockingLayer"))
        {
            return null; // Bubble is blocked
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
        }
    }
    public RectTransform GetCurrentBubble()
    {
        return currentBubble;
    }
    public void Reset()
    {
        animator.SetBool("stuned", false);
    }

    public void OnGameStart()
    {
        isActive = true;
        isMoving = false;
        animator.SetBool("stuned", false);
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
        Debug.Log("Game Over");
        animator.SetBool("stuned", true);
    }
}