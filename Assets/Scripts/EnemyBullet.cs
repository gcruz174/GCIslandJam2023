using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 10f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
