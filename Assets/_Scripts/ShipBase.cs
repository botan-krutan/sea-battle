using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ShipDirection
{
    Down,
    Right,
    Up,
    Left
}

public class ShipBase : MonoBehaviour
{
    public int hp;
    public ShipDirection currentDirection { get; private set; } = ShipDirection.Up;
    public List<Tile> currentlyOccupiedTiles = new List<Tile>();
    public bool IsPlaced => currentlyOccupiedTiles.Count > 0;

    public void RotateShip()
    {
        int state = (int)currentDirection;
        state++;
        if (state > 3) state = 0;
        currentDirection = (ShipDirection)state;
        UpdateShipRotation();
    }

    public void SetRandomDirection()
    {
        int dir = Random.Range(0, 4);
        currentDirection = (ShipDirection)dir;
        UpdateShipRotation();
    }

    public void AddOccupiedTile(Tile tile) => currentlyOccupiedTiles.Add(tile);

    public void ClearOccupiedTile() => currentlyOccupiedTiles.Clear();

    private void UpdateShipRotation()
    {
        switch (currentDirection)
        {
            case ShipDirection.Right:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -90);
                break;
            case ShipDirection.Up:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
                break;
            case ShipDirection.Down:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -180);
                break;
            case ShipDirection.Left:
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 90);
                break;
        }
    }

    public void UpdateShipPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }
}
