using System.Collections;
using UnityEngine;

public class ShieldAnimation : MonoBehaviour
{
    public DestroyPercentage destroyPercentage;
    public GameObject[] shields;
    
    private void Awake()
    {
        destroyPercentage.OnLayerDestroyed += FadeLayer;
    }
    
    private void OnDestroy()
    {
        destroyPercentage.OnLayerDestroyed -= FadeLayer;
    }
    
    private void FadeLayer(int layerNumber)
    {
        if (layerNumber > 1) return;
        
        var spriteRenderer = shields[layerNumber].GetComponent<SpriteRenderer>();
        StartCoroutine(TweenAlpha(spriteRenderer, 1f, 0f, 0.5f));
        
        // Fade next layer (if exists)
        if (layerNumber + 1 >= shields.Length) return;
        var currentShield = shields[layerNumber + 1];
        spriteRenderer = currentShield.GetComponent<SpriteRenderer>();
        StartCoroutine(TweenAlpha(spriteRenderer, 0f, 1f, 1f));
        StartCoroutine(TweenScale(shields[layerNumber + 1].transform, currentShield.transform.localScale * 4f, currentShield.transform.localScale, 0.5f));
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
    
    private IEnumerator TweenScale(Transform transform, Vector3 from, Vector3 to, float duration)
    {
        var startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            var t = (Time.time - startTime) / duration;
            t = EaseInOut(t);
            transform.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }
    
    private static float EaseInOut(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }
}
