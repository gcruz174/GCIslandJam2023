using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        public DestroyPercentage destroyPercentage;
        public Health bossHealth;
        
        [Header("Colors")]
        public Color normalColor;
        public Color damagedColor;

        private Image _image;
        private float _targetFillAmount;

        public enum State
        {
            LayersHealth,
            BossHealth
        }
        
        public State state;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            destroyPercentage.OnLayerDestroyed += OnLayerDestroyed;
        }
        
        private void OnDestroy()
        {
            destroyPercentage.OnLayerDestroyed -= OnLayerDestroyed;
        }
        
        private void OnLayerDestroyed(int layerNumber)
        {
            if (layerNumber == 2) state = State.BossHealth;
            StartCoroutine(TweenColor(damagedColor, normalColor, 0.35f));
        }
        
        private void Update()
        {
            switch (state)
            {
                case State.LayersHealth:
                    // Get current layer and fill accordingly. First layer is from 66% to 100%, second layer is from 33% to 66%, third layer is from 0% to 33%
                    var layer = destroyPercentage.currentLayer;
                    var percentage = 1f - destroyPercentage.percentage / 100f;
                    
                    // Layer 0: Map (0-1) to (0.66-1)
                    // Layer 1: Map (0-1) to (0.33-0.66)
                    // Layer 2: Map (0-1) to (0-0.33)
                    var min = layer == 0 ? 0.66f : layer == 1 ? 0.33f : 0f;
                    var max = layer == 0 ? 1f : layer == 1 ? 0.66f : 0.33f;
                    _image.fillAmount = Mathf.Lerp(min, max, percentage);
                    break;
                case State.BossHealth:
                    _image.fillAmount = bossHealth.currentHealth / bossHealth.maxHealth;
                    break;
            }
        }
        
        private IEnumerator TweenColor(Color from, Color to, float duration)
        {
            var startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                var t = (Time.time - startTime) / duration;
                t = EaseInOut(t);
                var color = Color.Lerp(from, to, t);
                _image.color = color;
                yield return null;
            }
        }
        
        private static float EaseInOut(float t)
        {
            return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
        }
    }
}
