using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    void OnMouseDown()
    {
        // Khi nhấn vào đối tượng, gọi OutlineManager để bật outline cho đối tượng này
        OutlineManager.Instance.SetOutline(gameObject);
    }
}
