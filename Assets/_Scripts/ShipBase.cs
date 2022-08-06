using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum ShipDirection
{
    Down,
    Right,
    Up,
    Left
}

public class ShipBase : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp = 0;
    public ShipDirection curentDirection = ShipDirection.Down;
    public List<Tile> currentlyOccupiedTiles = new List<Tile>();
    List<Tile> nearOccupiedTiles = new List<Tile>();
    public bool isPlaced = false;
    [SerializeField] AudioClip placeSound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    

    bool IsTileOccupied(Tile tile, bool player)
    {   if (player)
        {   
                //Debug.Log("hi");
                if (tile.playerShip != null && tile.playerShip != this || tile.nearShips[0] != null && tile.nearShips[0] != this || tile.nearShips[1] != null && tile.nearShips[1] != this)
                {
                    Debug.LogWarning(gameObject.name + $" ship placement failed. Player ship standing.");
                    return true;
                }

        }

        return false;
    }
    
    void MarkShotsAround(List<Tile> listCells, ref List<Tile> tiles)
    {
        foreach (var cell in listCells) // each cell beneath the new settled ship
        {
            int x = int.Parse(cell.gameObject.name.Replace($"Tile ", "")[0].ToString());
            int y = int.Parse(cell.gameObject.name.Replace($"Tile ", "")[1].ToString());
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((x + i >= 0) && (x + i <= 9) && (y + j >= 0) && (y + j <= 9))
                    {
                        if (!tiles.Contains(TileManager.Instance.GetTile(x + i, y + j)))
                        {
                            tiles.Add(TileManager.Instance.GetTile(x + i, y + j));
                            if (TileManager.Instance.GetTile(x + i, y + j).nearShips[0] != null)
                            {
                                TileManager.Instance.GetTile(x + i, y + j).nearShips[1] = this;
                            }
                            else TileManager.Instance.GetTile(x + i, y + j).nearShips[0] = this;
                            //!listCells.Contains(TileManager.Instance.GetTile(x + i, y + j))
                        }
                    }
                }
            }
        }
    }
}
