using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{

    float horizontal_input = 0f;
    float moveSpeed = 5f;

    float xLimit = 8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.leftArrowKey.isPressed ||
        Keyboard.current.aKey.isPressed)
        {
            horizontal_input = -1f;
        }
        else if(Keyboard.current.rightArrowKey.isPressed 
        || Keyboard.current.dKey.isPressed )
        {
            horizontal_input = 1f;
        }

        transform.position += Vector3.right * horizontal_input * moveSpeed * Time.deltaTime;
        float clampedX = Mathf.Clamp(transform.position.x,-xLimit,xLimit);
        transform.position = new Vector3(clampedX,transform.position.y,transform.position.z);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FindFirstObjectByType<GameManager>().GameOver();
    }
}
