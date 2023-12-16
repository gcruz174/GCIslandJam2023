using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DestroyPercentage : MonoBehaviour
{
    public Transform collidableLayer;
    public TMP_Text percentageText;
    
    private float _initialArea = -1f;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        _initialArea = CalculateBoxCollidersArea(collidableLayer.GetComponentsInChildren<BoxCollider2D>()) * 1.1f;
    }
    
    private void Update()
    {
        if (_initialArea < 0f) return;
        var currentArea = CalculateBoxCollidersArea(collidableLayer.GetComponentsInChildren<BoxCollider2D>());
        var percentage = 100f - currentArea / _initialArea * 100;
        percentageText.text = $"{percentage:0}%";
    }
    
    private float CalculateBoxCollidersArea(IEnumerable<BoxCollider2D> boxColliders)
    {
        return boxColliders.Sum(boxCollider => boxCollider.size.x * boxCollider.size.y);
    }
}
