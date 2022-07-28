using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ShipBase : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp = 0;
    [SerializeField] Transform[] _points;
    [SerializeField] Transform _shipGroup;
    List<Tile> pastOccupiedTiles = new List<Tile>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PlaceShip(float x, float y)
    {
        gameObject.transform.DOScale(1f, 0.5f);
        foreach (var ocTile in pastOccupiedTiles)
        {
            ocTile.occupiedShip = null;
        }
        List<Tile> occupiedTiles = new List<Tile>();
        RaycastHit2D result = Physics2D.Raycast(new Vector3(x, y, 0), Vector2.zero);
        if (result)
        {
            GameObject tile = result.collider.gameObject;
            if (result.collider.gameObject.GetComponent<Tile>())
            {
                Vector3 fallbackPosition = gameObject.transform.position;
                gameObject.transform.parent = tile.transform;
                gameObject.transform.localPosition = Vector3.zero;
                AddOffset();
                Debug.Log(x.ToString() + y.ToString());
                int resLength = 0;
                foreach (var point in _points)
                {
                    RaycastHit2D[] results = Physics2D.RaycastAll(point.position, Vector2.zero);
                    foreach (var res in results)
                    {
                       if(res.collider.gameObject.TryGetComponent(out Tile occupiedTile))
                        {
                            if (occupiedTile.occupiedShip != null)
                            {
                                Debug.Log(gameObject.name + " Place Occupied");
                                gameObject.transform.parent = _shipGroup;
                                gameObject.transform.position = fallbackPosition;
                                foreach (var ocTile in occupiedTiles)
                                {
                                    ocTile.occupiedShip = null;

                                }
                                return;
                            }
                            resLength++;
                            occupiedTile.occupiedShip = this;
                            occupiedTiles.Add(occupiedTile);
                            pastOccupiedTiles.Add(occupiedTile);
                        }
                    }
                }
              if (_points.Length > resLength )
                {
                    Debug.Log(gameObject.name + " Out of Bounds");
                    gameObject.transform.parent = _shipGroup;
                    gameObject.transform.position = fallbackPosition;
                    foreach (var ocTile in occupiedTiles)
                    {
                        ocTile.occupiedShip = null;
                    }
                    return;
                }

                if (_shipGroup.childCount <= 0)
                {
                    if (GameManager.Instance.gameState == GameManager.GameState.PlayerArrange)
                    {
                        GameManager.Instance.UpdateState(GameManager.GameState.AiArrange);
                    }
                }
            }

        }
    }
    public virtual void AddOffset()
    {

    }
}
