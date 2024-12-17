using UnityEngine;

public class ItemInfoReceiver : MonoBehaviour
{
    public void ReceiveItemInfo(GameObject itemInfoPrefab)
    {
        if (itemInfoPrefab == null)
        {
            Debug.LogError("Không nhận được prefab item info!");
            return;
        }

        // Xử lý item info (nếu cần thêm logic xử lý)
        Debug.Log("Đã nhận và xử lý item info: " + itemInfoPrefab.name);
    }
}