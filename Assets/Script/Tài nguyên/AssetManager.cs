using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public static AssetManager instance;

    [SerializeField] private Asset assetData; // Tham chiếu tới ScriptableObject Asset

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddResource(Item.ItemType type, int amount)
    {
        if (assetData != null)
        {
            assetData.AddToElement(type, amount);
        }
    }

    public void UseEnergy(int amount)
    {
        if (assetData != null)
        {
            assetData.UseEnergy(amount);
        }
    }

    public void ReplenishEnergy(int amount)
    {
        if (assetData != null)
        {
            assetData.ReplenishEnergy(amount);
        }
    }

    public Asset GetAssetData()
    {
        return assetData;
    }
}
