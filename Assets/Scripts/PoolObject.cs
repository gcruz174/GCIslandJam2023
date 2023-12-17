using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Effects
{
    [CreateAssetMenu(menuName = "Marble Smashers/Pool Object", order = 0)]
    public class PoolObject : ScriptableObject
    {
        [Tooltip("Spawn this amount of objects upon game start.")]
        public int defaultSpawnCount = 5;
        [Tooltip("Amount of objects to create in case the pool runs out.")]
        public int makeExtraCount = 3;

        [Tooltip("If this is enabled, object parent must be returned to use it.")]
        public bool returnToParentToReuse;

        [Tooltip("Prefab that this pool will instantiate")]
        public GameObject poolObject;

        [NonSerialized]
        private List<GameObject> _mPool = new();
        [NonSerialized]
        private Transform _mParent;

        public GameObject GetObject(bool active, Transform parent)
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                Debug.LogError("PoolObject should not be used when the game isn't running.");
#endif
                return null;
            }

            GameObject gameObject = null;
            foreach (var g in _mPool)
            {
                if (g == null)
                {
                    continue;
                }

                if (g.activeSelf) continue;
                if (returnToParentToReuse && g.transform.parent.GetInstanceID() != parent.GetInstanceID()) continue;

                gameObject = g;
                break;
            }

            if (ReferenceEquals(gameObject, null))
            {
                if (_mPool.Count == 0)
                {
                    Initialize(null);
                    gameObject = _mPool[0];
                }
                else
                {
                    PopulateGroup(makeExtraCount);
                    gameObject = _mPool[^makeExtraCount];
                }
            }

            if (parent) gameObject.transform.SetParent(parent);
            gameObject.SetActive(active);

            return gameObject;
        }

        public void ReturnAllObjects()
        {
            foreach (var t in _mPool)
            {
                ReturnObject(t, true);
            }
        }

        public void DestroyAllObjects()
        {
            for (var i = _mPool.Count - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                    Destroy(_mPool[i]);
                else
                    DestroyImmediate(_mPool[i]);
            }

            _mPool = new List<GameObject>();
        }

        public void ReturnObject(GameObject gameObject, bool returnToParent)
        {
            if (returnToParentToReuse) returnToParent = true;

            if (_mParent && returnToParent) gameObject.transform.SetParent(_mParent);
            gameObject.SetActive(false);
        }

        public void Initialize(Transform parent)
        {
            Assert.IsNotNull(poolObject, $"{name} is missing its poolObject!");
            _mParent = parent;
            PopulateGroup(defaultSpawnCount);
        }

        private void PopulateGroup(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var gameObject = Instantiate(poolObject);
                gameObject.name = gameObject.name + "_" + _mPool.Count;

                if (_mParent) gameObject.transform.SetParent(_mParent, false);
                gameObject.SetActive(false);
                _mPool.Add(gameObject);
            }
        }

        public IEnumerator ReturnWithDelay(GameObject gameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnObject(gameObject, true);
        }

        public IEnumerator DisableWithDelay(GameObject gameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }
    }
}