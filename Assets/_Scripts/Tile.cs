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
    //public ShipBase playerShip;
    //public ShipBase aiShip;
    public Color occupiedColor;
    //public ShipBase[] nearShips = new ShipBase[2];
    public ShipBase ship { get; private set; }
    public TileState state { get; private set; }

    void Start()

    {
        // Tile scaling animation
        //transform.DOScale(1, 0.5f);
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
        //if (GameManager.Instance.debugMode)
        //{   

        //    if (aiShip)
        //    {
        //        GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        //    }
        //    else if (playerShip && GameManager.Instance.gameState == GameManager.GameState.PlayerArrange )
        //    {
        //        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        //    }
        //    else if (nearShips[0] != null || nearShips[1] != null)
        //    {
        //        GetComponent<SpriteRenderer>().color = occupiedColor;
        //    }
        //    else GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        //}
        //else GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
}
