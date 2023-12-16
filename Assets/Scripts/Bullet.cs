using DTerrain;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask planetLayer;
    public int circleSize = 16;
    public int outlineSize = 4;

    private Shape _destroyCircle;
    private Shape _outlineCircle;
    
    public GameObject explosionPrefab;
    public bool collided = false;

    private void Awake()
    {
        _destroyCircle = Shape.GenerateShapeCircle(circleSize);
        _outlineCircle = Shape.GenerateShapeCircle(circleSize + outlineSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (planetLayer != (planetLayer | (1 << other.gameObject.layer))) return;
        
        if (collided) return;
        Destroy(gameObject);
        // Spawn explosion
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        print("HOLA");
        
        collided = true;
        if (other.transform.parent.CompareTag("Indestructible")) return;
        
        var primaryLayer = other.gameObject.GetComponentInParent<BasicPaintableLayer>();
        var hitPoint = transform.position - primaryLayer.transform.position;

        primaryLayer.Paint(new PaintingParameters() 
        { 
            Color = Color.clear, 
            Position = new Vector2Int((int)(hitPoint.x * primaryLayer.PPU) - circleSize, (int)(hitPoint.y * primaryLayer.PPU) - circleSize), 
            Shape = _destroyCircle, 
            PaintingMode=PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
