using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DestroyPercentage : MonoBehaviour
{
    public Transform[] collidableLayers;
    public int currentLayer = 0;

    public TMP_Text percentageText;
    
    private float _initialArea = -1f;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        _initialArea = CalculateBoxCollidersArea(collidableLayers[0].GetComponentsInChildren<BoxCollider2D>()) * 1.1f;
    }
    
    private void Update()
    {
        if (_initialArea < 0f) return;
        var currentArea = CalculateBoxCollidersArea(collidableLayers[currentLayer].GetComponentsInChildren<BoxCollider2D>());
        var percentage = 100f - currentArea / _initialArea * 100;
        percentageText.text = $"{percentage:0}%";
        
        if (percentage > 99f)
        {
            Destroy(collidableLayers[currentLayer].gameObject);
            currentLayer++;
            
            // Remove indestructible tag from the next layer
            if (currentLayer < collidableLayers.Length)
            {
                collidableLayers[currentLayer].tag = "Untagged";
            }
        }
    }
    
    private float CalculateBoxCollidersArea(IEnumerable<BoxCollider2D> boxColliders)
    {
        return boxColliders.Sum(boxCollider => boxCollider.size.x * boxCollider.size.y);
    }
}
