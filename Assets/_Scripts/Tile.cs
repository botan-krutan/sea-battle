using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TileState
{
    Clear,
    Occupied,
    Blocked
}

public class Tile : MonoBehaviour
{
    public Color occupiedColor;
    public int x { get; private set; }
    public int y { get; private set; }

    public ShipBase ship { get; private set; }
    public TileState state { get; private set; }

    void Start()

    {
        // Tile scaling animation
        //transform.DOScale(1, 0.5f);
    }

    public void InitTile(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetTileState(TileState state, ShipBase ship)
    {
        if (ship != null)
        {
            this.state = TileState.Occupied;
            this.ship = ship;
            return;
        }
        this.state = state;
    }


    void Update()
    {
        if (GameManager.debugMode)
        {

            if (state == TileState.Blocked)
            {
                GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            }
            if (state == TileState.Occupied)
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
            if (state == TileState.Clear)
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 255, 255); ;
            }
        }
        else GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
}
