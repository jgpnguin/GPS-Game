using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapQuery : MonoBehaviour
{
    public Tilemap tilemap;
    public static TilemapQuery instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Two tilemap queries detected, one not set to instance");
        }
    }

    public bool HasTile(Vector3Int pos)
    {
        return tilemap.HasTile(pos);
    }

    /// <summary>
    /// Gets the cartesian distance 
    /// </summary>
    /// <param name="targ"></param>
    /// <param name="pos"></param>
    /// <returns></returns> 
    public int GetTileCostRem(Vector3Int targ, Vector3Int pos)
    {
        if (HasTile(pos))
        {
            return int.MaxValue;
        }
        return Mathf.Abs(targ.x - pos.x) + Mathf.Abs(targ.y - pos.y);
    }

    public Vector3Int WorldToCell(Vector3 pos)
    {
        return tilemap.layoutGrid.WorldToCell(pos);
    }
    public Vector2 CellToWorld(Vector3Int pos)
    {
        return tilemap.layoutGrid.CellToWorld(pos);
    }
}
