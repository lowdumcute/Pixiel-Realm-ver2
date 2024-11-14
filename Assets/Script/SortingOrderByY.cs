using UnityEngine;

public class SortingOrderByY : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Sắp xếp thứ tự dựa trên vị trí y, nhân với -100 để các đối tượng thấp hơn (y nhỏ hơn) sẽ có thứ tự cao hơn
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
