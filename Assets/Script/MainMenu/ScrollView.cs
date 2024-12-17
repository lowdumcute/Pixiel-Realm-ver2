using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    public GameObject scrollBar;
    private float scroll_pos = 0f;

    private float[] pos;

    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            Transform child = transform.GetChild(i);

            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                // Phóng to bảng được chọn
                child.localScale = Vector2.Lerp(child.localScale, new Vector2(1f, 1f), 0.1f);

                // Kích hoạt các nút trong bảng được chọn
               

                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        Transform otherChild = transform.GetChild(a);

                        // Thu nhỏ các bảng không được chọn
                        otherChild.localScale = Vector2.Lerp(otherChild.localScale, new Vector2(0.6f, 0.6f), 0.1f);

                        // Vô hiệu hóa các nút trong bảng không được chọn
                        
                    }
                }
            }
        }
    }

    // Hàm chọn 1 mảng 
    public void ScrollToPosition(int index)
    {
        if (index >= 0 && index < pos.Length)
        {
            scroll_pos = pos[index];          
            scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, scroll_pos, 0.1f);
        }
    }

}
