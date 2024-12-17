using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInfoManager : MonoBehaviour
{
    [SerializeField] private GameObject itemInfoPrefab; // Prefab item info
    [SerializeField] private ItemInfoReceiver itemInfoReceiver; // Object chứa script ItemInfoReceiver

    private GameObject currentItemInfo; // Lưu tham chiếu đến item info hiện tại

    private void Start()
    {
        // Tìm đối tượng có ItemInfoReceiver nếu chưa gán
        if (itemInfoReceiver == null)
        {
            itemInfoReceiver = FindObjectOfType<ItemInfoReceiver>();
        }

        if (itemInfoReceiver == null)
        {
            Debug.LogError("Không tìm thấy ItemInfoReceiver trong scene!");
        }
    }

    public void SpawnItemInfo()
    {
        if (itemInfoPrefab == null)
        {
            Debug.LogError("ItemInfoPrefab chưa được gán!");
            return;
        }

        // Xóa item info trước đó nếu tồn tại
        if (currentItemInfo != null)
        {
            Destroy(currentItemInfo);
        }

        // Spawn prefab làm con của ItemInfoReceiver
        currentItemInfo = Instantiate(itemInfoPrefab, itemInfoReceiver.transform);

    }

    private void Update()
    {
        // Kiểm tra xem người dùng có nhấn bất kỳ nút nào không
        if (Input.anyKeyDown)
        {
            ClearItemInfo();
        }
    }

    public void ClearItemInfo()
    {
        if (currentItemInfo != null)
        {
            Destroy(currentItemInfo);
            currentItemInfo = null;
        }
    }
}
