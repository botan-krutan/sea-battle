using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> ships = new List<GameObject>();
    [SerializeField] Transform shipGroup;
    [SerializeField] Transform aiGroup;
    void Start()
    {
        aiGroup.gameObject.SetActive(false);
        for (int i = 0; i < shipGroup.childCount; i++)
        {
            ships.Add(shipGroup.GetChild(i).gameObject);
        }
        GameManager.Instance.OnStateUpdated.AddListener(ArrangeShips);
        GameManager.Instance.OnStateUpdated.AddListener(AiTurn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ArrangeShips(GameManager.GameState gameState)
    {
        TileManager.Instance.GenerateGrid();
        
        aiGroup.gameObject.SetActive(true);
        // Arrange ships HERE

        // ship.GetComponent<ShipBase>().PlaceShip(10, 4); - place ship
        // foreach(var ship in ships) - for loop
        // ship.GetComponent<ShipBase>().hp - get hp

        // Structure to choose a ships direction

        var directions = new Dictionary<int, string>()
        {
            {0, "left"},
            {1, "up"},
            {2, "right"},
            {3, "down"},
        };

        // List of cells that new settled ship occupies

        List <(int, int)> shipCells = new List<(int, int)>();

        // All the cells 10x10 -  the field for setting AI ships (0 - free, 1 - used or restricted)

        int[,] aiBattleField = new int[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                aiBattleField[i, j] = 0;
            }
        }

            
        for (int cells = 2; cells > 0; cells--) // from biggest ship (4) - to 1-celled ship
        {
            for (int i = 0; i <= 4-cells; i++)
            {
                int x = 0;
                int y = 0;
                bool thisShipIsSet = false;
                while (! thisShipIsSet)
                {
                    x = Random.Range(0, 10);
                    y = Random.Range(0, 10);
                    int dir = 1; // replace by Random.Range(0, 4)
                    thisShipIsSet = SetShip(x, y, cells, directions[dir]); // true if ship is set and cells are marked with '1'
                }

                foreach (var ship in ships)
                {
                    if (ship.GetComponent<ShipBase>().hp == cells)
                    {
                        Debug.Log("Placed Ship");
                        ship.GetComponent<ShipBase>().PlaceShip(x, y); // add dir somehow!!!
                        ships.Remove(ship);
                        break;
                    }
                }
            }
        }

        // Marking all the cells that ship occupies and plus all near cells (prohibited)

        void MarkShotsAround(List <(int, int)> listCells)
        {
            foreach (var cell in listCells) // each cell beneath the new settled ship
            {
                int x = cell.Item1;
                int y = cell.Item2;
                for (int i = -1; i <= 1; i++) 
                {
                    for (int j = -1;  j <= 1; j++)
                    {
                        if ((x + i >= 0) && (x + i <= 9) && (y + j >= 0) && (y + j <= 9)) 
                        {                            
                            aiBattleField[x + i, y + j] = 1;                                                       
                        }
                    }
                }                   
            }           
        }
    
        // Looking and finding free cells (number==cells) for a new ship from random start (x,y) to random direction

        bool SetShip(int x, int y, int cells, string dir)
        {
            int currentX = x;
            int currentY = y;
            shipCells.Clear();
            for (int i = 0; i < cells; i++)
            {
                if ((currentX < 0) || (currentX > 9) || (currentY < 0) || (currentY > 9))
                {
                    return false;
                }
                if (aiBattleField[currentX, currentY] != 0)
                {
                    return false;
                }
                else
                {
                    shipCells.Add((currentX, currentY));
                    switch (dir)
                    {
                        case "left":
                            currentX--;
                            break;
                        case "up":
                            currentY++;
                            break;
                        case "right":
                            currentX++;
                            break;
                        case "down":
                            currentY--;
                            break;                    
                    }
                    
                }
            }
            MarkShotsAround(shipCells);
            return true;
        }









        GameManager.Instance.OnStateUpdated.RemoveListener(ArrangeShips);
        GameManager.Instance.UpdateState(GameManager.GameState.PlayerTurn);

    }

    void AiTurn(GameManager.GameState gameState) 
    {
        if (gameState == GameManager.GameState.AiTurn) {
            // Ai Turn
            // ShootTile
        }
    }
}
