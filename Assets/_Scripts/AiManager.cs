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
        StartCoroutine(TileManager.Instance.GenerateGrid());
        // Arrange ships;
        aiGroup.gameObject.SetActive(true);

        GameManager.Instance.OnStateUpdated.RemoveListener(ArrangeShips);
        //GameManager.Instance.UpdateState(GameManager.GameState.PlayerTurn);
    }
}
