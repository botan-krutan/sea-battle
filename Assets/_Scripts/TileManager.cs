using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    [SerializeField] private Transform _playerShipsParent;


    public static TileManager Instance;
    public static Action<bool> OnBattlefieldChanged;

    private Tile[,] playerTileArray = new Tile[10, 10];
    private Tile[,] aITileArray = new Tile[10, 10];
    private List<GameObject> _playerShips = new List<GameObject>();
    private const float AI_BATTLEFIELD_OFFSET = 12f;
    private List<ShipBase> _currentPlayerShipsOnBattlefield = new List<ShipBase>();

    private void Awake()
    {
        Instance = this;
    }

    public void InitTileManager()
    {
        GenerateGrid(playerTileArray, 0);
        GenerateGrid(aITileArray, AI_BATTLEFIELD_OFFSET);

        foreach (Transform shipTransform in _playerShipsParent)
        {
            _playerShips.Add(shipTransform.gameObject);
        }
    }

    public void AutoPlaceShipsButtonPressHandler()
    {
        RandomPlaceShips(_playerShips, Battlefield.Player);
    }

    public void GenerateGrid(Tile[,] tileArray, float xOffset)
    {
        //tile spawning
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(xOffset + x, y), Quaternion.identity);
                spawnedTile.transform.parent = _tileGroup;
                spawnedTile.name = $"Tile {x}{y}";
                spawnedTile.InitTile(x, y);
                if ((x + y) % 2 == 1)
                {
                    spawnedTile.GetComponent<SpriteRenderer>().color = _patternColor;
                }
                tileArray[x, y] = spawnedTile;
            }
        }
    }

    public void RandomPlaceShips(List<GameObject> ships, Battlefield battlefield)
    {
        foreach (var shipGO in ships)
        {
            ShipBase ship = shipGO.GetComponent<ShipBase>();
            if (ship.IsPlaced) continue;
            bool thisShipIsSet = false;
            int x = 0;
            int y = 0;

            while (!thisShipIsSet)
            {
                x = Random.Range(0, 10);
                y = Random.Range(0, 10);
                ship.SetRandomDirection();
                thisShipIsSet = CanPlaceShip(ship, x, y, battlefield);
            }
            PlaceShip(ship, x, y, battlefield);
        }
    }

    public void PlaceShip(ShipBase ship, int x, int y, Battlefield battlefield)
    {
        if (!CanPlaceShip(ship, x, y, battlefield)) return;

        Tile tile = GetTile(x, y, battlefield);
        ship.UpdateShipPosition(tile.transform.position);
        SetShipOnBattlefield(ship, x, y, battlefield);
        if (battlefield == Battlefield.AI) return;

        _currentPlayerShipsOnBattlefield.Add(ship);
        if (_playerShips.Find(s => !s.GetComponent<ShipBase>().IsPlaced) == null)
            OnBattlefieldChanged?.Invoke(true);
        else OnBattlefieldChanged?.Invoke(false);
    }

    public bool CanPlaceShip(ShipBase ship, int x, int y, Battlefield battlefield)
    {
        Tile tile;
        for (int i = 0; i < ship.hp; i++)
        {
            switch (ship.currentDirection)
            {
                case ShipDirection.Left:
                    tile = GetTile(x - i, y, battlefield);
                    if (x - i < 0 || tile.state == TileState.Occupied || tile.state == TileState.Blocked)
                    {
                        return false;
                    }
                    break;
                case ShipDirection.Right:
                    tile = GetTile(x + i, y, battlefield);
                    if (x + i >= 10 || tile.state == TileState.Occupied || tile.state == TileState.Blocked)
                    {
                        return false;
                    }
                    break;

                case ShipDirection.Up:
                    tile = GetTile(x, y + i, battlefield);
                    if (y + i >= 10 || tile.state == TileState.Occupied || tile.state == TileState.Blocked)
                    {
                        return false;
                    }
                    break;
                case ShipDirection.Down:
                    tile = GetTile(x, y - i, battlefield);
                    if (y - i < 0 || tile.state == TileState.Occupied || tile.state == TileState.Blocked)
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

    private void SetStatesTilesAround(int x, int y, TileState state, Battlefield battlefield)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                var tile = GetTile(x + i, y + j, battlefield);
                if (tile != null && tile.state != TileState.Occupied) tile.SetTileState(state, null);
            }
        }
    }

    private void SetShipOnBattlefield(ShipBase ship, int x, int y, Battlefield battlefield)
    {
        Tile tile;

        for (int i = 0; i < ship.hp; i++)
        {
            switch (ship.currentDirection)
            {
                case ShipDirection.Up:
                    tile = GetTile(x, y + i, battlefield);
                    tile.SetTileState(TileState.Occupied, ship);
                    ship.AddOccupiedTile(tile);
                    SetStatesTilesAround(x, y + i, TileState.Blocked, battlefield);
                    break;

                case ShipDirection.Down:
                    tile = GetTile(x, y - i, battlefield);
                    tile.SetTileState(TileState.Occupied, ship);
                    ship.AddOccupiedTile(tile);
                    SetStatesTilesAround(x, y - i, TileState.Blocked, battlefield);
                    break;

                case ShipDirection.Right:
                    tile = GetTile(x + i, y, battlefield);
                    tile.SetTileState(TileState.Occupied, ship);
                    ship.AddOccupiedTile(tile);
                    SetStatesTilesAround(x + i, y, TileState.Blocked, battlefield);
                    break;

                case ShipDirection.Left:
                    tile = GetTile(x - i, y, battlefield);
                    tile.SetTileState(TileState.Occupied, ship);
                    ship.AddOccupiedTile(tile);
                    SetStatesTilesAround(x - i, y, TileState.Blocked, battlefield);
                    break;
            }
        }
    }

    public void RemoveShipOnBattlefield(ShipBase ship)
    {
        _currentPlayerShipsOnBattlefield.Remove(ship);

        foreach (var tile in ship.currentlyOccupiedTiles)
            tile.SetTileState(TileState.Clear, null);

        ship.ClearOccupiedTile();
        UpdateBlockedTiles();
        OnBattlefieldChanged?.Invoke(false);
    }

    public void UpdateBlockedTiles()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                var tile = GetTile(x, y, Battlefield.Player);
                if (tile.state == TileState.Blocked) tile.SetTileState(TileState.Clear, null);

            }
        }

        foreach (var ship in _currentPlayerShipsOnBattlefield)
        {
            foreach (var tile in ship.currentlyOccupiedTiles)
                SetStatesTilesAround(tile.x, tile.y, TileState.Blocked, Battlefield.Player);
        }
    }


    public Tile GetTile(int x, int y, Battlefield battlefield)
    {
        if (x < 0 || x > 9 || y < 0 || y > 9) return null;

        switch (battlefield)
        {
            case Battlefield.Player: return playerTileArray[x, y];
            case Battlefield.AI: return aITileArray[x, y];
            default: return playerTileArray[x, y];
        }
    }
}
