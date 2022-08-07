using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> ships = new List<GameObject>();
    [SerializeField] Transform shipGroup;
    [SerializeField] Transform aiGroup;
    public static AiManager Instance;

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


    private void Awake()
    {
        Instance = this;
    }

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

    public void InitAiManager()
    {
        aiGroup.gameObject.SetActive(false);

        for (int i = 0; i < shipGroup.childCount; i++)
        {
            ships.Add(shipGroup.GetChild(i).gameObject);
        }

        // All the cells 10x10 -  the field for setting AI ships (0 - free, 1 - used or restricted)

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                aiBattleField[i, j] = 0;
            }
        }

        ArrangeShips();
        GameManager.Instance.OnStateUpdated.AddListener(AiTurn);
    }

    void ArrangeShips()
    {
        TileManager.Instance.RandomPlaceShips(ships, Battlefield.AI);

        // All the cells 10x10 -  the field for setting AI ships (0 - free, 1 - used or restricted)

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                aiBattleField[i, j] = 0;
            }
        }
    }

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
                countOut++;
                break;
            }
            if (field[x, y + i] != 0)
            {
                countOut++;
                break;
            }
        }
        for (int i = 1; i < size; i++)
        {
            if (y - i < 0)
            {
                countOut++;
                break;
            }
            if (field[x, y - i] != 0)
            {
                countOut++;
                break;
            }
        }
        for (int i = 1; i < size; i++)
        {
            if (x + i > 9)
            {
                countOut++;
                break;
            }
            if (field[x + i, y] != 0)
            {
                countOut++;
                break;
            }
        }
        for (int i = 1; i < size; i++)
        {
            if (x - i < 0)
            {
                countOut++;
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
        var shot = new int[] { 10, 10 };
        while (size > 1)
        {
            var vesselOptions = 4;
            while ((shot[0] == 10) && (vesselOptions > 0))
            {
                shot = FindShot(field, size, vesselOptions);
                vesselOptions--;
            }
            if (shot[0] != 10)
            {
                Debug.Log("Options:" + (vesselOptions + 1));
                Debug.Log("Size:" + size);
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
                newCell = new int[] { x, y + 1 };
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
        var index = Random.Range(0, Mathf.Max(1, cellOptions.Count));
        Debug.Log("OPtionNumber" + index);
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

    void AiTurn(GameManager.GameState gameState)
    {
        if (gameState != GameManager.GameState.AiTurn) return;

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

        }
        Debug.Log("x:" + aiMove[0]);
        Debug.Log("y:" + aiMove[1]);
        Debug.Log("cell:" + aiBattleField[aiMove[0], aiMove[1]]);
        aiBattleField[aiMove[0], aiMove[1]] = 1;
        float[] aiMoveFloat = new float[2];
        aiMoveFloat[0] = (float)aiMove[0];
        aiMoveFloat[1] = (float)aiMove[1];
        int result = ShootingManager.Instance.ShootTile(aiMoveFloat, false);
        if ((result == 0) || (result == 1))
        {
            aiBattleField[aiMove[0], aiMove[1]] = 1;
            woundedShip.Add(aiMove);
            if (result == 1)
            {
                MarkShotsAround(woundedShip);
                var length = woundedShip.Count;
                fleet[length]--;
                woundedShip.Clear();
                notAllKilled = FleetNotEmpty(fleet);
                if (!notAllKilled)
                {
                    Debug.Log("End Game! Computer rules!!!");
                    GameManager.Instance.UpdateState(GameManager.GameState.PlayerLoose);
                    return;
                }

            }
            StartCoroutine(Delay(1));
        }
        else GameManager.Instance.ContinueMessage(true);

    }
    IEnumerator Delay(int delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.UpdateState(GameManager.GameState.AiTurn);
    }
}
