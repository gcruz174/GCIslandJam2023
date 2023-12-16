using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
