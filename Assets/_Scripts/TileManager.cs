using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum Battlefield
{
    AI,
    Player
}

public class TileManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Color _patternColor;
    [SerializeField] private Transform _tileGroup;

    public static TileManager Instance;

    private Tile[,] playerTileArray = new Tile[10, 10];
    private Tile[,] aITileArray = new Tile[10, 10];
    private const float AI_BATTLEFIELD_OFFSET = 12f;

    void Start()
    {
        //create grid
        GenerateGrid(playerTileArray, 0);
        GenerateGrid(aITileArray, AI_BATTLEFIELD_OFFSET);
    }

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid(Tile[,] tileArray, float xOffset)
    {
        //set camera position to grid center
        Camera.main.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        //tile spawning
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(xOffset + x, y), Quaternion.identity);
                spawnedTile.transform.parent = _tileGroup;
                spawnedTile.name = $"Tile {x}{y}";
                if ((x + y) % 2 == 1)
                {
                    spawnedTile.GetComponent<SpriteRenderer>().color = _patternColor;
                }
                tileArray[x, y] = spawnedTile;
            }
        }
    }

    public void TryPlaceShip(ShipBase ship, float x, float y, ShipDirection dir, Battlefield battlefield)
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
        if (x <= 9.5 && y <= 9.5 && x >= -0.5 && y >= -0.5)
        {
            if (tileRaycastResult && tileRaycastResult.collider.gameObject.TryGetComponent(out Tile raycastedTile))
            {
                Debug.Log(raycastedTile.gameObject.name);
                int tilex = int.Parse(raycastedTile.gameObject.name.Replace($"Tile ", "")[0].ToString());
                int tiley = int.Parse(raycastedTile.gameObject.name.Replace($"Tile ", "")[1].ToString());

                if (CanPlaceShip(ship, tilex, tiley, battlefield))
                {
                    //for (int i = 0; i < nearOccupiedTiles.Count; i++)
                    //{
                    //    if (nearOccupiedTiles[i].nearShips[0] == this) nearOccupiedTiles[i].nearShips[0] = null;
                    //    else if (nearOccupiedTiles[i].nearShips[1] == this) nearOccupiedTiles[i].nearShips[1] = null;
                    //}
                    nearOccupiedTiles.Clear();
                    //Debug.Log()
                    SetShipOnBattlefield(ship, tilex, tiley, battlefield);
                    MarkShotsAround(futureOccupiedTiles, ref nearOccupiedTiles);
                    gameObject.transform.position = raycastedTile.transform.position;

                    if (dir == ShipDirection.Right)
                    {
                        gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -90);
                        //AddOffset(false);

                    }
                    else if (dir == ShipDirection.Up)
                    {
                        gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
                        //AddOffset(false);
                    }
                    else if (dir == ShipDirection.Down)
                    {
                        gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -180);
                        //AddOffset(false);
                    }
                    else if (dir == ShipDirection.Left)
                    {
                        gameObject.transform.eulerAngles = gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 90);
                        //AddOffset(false);
                    }
                    ship.curentDirection = dir;

                    for (int i = 0; i < ship.currentlyOccupiedTiles.Count; i++)
                    {
                        if (ship.currentlyOccupiedTiles[i] != raycastedTile && !futureOccupiedTiles.Contains(ship.currentlyOccupiedTiles[i])) ship.currentlyOccupiedTiles[i].playerShip = null;
                    }

                    ship.currentlyOccupiedTiles = futureOccupiedTiles;
                    ship.isPlaced = true;
                    AudioPlayer.Instance.PlaySound(placeSound);
                    if (battlefield == Battlefield.Player)
                    {
                        foreach (var item in gameObject.transform.parent.GetComponentsInChildren<ShipBase>(false))
                        {
                            if (!item.isPlaced) return;
                        }
                        GameManager.Instance.ContinueMessage(true);
                    }
                }

            }
            else
            {
                Debug.LogWarning(gameObject.name + " ship placement failed. no starter tile found. result - " + tileRaycastResult.collider.gameObject.name);
                return;
            }
        }
    }

    bool CanPlaceShip(ShipBase ship, int x, int y, Battlefield battlefield)
    {
        for (int i = 0; i < ship.hp; i++)
        {
            switch (ship.curentDirection)
            {
                case ShipDirection.Left:
                    if (x - i >= 10 || x - i < 0 || GetTile(x - i, y, battlefield).state == TileState.Occupied)
                    {
                        return false;
                    }
                    break;
                case ShipDirection.Right:
                    if (x + i >= 10 || x + i < 0 || GetTile(x + i, y, battlefield).state == TileState.Occupied)
                    {
                        return false;
                    }
                    break;

                case ShipDirection.Up:
                    if (y + i >= 10 || y + i < 0 || GetTile(x, y + i, battlefield).state == TileState.Occupied)
                    {
                        return false;
                    }
                    break;
                case ShipDirection.Down:
                    if (y - i >= 10 || y - i < 0 || GetTile(x, y - i, battlefield).state == TileState.Occupied)
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

    void SetShipOnBattlefield(ShipBase ship, int x, int y, Battlefield battlefield)
    {
        Tile tile;
        for (int i = 0; i < ship.hp; i++)
        {
            switch (ship.curentDirection)
            {
                case ShipDirection.Up:
                    tile = GetTile(x, y + i, battlefield);
                    tile.SetTileState(TileState.Occupied, ship);
                    ship.currentlyOccupiedTiles.Add(tile);
                    break;

                case ShipDirection.Down:
                    tile = GetTile(x, y - i, battlefield);
                    tile.SetTileState(TileState.Occupied, ship);
                    ship.currentlyOccupiedTiles.Add(tile);
                    break;

                case ShipDirection.Right:
                    tile = GetTile(x + i, y, battlefield);
                    tile.SetTileState(TileState.Occupied, ship);
                    ship.currentlyOccupiedTiles.Add(tile);
                    break;

                case ShipDirection.Left:
                    tile = GetTile(x - i, y, battlefield);
                    tile.SetTileState(TileState.Occupied, ship);
                    ship.currentlyOccupiedTiles.Add(tile);
                    break;
            }
        }
    }

    public Tile GetTile(int x, int y, Battlefield battlefield = Battlefield.Player)
    {
        switch (battlefield)
        {
            case Battlefield.Player: return playerTileArray[x, y];
            case Battlefield.AI: return aITileArray[x, y];
            default: return playerTileArray[x, y];
        }
    }
}
