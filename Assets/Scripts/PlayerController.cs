using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 1f;
    public float maxSpeed = 5f;

    public GameObject boosterFlame;
    [SerializeField] public GameObject ExplosionEffect;
    private float elpsedTime = 0f;
    public float ScoreMultiplier = 10.0f;
    private float score = 0f;

    [SerializeField] private PanelRenderer panel;
    [SerializeField] private VisualElementReference<Label> labelRefrence;
    private Label scoreText;
    private Label HighscoreText;
    private Button Restart;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            elpsedTime += Time.deltaTime;
            score = Mathf.FloorToInt(elpsedTime * ScoreMultiplier); // "Mathf.FloorToInt" just rounds
            if(scoreText != null)
            {
                scoreText.text = "Score: " + score.ToString();
            }
     
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
        Destroy(gameObject);
        //if(Collision.gameObject.CompareTag("Obstacle"))
        //{
        //    Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        //    Destroy(ExplosionEffect);
        //}

    }

    private void Awake()
    {
        panel.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        panel.UnregisterUIReloadCallback(OnUIReload);
    }

    private void OnDestroy()
    {
        labelRefrence.UnregisterReferenceUnloadedCallback(HandleLabelRefrenceCallBack);
    }

    private void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {
        labelRefrence.RegisterReferenceResolvedCallback(HandleLabelRefrenceCallBack);
    }

    private void HandleLabelRefrenceCallBack(Label label)
    {
        scoreText = label;
    }

    private void OnRestart()
    {

    }
}
