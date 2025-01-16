using UnityEngine;
using TMPro;

public class ChildObjectChecker : MonoBehaviour
{
    public GameObject referenceObject; // Object tham chiếu
    public GameObject redDotObject;    // Object chấm đỏ
    public TextMeshProUGUI redDotText; // Component TextMeshPro trong chấm đỏ

    private int previousChildCount; // Lưu lại số lượng object con trước đó

    void Start()
    {
        // Cập nhật khi game bắt đầu
        if (referenceObject != null)
        {
            previousChildCount = referenceObject.transform.childCount;
            CheckAndUpdateChildCount(previousChildCount); // Cập nhật ngay tại Start()
        }
    }

    void Update()
    {
        // Kiểm tra mỗi khung hình nếu có sự thay đổi trong số lượng object con trực tiếp
        if (referenceObject != null)
        {
            int currentChildCount = referenceObject.transform.childCount;

            // Nếu số lượng object con trực tiếp thay đổi
            if (currentChildCount != previousChildCount)
            {
                CheckAndUpdateChildCount(currentChildCount);
                previousChildCount = currentChildCount;
            }
        }
    }

    void CheckAndUpdateChildCount(int childCount)
    {
        // Kiểm tra nếu có object con
        if (childCount > 0)
        {
            // Bật chấm đỏ và cập nhật text
            redDotObject.SetActive(true);
            if (redDotText != null)
            {
                redDotText.text = childCount.ToString(); // Cập nhật số lượng object con
            }
        }
        else
        {
            // Tắt chấm đỏ nếu không có object con
            redDotObject.SetActive(false);
        }
    }
}
