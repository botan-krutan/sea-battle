using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> ships = new List<GameObject>();
    [SerializeField] Transform shipGroup;
    [SerializeField] Transform aiGroup;

    int[,] aiBattleField = new int[10, 10];
    bool notAllKilled = true;
    Dictionary<int, int> fleet = new Dictionary<int, int>() // all the enemy's ships left
    {
      { 4,1 },
      { 3,2 },
      { 2,3 },
      { 1,4 }
    }; 
    List<int[]> woundedShip = new List<int[]>(); // if we have partitially destroyed ship
    int[] aiMove = new int[2]; // shot of computer

    // Marking all the cells that ship occupies and plus all near cells (prohibited)
    void MarkShotsAround(List<int[]> listCells)
    {
        foreach (var cell in listCells) // each cell beneath the new settled ship
        {
            int x = cell[0];
            int y = cell[1];
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((x + i >= 0) && (x + i <= 9) && (y + j >= 0) && (y + j <= 9))
                    {
                        aiBattleField[x + i, y + j] = 1;
                    }
                }
            }
        }
    }

    void Start()
    {
        aiGroup.gameObject.SetActive(false);

        for (int i = 0; i < shipGroup.childCount; i++)
        {
            ships.Add(shipGroup.GetChild(i).gameObject);
        }

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                aiBattleField[i, j] = 0;
            }
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
        
        // Structure to choose a ships direction

        var directions = new Dictionary<int, string>()
        {
            {0, "left"},
            {1, "up"},
            {2, "right"},
            {3, "down"},
        };

        // List of cells that new settled ship occupies

        List <int[]> shipCells = new List<int[]>();

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
                        ship.GetComponent<ShipBase>().PlaceShip(x, y); // add dir later!!!
                        ships.Remove(ship);
                        break;
                    }
                }
            }
        }

       

       

    
        // Checking free cells (number==cells) for a new ship from random start (x,y) to random direction

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
                    var newCell = new int[]{ currentX, currentY };
                    shipCells.Add(newCell);
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
        int NumberOfVessels(int[,] field, int x, int y, int size)
        {
            var countOptions = 4;
            var countOut = 0;
            if (field[x, y] != 0)
            {
                //cell is used - no solution
                return 0;
            }
            else if (size == 1)
            {
                return 1;
            }
            for (int i = 1; i < size; i++)            
            {
                if (y + i > 9)
                {
                    countOut ++;
                    break;
                }
                if (field[x, y + i] != 0)
                {
                    countOut ++;
                    break;
                }
            }
            for (int i = 1; i < size; i++)
            {
                if (y - i < 0)
                {
                    countOut ++;
                    break;
                }
                if (field[x, y - i] != 0)
                {
                    countOut ++;
                    break;
                }
            }
            for (int i = 1; i < size; i++)
            {
                if (x + i > 9)
                {
                    countOut ++;
                    break;
                }
                if (field[x + i, y] != 0)
                {
                    countOut ++;
                    break;
                }
            }
            for (int i = 1; i < size; i++)
            {
                if (x - i < 0)
                {
                    countOut ++;
                    break;
                }
                if (field[x - i, y] != 0)
                {
                    countOut += 1;
                    break;
                }
            }
            countOptions -= countOut;
            return countOptions;
        }

        int[] FindShot(int[,] field, int size, int options)
        {
            var shotOptions = new List<int[]>();
            for (int i = 0; i < 10; i++)           
            {
                for (int j = 0; j < 10; j++)
                {
                    if (NumberOfVessels(field, i, j, size) == options)
                    {
                        var newShot = new int[] { i, j };
                        shotOptions.Add(newShot);
                    }
                }
            }
            if (shotOptions.Count > 0)
            {
                var randIndex = Random.Range(0, shotOptions.Count);
                return shotOptions[randIndex];
            }
            else
            {
                return new int[] { 10, 10 };
            }
        }

        int[] CompNextMove(int[,] field, int bigShip)
        {
            // size is the biggest ship to find
            var size = bigShip;
            var shot = new int[] {10, 10};
            while (size > 1)
            {
                var vesselOptions = 4;
                while ((shot[0] == 10) && (vesselOptions > 0))
                {
                    shot = FindShot(field, size, vesselOptions);
                    vesselOptions --;
                }
                if (shot[0] != 10)
                {
                    Debug.Log(vesselOptions + 1);
                    Debug.Log(size);
                    return shot;
                }
                else
                {
                    size -= 1;
                }
            }
            return FindShot(field, 1, 1);
        }

        int[] AnyEmptyCellAroundOne(int x, int y)
        {
            var cellOptions = new List<int[]>();
            int[] newCell; 
            if (x + 1 <= 9)
            {
                if (aiBattleField[x + 1, y] == 0)
                {
                    newCell = new int[] { x + 1, y };
                    cellOptions.Add(newCell);
                }
            }
            if (y + 1 <= 9)
            {
                if (aiBattleField[x, y + 1] == 0)
                {
                    newCell = new int[] { x, y +1 };
                    cellOptions.Add(newCell);
                }
            }
            if (x - 1 >= 0)
            {
                if (aiBattleField[x - 1, y] == 0)
                {
                    newCell = new int[] { x - 1, y };
                    cellOptions.Add(newCell);
                }
            }
            if (y - 1 >= 0)
            {
                if (aiBattleField[x, y - 1] == 0)
                {
                    newCell = new int[] { x, y - 1 };
                    cellOptions.Add(newCell);
                }
            }
            var index = Random.Range(0, cellOptions.Count);
            return cellOptions[index];
        }

        int[] AnyEmptyCellDirectioned(List<int[]> shipOnFire, char direction)
        {
            var cellOptions = new List<int[]>();
            int[] newCell;
            if (direction == 'x')
            {
                var minY = findMin(shipOnFire, 1);
                var maxY = findMax(shipOnFire, 1);
                var x = shipOnFire[0][0];
                if (minY - 1 >= 0)
                {
                    if (aiBattleField[x, minY - 1] == 0)
                    {
                        newCell = new int[] { x, minY - 1 };
                        cellOptions.Add(newCell);
                    }
                }
                if (maxY + 1 <= 9)
                {
                    if (aiBattleField[x, maxY + 1] == 0)
                    {
                        newCell = new int[] { x, maxY + 1 };
                        cellOptions.Add(newCell);
                    }
                }
            }
            else if (direction == 'y')
            {
                var minX = findMin(shipOnFire, 0);
                var maxX = findMax(shipOnFire, 0);
                var y = shipOnFire[0][1];
                if (minX - 1 >= 0)
                {
                    if (aiBattleField[minX - 1, y] == 0)
                    {
                        newCell = new int[] { minX - 1, y };
                        cellOptions.Add(newCell);
                    }
                }
                if (maxX + 1 <= 9)
                {
                    if (aiBattleField[maxX + 1, y] == 0)
                    {
                        newCell = new int[] { maxX + 1, y };
                        cellOptions.Add(newCell);
                    }
                }
            }
            var index = Random.Range(0, cellOptions.Count);
            return cellOptions[index];
        }

        int findMin(List<int[]> listCells, int num)
        {
            var minCell = listCells[0][num];
            foreach (var cell in listCells)
            {
                if (cell[num] < minCell)
                {
                    minCell = cell[num];
                }
            }
            return minCell;
        }

        int findMax(List<int[]> listCells, int num)
        {
            var maxCell = listCells[0][num];
            foreach (var cell in listCells)
            {
                if (cell[num] > maxCell)
                {
                    maxCell = cell[num];
                }
            }
            return maxCell;
        }

        int[] FindNextShot(List<int[]> shipOnFire)
        {
            if (shipOnFire.Count == 1)
            {
                return AnyEmptyCellAroundOne(shipOnFire[0][0], shipOnFire[0][1]);
            }
            if (shipOnFire[0][0] == shipOnFire[1][0])
            {
                return AnyEmptyCellDirectioned(shipOnFire, 'x');
            }
            else
            {
                return AnyEmptyCellDirectioned(shipOnFire, 'y');
            }
        }

        bool FleetNotEmpty(Dictionary<int, int> dictFleet)
        {
            foreach (var ship in dictFleet)
            {
                if (ship.Value > 0)
                {
                    return true;
                }
            }
            return false;
        }

        int BiggestLeft(Dictionary<int, int> dictFleet)
        {
            foreach (var ship in dictFleet)
            {
                if (ship.Value > 0)
                {
                    return ship.Key;
                }
            }
            return 0;
        }

        if (gameState == GameManager.GameState.AiTurn) 
        {

            while (notAllKilled)
            {
                if (woundedShip.Count == 0)
                {
                    var biggestShip = BiggestLeft(fleet);
                    aiMove = CompNextMove(aiBattleField, biggestShip);
                }
                else
                {
                    aiMove = FindNextShot(woundedShip);
                }
                if (aiMove[0] == 10)
                {
                    Debug.Log("Can't be!!!");
                    break;
                }
                Debug.Log(aiMove);
                aiBattleField[aiMove[0], aiMove[1]] = 1;

                var result = 'm'; // MakeMove(aiMove);
                if ((result == 'w') || (result == 'k'))
                {
                    aiBattleField[aiMove[0], aiMove[1]] = 2;
                    woundedShip.Add(aiMove);
                    if (result == 'k')
                    {
                        MarkShotsAround(woundedShip);
                        var length = woundedShip.Count;
                        fleet[length]--;
                        result = 'm';
                        woundedShip.Clear();
                        notAllKilled = FleetNotEmpty(fleet);
                    }
                }
                
            }

            Debug.Log("End Game! Computer rules!!!");            
            // Ai Turn
            // ShootTile
        }
    }
}
