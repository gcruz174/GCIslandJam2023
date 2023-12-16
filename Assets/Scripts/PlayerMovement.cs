using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform spriteTransform;
    public float rotationSpeed = 100f;

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward, horizontalInput * rotationSpeed * Time.deltaTime);
        
        // if (horizontalInput != 0f)
        //     spriteTransform.localScale = new Vector3(horizontalInput < 0f ? -0.1f : 0.1f, 0.1f, 0.1f);
    }
}
