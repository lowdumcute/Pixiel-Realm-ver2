using System.Collections.Generic;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    private List<Vector3> initialPositions = new List<Vector3>(); // Lưu vị trí ban đầu của các object con
    private List<Transform> childUnits = new List<Transform>();   // Danh sách các object con

    private void Start()
    {
        // Lưu lại vị trí ban đầu của tất cả object con trong trại lính
        foreach (Transform child in transform)
        {
            initialPositions.Add(child.position);
            childUnits.Add(child);
        }
    }

    // Hàm này sẽ được gọi khi OnEnemyDefeated() của SpawnManager được kích hoạt
    public void ResetUnitsPositions()
    {
        for (int i = 0; i < childUnits.Count; i++)
        {
            if (childUnits[i] != null)
            {
                // Ẩn object con và tắt flipX trước khi di chuyển về vị trí ban đầu
                childUnits[i].gameObject.SetActive(false);
                SpriteRenderer spriteRenderer = childUnits[i].GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = false;
                }

                // Đặt lại vị trí ban đầu
                childUnits[i].position = initialPositions[i];

                // Hiển thị lại object và gọi Initialize
                childUnits[i].gameObject.SetActive(true);

                WarriorHealth warriorHealth = childUnits[i].GetComponent<WarriorHealth>();
                if (warriorHealth != null)
                {
                    warriorHealth.Initialize(); // Gọi Initialize thay vì Start
                }
            }
        }
    }
}
