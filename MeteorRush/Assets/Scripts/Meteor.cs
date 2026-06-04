using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float speed = 4f;

    // How much the meteor rotates per second 
    public float rotationSpeed = 120f;

    private Vector3 moveDirection;

    // Called by MeteorSpawner after instantiation so we know where the player is
    public void Initialize(Vector3 playerPosition)
    {
        moveDirection = (playerPosition - transform.position).normalized;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // Destroy if it travels far off-screen
        if (transform.position.y < -8f || Mathf.Abs(transform.position.x) > 10f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHitByMeteor();
            Destroy(gameObject);
        }

        // Bullets can also destroy meteors — gives the player a way to shoot them down
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
