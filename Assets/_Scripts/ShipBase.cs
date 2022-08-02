using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ShipBase : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp = 0;
    public string curDir = "down";
    List<Tile> currentlyOccupiedTiles = new List<Tile>();
    List<Tile> nearOccupiedTiles = new List<Tile>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PlaceShip(float x, float y, int shipLength, string dir, bool player)
    {

        gameObject.transform.DOScale(1f, 0.5f);
        List<Tile> futureOccupiedTiles = new List<Tile>();
        RaycastHit2D tileRaycastResult = new RaycastHit2D();
        RaycastHit2D[] results = Physics2D.RaycastAll(new Vector3(x, y, 0), Vector2.zero);
        foreach (var result in results)
        {
            if (result.collider.gameObject.TryGetComponent(out Tile sussy))
            {
                tileRaycastResult = result;
            }
        }
        if (tileRaycastResult && tileRaycastResult.collider.gameObject.TryGetComponent(out Tile raycastedTile))    
        {
            Debug.Log(raycastedTile.gameObject.name);
            int tilex = int.Parse(raycastedTile.gameObject.name.Replace($"Tile ", "")[0].ToString());
            int tiley = int.Parse(raycastedTile.gameObject.name.Replace($"Tile ", "")[1].ToString());
                
            if(CanPlaceShip(tilex,tiley,shipLength, dir,player))
            {
                for (int i = 0; i < nearOccupiedTiles.Count; i++)
                {
                    if(nearOccupiedTiles[i].nearShips[0] == this) nearOccupiedTiles[i].nearShips[0] = null;
                    else if (nearOccupiedTiles[i].nearShips[1] == this)nearOccupiedTiles[i].nearShips[1] = null;
                }
                nearOccupiedTiles.Clear();
                //Debug.Log()
                CheckTilesAsOccupied(tilex, tiley, shipLength, dir, player, ref  futureOccupiedTiles);
                MarkShotsAround(futureOccupiedTiles, ref nearOccupiedTiles);
                gameObject.transform.position = raycastedTile.transform.position;

                if (dir == "right")
                {
                    gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -90);
                    //AddOffset(false);

                }
                else if (dir == "up")
                {
                    gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
                    //AddOffset(false);
                }
                else if (dir == "down")
                {
                    gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -180);
                    //AddOffset(false);
                }
                else if (dir == "left")
                {
                    gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 90);
                    //AddOffset(false);
                }
                curDir = dir;

                for (int i = 0; i < currentlyOccupiedTiles.Count; i++)
                {
                    if (currentlyOccupiedTiles[i] != raycastedTile) currentlyOccupiedTiles[i].playerShip = null;
                }

                currentlyOccupiedTiles = futureOccupiedTiles;
                //Debug.LogWarning("Full Success of " + gameObject.name);
            }
            //Debug.Log($"Tile {tilex} {tiley}");

        }
        else
        {
            Debug.LogWarning(gameObject.name + " ship placement failed. no starter tile found. result - " + tileRaycastResult.collider.gameObject.name);
            return;
        }
    }

    bool CanPlaceShip(int x, int y, int shipLength, string dir, bool player)
    {   
        for (int i = 0; i < shipLength; i++)
        {
            switch(dir)
            {
                case "left":
                    if (x-i >= 10 || x-i < 0 || IsTileOccupied(TileManager.Instance.GetTile(x-i,y), player))
                    {
                        return false;
                    }
                    break;
                case "right":
                    if (x + i >= 10 || x + i < 0 || IsTileOccupied(TileManager.Instance.GetTile(x + i, y), player))
                    {
                        return false;
                    }
                    break;

                case "up":
                    if (y + i >= 10 || y + i < 0 || IsTileOccupied(TileManager.Instance.GetTile(x, y + i), player))
                    {
                        return false;
                    }
                    break;
                case "down":
                    if (y-i >= 10 || y - i < 0 || IsTileOccupied(TileManager.Instance.GetTile(x, y - i), player))
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
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
    void CheckTilesAsOccupied(int x, int y, int shipLength, string dir, bool player, ref List<Tile> tilesArray)
    {   
        for (int i = 0; i < shipLength; i++)
        {
            switch (dir)
            {
                case "up":
                    if (player) TileManager.Instance.GetTile(x, y + i).playerShip = this;
                    else TileManager.Instance.GetTile(x, y + i).aiShip = this;
                    tilesArray.Add(TileManager.Instance.GetTile(x, y + i));
                    break;
                case "down":
                    if (player) TileManager.Instance.GetTile(x, y - i).playerShip = this;
                    else TileManager.Instance.GetTile(x, y - i).aiShip = this;
                    tilesArray.Add(TileManager.Instance.GetTile(x, y - i));
                    break;
                case "right":
                    if (player) TileManager.Instance.GetTile(x + i, y).playerShip = this;
                    else TileManager.Instance.GetTile(x + i, y).aiShip = this;
                    tilesArray.Add(TileManager.Instance.GetTile(x + i, y));
                    break;
                case "left":
                    if (player) TileManager.Instance.GetTile(x - i, y).playerShip = this;
                    else TileManager.Instance.GetTile(x-i, y).aiShip = this;
                    tilesArray.Add(TileManager.Instance.GetTile(x - i, y));
                    break;
            }
        }
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
                        if (! listCells.Contains(TileManager.Instance.GetTile(x + i, y + j)) && !tiles.Contains(TileManager.Instance.GetTile(x + i, y + j)))
                        {
                            tiles.Add(TileManager.Instance.GetTile(x + i, y + j));
                            if (TileManager.Instance.GetTile(x + i, y + j).nearShips[0] != null)
                            {
                                TileManager.Instance.GetTile(x + i, y + j).nearShips[1] = this;
                            }
                            else TileManager.Instance.GetTile(x + i, y + j).nearShips[0] = this;
                        }

                        
                        

                    }
                }
            }
        }
    }
}
