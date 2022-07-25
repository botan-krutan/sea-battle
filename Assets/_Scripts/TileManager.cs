using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] Tile _tilePrefab;
    [SerializeField] Transform _tilesParent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateGrid());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator GenerateGrid()
    {
        Camera.main.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.transform.parent = _tilesParent;
                yield return new WaitForSeconds(0.01f);
            }
        }
        
    }
}
