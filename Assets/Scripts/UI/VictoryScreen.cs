using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class VictoryScreen : MonoBehaviour
    {
        public float duration;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(duration);
            SceneManager.LoadScene("MainTitle");
        }
    }
}
