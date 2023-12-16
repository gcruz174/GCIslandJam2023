using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public Health playerHealth;
        public DestroyPercentage destroyPercentage;
        public RawImage healthBar;
        public RawImage shieldBar;

        private void OnEnable()
        {
            playerHealth.OnDeath += OnDeath;
        }
        
        private void OnDisable()
        {
            playerHealth.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            SceneManager.LoadScene("GameOver");
        }

        private void Update()
        {
            var healthAmount = playerHealth.currentHealth / playerHealth.maxHealth;
            
            // Set height of health bar image to 101 if health is 100 and 0 if health is 0
            healthBar.rectTransform.anchoredPosition = new Vector2(-50f, 100f * healthAmount - 50f);
            
            // var percentage = destroyPercentage.
            // shieldBar.fillAmount = playerHealth.currentShield / playerHealth.maxShield;
        }
    }
}
