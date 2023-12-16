using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRate;
    public float modeDuration;
    
    public float bulletSpeed;
    public float rotationSpeed;
    
    private float _lastSpawnTime;
    private float _lastModeChangeTime;
    private float _currentAngle;

    public enum SpawnMode
    {
        Spiral,
        Fans,
        Random
    }

    public SpawnMode spawnMode = SpawnMode.Spiral;
    
    private void Update()
    {
        HandleBulletSpawnMode();
    }
    
    private void HandleBulletSpawnMode()
    {
        if (Time.time - _lastSpawnTime < spawnRate) return;
        _lastSpawnTime = Time.time;
        Vector2 velocity;
        
        switch (spawnMode)
        {
            case SpawnMode.Spiral:
                _currentAngle += rotationSpeed * Time.deltaTime;
                velocity = new Vector2(Mathf.Cos(_currentAngle), Mathf.Sin(_currentAngle));
                ShootBullet(velocity, bulletSpeed);
                break;
            case SpawnMode.Fans:
                
                break;
            case SpawnMode.Random:
                velocity = Random.insideUnitCircle.normalized;
                ShootBullet(velocity, bulletSpeed);
                break;
        }
        
        if (Time.time - _lastModeChangeTime < modeDuration) return;
        _lastModeChangeTime = Time.time;
        spawnMode = (SpawnMode) Random.Range(0, 3);
    }
    
    private void ShootBullet(Vector2 direction, float speed)
    {
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
