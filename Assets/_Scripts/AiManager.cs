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

        var directions = new Dictionary<int, string>()
        {
            /*{ "left", (-1, 0) },
            { "up", (0, 1) },
            { "right", (1, 0) },
            { "down", (0, 1) }*/

            {0, "left"},
            {1, "up"},
            {2, "right"},
            {3, "down"},
        };

        List <(int, int)> shipCells = new List<(int, int)>();

        int[,] aiBattleField = new int[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                aiBattleField[i, j] = 0;
            }
        }

        //Dictionary<int, int[]> myFleet = new Dictionary<int, int[]>()
        //{
        //};
       
        for (int cells = 2; cells > 0; cells--) 
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
                    int dir = 1;
                    thisShipIsSet = SetShip(x, y, cells, directions[dir]);
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

        void MarkShotsAround(List <(int, int)> listCells)
        {
            foreach (var cell in listCells) 
            {
                int x = cell.Item1;
                int y = cell.Item2;
                for (int i = -1; i <= 1; i++) 
                {
                    for (int j = -1;  j <= 1; j++)
                    {
                        if ((x + i >= 0) && (x + i <= 9) && (y + j >= 0) && (y + j <= 9)) 
                        {
                            if (aiBattleField[x + i, y + j] == 0)
                            {
                                aiBattleField[x + i, y + j] = 1;
                            }                            
                        }
                    }
                }                   
            }           
        }
    

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
        //GameManager.Instance.UpdateState(GameManager.GameState.PlayerTurn);

    }
}
