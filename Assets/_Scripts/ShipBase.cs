using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ShipBase : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp = 0;
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
        if (player)
        {
            foreach (var ocTile in pastOccupiedTiles)
            {
                ocTile.playerShip = null;
            }
        }
        else
        {
            foreach (var ocTile in pastOccupiedTiles)
            {
                ocTile.aiShip = null;
            }
        }

        
        List<Tile> occupiedTiles = new List<Tile>();
        RaycastHit2D tileRaycastResult = new RaycastHit2D();
        RaycastHit2D[] results = Physics2D.RaycastAll(new Vector3(x, y, 0), Vector2.zero);
        foreach (var result in results)
        {
            if (result.collider.gameObject.TryGetComponent<Tile>(out Tile sussy))
            {
                tileRaycastResult = result;
            }
        }
        if (tileRaycastResult && tileRaycastResult.collider.gameObject.TryGetComponent(out Tile raycastedTile))    
        {
            Debug.Log(raycastedTile.gameObject.name);
            int tilex = int.Parse(raycastedTile.gameObject.name.Replace($"Tile ", "")[0].ToString());
            int tiley = int.Parse(raycastedTile.gameObject.name.Replace($"Tile ", "")[1].ToString());

            Debug.Log($"Tile {tilex} {tiley}");
            for (int i = 0; i < shipLength; i++)
            {
                if(dir == "up")
                {
                   // Debug.Log($"Tile {tilex}{tiley + i}");
                    if (GameObject.Find($"Tile {tilex}{tiley + i}"))
                    {
                        Tile currentTile = GameObject.Find($"Tile {tilex}{tiley + i}").GetComponent<Tile>();
                        if (player)
                        {
                            if (currentTile.playerShip != null && currentTile.playerShip != this)
                            {
                                Debug.LogWarning(gameObject.name + " ship placement failed. Player ship standing ");
                                return;
                            }
                            currentTile.playerShip = this;
                        }
                        else
                        {
                            if (currentTile.aiShip != null && currentTile.aiShip != this)
                            {
                                Debug.LogWarning(gameObject.name + " ship placement failed. Ai ship standing.");
                                return;
                            }
                            currentTile.aiShip = this;
                        }


                    }
                    else
                    {
                        Debug.LogWarning(gameObject.name + $" ship placement failed. Tile {tilex}{tiley + i} not found.");
                        return;
                    }
                }
                else if (dir == "right")
                {
                    if (GameObject.Find($"Tile {tilex + i}{tiley}"))
                    {   
                        Tile currentTile = GameObject.Find($"Tile {tilex + i}{tiley}").GetComponent<Tile>();
                        if (player)
                        {
                            if (currentTile.playerShip != null && currentTile.playerShip != this)
                            {
                                Debug.LogWarning(gameObject.name + $" ship placement failed. Player ship standing.");
                                return;
                            }
                            currentTile.playerShip = this;
                        }
                        else
                        {
                            if (currentTile.aiShip != null && currentTile.aiShip != this)
                            {
                                Debug.LogWarning(gameObject.name + " ship placement failed. Ai ship standing.");
                                return;
                            }
                            currentTile.aiShip = this;
                        }
                    }
                    else
                    {
                        Debug.LogWarning(gameObject.name + $" ship placement failed.Tile {tilex}{tiley + i} not found.");
                        return;
                    }
                }
            }
            Debug.LogWarning("Full Success of " + gameObject.name);
            gameObject.transform.position = raycastedTile.transform.position;
            if (dir == "right")
            {
                gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,gameObject.transform.eulerAngles.y,-90);
            }
            if (dir == "up")
            {
                gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
            }
            AddOffset();
            
            

        }
        else
        {
            Debug.LogWarning(gameObject.name + " ship placement failed. no starter tile found. result - " + tileRaycastResult.collider.gameObject.name);
            return;
        }
    }
    public virtual void AddOffset()
    {

    }
}
