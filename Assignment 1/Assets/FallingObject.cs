using UnityEngine;

public class FallingObject : MonoBehaviour
{
    float fallSpeed;
    float destroyY = -5f;

    // Range of random speeds
    float minSpeed = 2f;
    float maxSpeed = 8f;

    void Start()
    {
        // Pick a random speed for this object
        fallSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}