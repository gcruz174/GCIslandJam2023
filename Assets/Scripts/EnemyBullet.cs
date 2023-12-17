using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 10f;
    public LayerMask layerMask;
    
    public GameObject hitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (layerMask != (layerMask | (1 << other.gameObject.layer)))
            return;
        
        var health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
        
        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);
    }
}
