using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DestroyPercentage : MonoBehaviour
{
    public Transform[] collidableLayers;
    public int currentLayer = 0;
    public float percentage = 0f;

    public TMP_Text percentageText;
    
    private float _initialArea = -1f;
    
    public event Action<int> OnLayerDestroyed;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        _initialArea = CalculateBoxCollidersArea(collidableLayers[0].GetComponentsInChildren<BoxCollider2D>());
    }
    
    private void Update()
    {
        if (_initialArea < 0f || currentLayer >= collidableLayers.Length) return;
        var currentArea = CalculateBoxCollidersArea(collidableLayers[currentLayer].GetComponentsInChildren<BoxCollider2D>());
        percentage = 100f - currentArea / _initialArea * 100;
        percentageText.text = $"{percentage:0}%";
        
        // Hotkey for testing layers
        if (Input.GetKeyDown(KeyCode.Q)) NextLayer();
        if (percentage > 99f) NextLayer();
    }
    
    private void NextLayer()
    {
        Destroy(collidableLayers[currentLayer].gameObject);
        OnLayerDestroyed?.Invoke(currentLayer);
        currentLayer++;
        
        if (currentLayer >= collidableLayers.Length)
        {
            percentageText.text = "100%";
            return;
        }
        
        _initialArea = CalculateBoxCollidersArea(collidableLayers[currentLayer].GetComponentsInChildren<BoxCollider2D>());
        collidableLayers[currentLayer].tag = "Untagged";
    }
    
    private static float CalculateBoxCollidersArea(IEnumerable<BoxCollider2D> boxColliders)
    {
        return boxColliders.Sum(boxCollider => boxCollider.size.x * boxCollider.size.y);
    }
}
