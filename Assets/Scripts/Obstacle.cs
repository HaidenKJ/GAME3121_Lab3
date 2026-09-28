using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    
    public float minSpeed = 50f;
    public float maxSpeed = 150f;

    public float maxSpinSpeed = 10f; 
    [SerializeField] public GameObject ExplosionEffect;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle; // within 360 degrees

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDirection * randomSpeed);

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {  
        if(collision.gameObject.CompareTag("Player"))
        {     
            GameObject ExplosionInstance = Instantiate(ExplosionEffect, collision.GetContact(0).point, Quaternion.identity);
            Destroy(ExplosionInstance, 2f);
        }
    }
}
