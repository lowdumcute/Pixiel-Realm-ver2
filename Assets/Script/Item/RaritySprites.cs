using UnityEngine;

[CreateAssetMenu(fileName = "RaritySprites", menuName = "ScriptableObjects/RaritySprites", order = 1)]
public class RaritySprites : ScriptableObject
{
    [Header("Sprites for Rarity Levels")]
    public Sprite commonSprite;  // Sprite for Common rarity
    public Sprite rareSprite;    // Sprite for Rare rarity
    public Sprite legendSprite;  // Sprite for Legend rarity
    [Header("Sprites for Start")]
    public Sprite startFillSprite; // Sprite for Start Fill
    public Sprite startEmptySprite; // Sprite for Start Empty
}
