using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _playerTilesParent;
    [SerializeField] private Transform _aiTilesParent;
    [SerializeField] private Color _patternColor;

    public static TileManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        //create grid
        GenerateGrid();
    }
    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateGrid()
    {   
        //set camera position to grid center
        Camera.main.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        
        //tile spawning
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);

                spawnedTile.name = $"Tile {x} {y}";

                if(GameManager.Instance.gameState == GameManager.GameState.PlayerArrange)
                {
                    spawnedTile.transform.parent = _playerTilesParent;
                }
                if (GameManager.Instance.gameState == GameManager.GameState.AiArrange)
                {
                    spawnedTile.transform.parent = _aiTilesParent;
                }

                if ((x + y) % 2 == 1)
                {
                    spawnedTile.GetComponent<SpriteRenderer>().color = _patternColor;
                }
            }
        }
    }
}
