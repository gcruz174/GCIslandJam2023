using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBoss : MonoBehaviour
{
    public DestroyPercentage destroyPercentage;
    public AudioSource normalMusic;
    public AudioSource bossMusic;
    
    private CircleCollider2D _circleCollider2D;
    private SpriteRenderer _spriteRenderer;
    private Health _health;

    private bool _active;
    
    private void Awake()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _health = GetComponent<Health>();
        
        destroyPercentage.OnLayerDestroyed += OnLayerDestroyed;
        _health.OnDeath += OnDeath;
    }

    private void OnDestroy()
    {
        destroyPercentage.OnLayerDestroyed -= OnLayerDestroyed;
        _health.OnDeath -= OnDeath;
    }

    private void OnDeath()
    {
        SceneManager.LoadScene("Victory");
    }

    private void OnLayerDestroyed(int layerNumber)
    {
        if (_active || layerNumber < 2) return;
        _active = true;
        _circleCollider2D.enabled = true;
        StartCoroutine(TweenAlpha(0f, 1f, 1f));
        normalMusic.Stop();
        bossMusic.Play();
    }
    
    private IEnumerator TweenAlpha(float from, float to, float duration)
    {
        var startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            var t = (Time.time - startTime) / duration;
            t = EaseInOut(t);
            var alpha = Mathf.Lerp(from, to, t);
            var color = _spriteRenderer.color;
            color.a = alpha;
            _spriteRenderer.color = color;
            yield return null;
        }
    }

    private IEnumerator TweenPosition(Vector3 from, Vector3 to, float duration)
    {
        var startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            var t = EaseInOut((Time.time - startTime) / duration);
            transform.position = Vector3.Lerp(from, to, t);;
            yield return null;
        }
    }
    
    private static float EaseInOut(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }
}
