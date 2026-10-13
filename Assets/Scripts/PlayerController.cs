using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public float thrustForce = 1f;
    public float maxSpeed = 5f;

    public GameObject boosterFlame;

    Rigidbody2D rb;
    private float lastWallSound = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;
            transform.up = direction;

            rb.AddForce(direction * thrustForce);
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (Time.time - lastWallSound > 0.2f)
            {
                lastWallSound = Time.time;
                AudioManager.Instance.PlayWallHit();
            }
        }
        else if (collision.gameObject.GetComponent<Obstacle>() != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayDeath(transform.position);

            if (gameManager != null) gameManager.GameOver();
            Destroy(gameObject);
        }
    }
}