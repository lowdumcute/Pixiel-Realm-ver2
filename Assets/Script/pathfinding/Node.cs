using UnityEngine;

public class Node
{
    public Vector3 worldPosition;
    public bool isWalkable;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public int fCost { get { return gCost + hCost; } }

    public Node(Vector3 _worldPosition, bool _isWalkable, int _gridX, int _gridY)
    {
        worldPosition = _worldPosition;
        isWalkable = _isWalkable;
        gridX = _gridX;
        gridY = _gridY;
    }
}
