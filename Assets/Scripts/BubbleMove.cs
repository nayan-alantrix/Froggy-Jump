using UnityEngine;

public class BubbleMove : MonoBehaviour
{
    [SerializeField] private float speed = 3f;      // Downward speed
    [SerializeField] private float destroyY = -10f; // Auto destroy when off screen
    [SerializeField] private RectTransform rectTransform;

    private bool isActive = false;

    void Update()
    {
        if (!isActive)return;
        // Move object downward in 2D
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Destroy if it goes below screen
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }

    public void SetMovementState(bool active)
    {
        isActive = active;
    }
}