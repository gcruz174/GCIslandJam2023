using System.Collections;
using UnityEngine;

public class ShieldAnimation : MonoBehaviour
{
    public DestroyPercentage destroyPercentage;
    public GameObject[] shields;
    
    private int _lastLayer = 0;
    
    private void Update()
    {
        if (destroyPercentage.currentLayer > _lastLayer)
        {
            var spriteRenderer = shields[_lastLayer].GetComponent<SpriteRenderer>();
            StartCoroutine(TweenAlpha(spriteRenderer, 1f, 0f, 0.5f));
            
            _lastLayer = destroyPercentage.currentLayer;

            if (_lastLayer >= shields.Length) return;
            spriteRenderer = shields[_lastLayer].GetComponent<SpriteRenderer>();
            StartCoroutine(TweenAlpha(spriteRenderer, 0f, 1f, 0.5f));
        }
    }

    private IEnumerator TweenAlpha(SpriteRenderer spriteRenderer, float from, float to, float duration)
    {
        var startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            var t = (Time.time - startTime) / duration;
            t = EaseInOut(t);
            var alpha = Mathf.Lerp(from, to, t);
            foreach (var shield in shields)
            {
                var color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
            yield return null;
        }
    }
    
    private static float EaseInOut(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }
}
