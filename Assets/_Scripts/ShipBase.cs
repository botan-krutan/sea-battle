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
    public float yOffset = 0;
    List<Tile> pastOccupiedTiles = new List<Tile>();
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
        List<Tile> occupiedTiles = new List<Tile>();
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
/*            if (!CheckIfOccupied(player, ref raycastedTile))
            {   
                Debug.Log("Hi");
                return;
            }*/
            Debug.Log(raycastedTile.gameObject.name);
            int tilex = int.Parse(raycastedTile.gameObject.name.Replace($"Tile ", "")[0].ToString());
            int tiley = int.Parse(raycastedTile.gameObject.name.Replace($"Tile ", "")[1].ToString());

            //Debug.Log($"Tile {tilex} {tiley}");
            for (int i = 0; i < shipLength; i++)
            {
                if(dir == "up")
                {
                   // Debug.Log($"Tile {tilex}{tiley + i}");
                    if (tiley + i < 10 && tiley + i >= 0)
                    {
                        Tile currentTile = TileManager.Instance.GetTile(tilex, tiley + i);
                        occupiedTiles.Add(currentTile);
                        if (!CheckIfOccupied(player, ref currentTile))
                        {
                            ClearTiles(occupiedTiles);
                            return;
                        }
                        
                    }
                    else
                    {
                        Debug.LogWarning(gameObject.name + $" ship placement failed. Tile {tilex}{tiley + i} not found.");
                        ClearTiles(occupiedTiles);
                        return;
                    }
                }
                else if (dir == "down")
                {
                    if (tiley - i < 10 && tiley - i >= 0)
                    {
                        Tile currentTile = TileManager.Instance.GetTile(tilex, tiley - i);
                        occupiedTiles.Add(currentTile);
                        if (!CheckIfOccupied(player, ref currentTile))
                        {
                            ClearTiles(occupiedTiles);
                            return;
                        }
                    }
                    else
                    {
                        Debug.LogWarning(gameObject.name + $" ship placement failed. Tile {tilex}{tiley + i} not found.");
                        ClearTiles(occupiedTiles);
                        return;
                    }
                }
                else if (dir == "right")
                {
                    if (tilex + i < 10 && tilex + i >= 0)
                    {   
                        Tile currentTile = TileManager.Instance.GetTile(tilex + i, tiley);
                        occupiedTiles.Add(currentTile);
                        if (!CheckIfOccupied(player, ref currentTile))
                        {
                            
                            ClearTiles(occupiedTiles);
                            return;
                        }
                        Debug.Log(currentTile.gameObject.name);
                    }
                    else
                    {
                        Debug.LogWarning(gameObject.name + $" ship placement failed.Tile {tilex}{tiley + i} not found.");
                        ClearTiles(occupiedTiles);
                        return;
                    }
                }
                else if (dir == "left")
                {
                    if (tilex-i < 10 && tilex - i >= 0)
                    {
                        Tile currentTile = TileManager.Instance.GetTile(tilex - i, tiley);
                        occupiedTiles.Add(currentTile);
                        if (!CheckIfOccupied(player, ref currentTile))
                        {
                            ClearTiles(occupiedTiles);
                            return;
                        }
                    }
                    else
                    {
                        Debug.LogWarning(gameObject.name + $" ship placement failed. Tile {tilex}{tiley + i} not found.");
                        ClearTiles(occupiedTiles);
                        return;
                    }
                }

            }

           

            gameObject.transform.position = raycastedTile.transform.position;

            if (dir == "right")
            {
                gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,gameObject.transform.eulerAngles.y,-90);
                AddOffset(false);
                
            }
            else if (dir == "up")
            {
                gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
                AddOffset(false);
            }
            else if (dir == "down")
            {
                gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, - 180);
                AddOffset(false);
            }
            else if (dir == "left")
            {
                gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 90);
                AddOffset(false);
            }
            curDir = dir;
            for (int i = 0; i < pastOccupiedTiles.Count; i++)
            {
                if(pastOccupiedTiles[i] != raycastedTile) pastOccupiedTiles[i].playerShip = null;

            }
            pastOccupiedTiles = occupiedTiles;
            Debug.LogWarning("Full Success of " + gameObject.name);
        }
        else
        {
            Debug.LogWarning(gameObject.name + " ship placement failed. no starter tile found. result - " + tileRaycastResult.collider.gameObject.name);
            return;
        }
    }
    public void AddOffset(bool negative)
    {   
        if(negative)
        {
            gameObject.transform.Translate(0, -yOffset, 0);
            return;
        }
        gameObject.transform.Translate(0, yOffset, 0);
    }
    
    bool CheckIfOccupied(bool player, ref Tile currentTile)
    {
        if (currentTile.playerShip != null && currentTile.playerShip != this)
        {
            Debug.LogWarning(gameObject.name + $" ship placement failed. Player ship standing.");
            return false;
        }
        currentTile.playerShip = this;
        Debug.Log(currentTile.gameObject.name + currentTile.playerShip.name);
        return true;
    }
    void ClearTiles( List<Tile> occupiedTiles)
    {   
        for (int i = 0; i < occupiedTiles.Count; i++)
        {
            occupiedTiles[i].playerShip = null;
        } 
    }
}
