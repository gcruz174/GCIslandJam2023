using System;
using System.Collections;
using Effects;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletSpawner : MonoBehaviour
{
    public PoolObject bulletPrefab;
    public PoolObject bulletPrefabStrong;
    public DestroyPercentage destroyPercentage;
    
    // Configurations for different modes
    
    // Random
    [Header("Random Mode")]
    public float randomBulletSpeed;
    public float randomSpawnRate;
    
    // Spiral
    [Header("Homing Mode")]
    public float homingBulletSpeed;
    public float homingSpawnRate;
    
    // Cross
    [Header("Cross Mode")]
    public float crossBulletSpeed;
    public float crossRotationSpeed;
    public float crossSpawnRate;
    
    // Second cross
    [Header("Second Cross Mode")]
    public float secondCrossBulletSpeed;
    public float secondCrossRotationSpeed;
    public float secondCrossSpawnRate;
    
    private float _lastSpawnTimeRandom;
    private float _lastSpawnTimeSpiral;
    private float _lastSpawnTimeCross;
    private float _lastSpawnTimeSecondCross;
    
    private float _currentAngle;
    private float _currentAngleSecondCross;
    private Transform _player;
    private bool _strongBullets;

    [Flags]
    public enum SpawnMode
    {
        None = 0,
        Cross = 1,
        Random = 2,
        Homing = 4,
        SecondCross = 8
    }

    public SpawnMode spawnMode = SpawnMode.Cross | SpawnMode.Random;
    
    private void Awake()
    {
        destroyPercentage.OnLayerDestroyed += OnLayerDestroyed;
        _player = GameObject.FindWithTag("Player").transform;
    }
    
    private void OnDestroy()
    {
        destroyPercentage.OnLayerDestroyed -= OnLayerDestroyed;
    }
    
    private void OnLayerDestroyed(int layerNumber)
    {
        if (layerNumber == 0)
        {
            spawnMode |= SpawnMode.Cross;
            homingSpawnRate = 0.75f;
        }
        
        if (layerNumber == 1)
        {
            _strongBullets = true;
        }
        
        if (layerNumber == 2)
        {
            // Remove cross mode
            spawnMode &= ~SpawnMode.Cross;
            // Add second cross mode
            spawnMode |= SpawnMode.SecondCross;
        }
    }
    
    private void Update()
    {
        HandleBulletSpawnMode();
    }
    
    private void HandleBulletSpawnMode()
    {
        if (spawnMode.HasFlag(SpawnMode.Cross)) HandleCrossMode();
        if (spawnMode.HasFlag(SpawnMode.Random)) HandleRandomMode();
        if (spawnMode.HasFlag(SpawnMode.Homing)) HandleHomingMode();
        if (spawnMode.HasFlag(SpawnMode.SecondCross)) HandleSecondCrossMode();
    }

    private void HandleCrossMode()
    {
        if (Time.time - _lastSpawnTimeCross < crossSpawnRate) return;
        _lastSpawnTimeCross = Time.time;
                
        _currentAngle += crossRotationSpeed * Time.deltaTime;
        for (var i = 0; i < 4; i++)
        {
            var velocity = new Vector2(Mathf.Cos(_currentAngle + i * Mathf.PI / 2f), Mathf.Sin(_currentAngle + i * Mathf.PI / 2f));
            ShootBullet(velocity, crossBulletSpeed);
        }
    }
    
    private void HandleSecondCrossMode()
    {
        if (Time.time - _lastSpawnTimeSecondCross < crossSpawnRate) return;
        _lastSpawnTimeSecondCross = Time.time;
                
        _currentAngleSecondCross += secondCrossRotationSpeed * Time.deltaTime;
        for (var i = 0; i < 8; i++)
        {
            var velocity = new Vector2(Mathf.Cos(_currentAngleSecondCross + i * Mathf.PI / 4f), Mathf.Sin(_currentAngleSecondCross + i * Mathf.PI / 2f));
            ShootBullet(velocity, secondCrossBulletSpeed);
        }
    }
    
    private void HandleRandomMode()
    {
        if (Time.time - _lastSpawnTimeRandom < randomSpawnRate) return;
        _lastSpawnTimeRandom = Time.time;
                
        var velocity = Random.insideUnitCircle.normalized;
        ShootBullet(velocity, randomBulletSpeed);
    }
    
    private void HandleHomingMode()
    {
        if (Time.time - _lastSpawnTimeSpiral < homingSpawnRate) return;
        _lastSpawnTimeSpiral = Time.time;
        StartCoroutine(SpawnHomingBullets());
    }
    
    private IEnumerator SpawnHomingBullets()
    {
        for (int i = 0; i < 3; i++)
        {
            var velocity = (_player.position - transform.position).normalized;
            ShootBullet(velocity, homingBulletSpeed);
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    private void ShootBullet(Vector2 direction, float speed)
    {
        var bulletPool = _strongBullets ? bulletPrefabStrong : bulletPrefab;
        var bullet = bulletPool.GetObject(true, null);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
        bullet.GetComponent<TrailRenderer>().Clear();
        StartCoroutine(bulletPool.ReturnWithDelay(bullet, 7.5f));
    }
}
