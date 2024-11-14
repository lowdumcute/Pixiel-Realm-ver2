using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public Material outlineMaterial;  // Material outline
    private GameObject currentOutlinedObject;

    public static OutlineManager Instance;

    void Awake()
    {
        // Đảm bảo chỉ có một OutlineManager tồn tại trong scene
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetOutline(GameObject obj)
    {
        // Tắt outline của đối tượng hiện tại nếu có
        if (currentOutlinedObject != null)
        {
            ToggleOutline(currentOutlinedObject, false);
        }

        // Bật outline cho đối tượng mới
        ToggleOutline(obj, true);
        currentOutlinedObject = obj;
    }

    private void ToggleOutline(GameObject obj, bool enabled)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Nếu bật outline, thêm material outline vào renderer; nếu tắt, loại bỏ nó
            List<Material> materials = new List<Material>(renderer.materials);
            if (enabled && !materials.Contains(outlineMaterial))
            {
                materials.Add(outlineMaterial);
            }
            else if (!enabled && materials.Contains(outlineMaterial))
            {
                materials.Remove(outlineMaterial);
            }
            renderer.materials = materials.ToArray();
        }
    }
}
