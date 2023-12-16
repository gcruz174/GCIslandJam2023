using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRate;
    public float rotationSpeed;
    
    private float _lastSpawnTime;
    private float _currentAngle;
    
    private void Update()
    {
        if (Time.time - _lastSpawnTime < spawnRate) return;
        _lastSpawnTime = Time.time;
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        
        // Rotate the bullet
        _currentAngle += rotationSpeed * Time.deltaTime;
        var velocity = new Vector2(Mathf.Cos(_currentAngle), Mathf.Sin(_currentAngle));
        
        // Give the bullet a random velocity
        bullet.GetComponent<Rigidbody2D>().velocity = velocity * 10f;
    }
}
