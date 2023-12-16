using System;
using DTerrain;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask planetLayer;
    public int circleSize = 16;
    public int outlineSize = 4;
    
    protected Shape destroyCircle;
    protected Shape outlineCircle;

    private void Awake()
    {
        destroyCircle = Shape.GenerateShapeCircle(circleSize);
        outlineCircle = Shape.GenerateShapeCircle(circleSize + outlineSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (planetLayer != (planetLayer | (1 << other.gameObject.layer))) return;
        
        var primaryLayer = other.gameObject.GetComponentInParent<BasicPaintableLayer>();
        var secondaryLayer = other.transform.parent.GetChild(0).GetComponent<BasicPaintableLayer>();
        var hitPoint = transform.position - primaryLayer.transform.position;

        primaryLayer?.Paint(new PaintingParameters() 
        { 
            Color = Color.clear, 
            Position = new Vector2Int((int)(hitPoint.x * primaryLayer.PPU) - circleSize, (int)(hitPoint.y * primaryLayer.PPU) - circleSize), 
            Shape = destroyCircle, 
            PaintingMode=PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });

        secondaryLayer?.Paint(new PaintingParameters() 
        { 
            Color = new Color(0.0f,0.0f,0.0f,0.75f), 
            Position = new Vector2Int((int)(hitPoint.x * secondaryLayer.PPU) - circleSize-outlineSize, (int)(hitPoint.y * secondaryLayer.PPU) - circleSize-outlineSize), 
            Shape = outlineCircle, 
            PaintingMode=PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.NONE
        });
            
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
