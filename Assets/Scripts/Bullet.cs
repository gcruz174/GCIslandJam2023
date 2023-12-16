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
    
    private BasicPaintableLayer primaryLayer;
    private BasicPaintableLayer secondaryLayer;

    private void Awake()
    {
        primaryLayer = GameObject.Find("CollidableLayer").GetComponent<BasicPaintableLayer>();
        secondaryLayer = GameObject.Find("OutlineLayer").GetComponent<BasicPaintableLayer>();
        
        destroyCircle = Shape.GenerateShapeCircle(circleSize);
        outlineCircle = Shape.GenerateShapeCircle(circleSize + outlineSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (planetLayer != (planetLayer | (1 << other.gameObject.layer))) return;
        
        Vector3 p = transform.position - primaryLayer.transform.position;

        primaryLayer?.Paint(new PaintingParameters() 
        { 
            Color = Color.clear, 
            Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - circleSize, (int)(p.y * primaryLayer.PPU) - circleSize), 
            Shape = destroyCircle, 
            PaintingMode=PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });

        secondaryLayer?.Paint(new PaintingParameters() 
        { 
            Color = new Color(0.0f,0.0f,0.0f,0.75f), 
            Position = new Vector2Int((int)(p.x * secondaryLayer.PPU) - circleSize-outlineSize, (int)(p.y * secondaryLayer.PPU) - circleSize-outlineSize), 
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
