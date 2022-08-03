using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public static ShootingManager Instance;
    [SerializeField] GameObject _rocketPrefab, _crossPrefab, _dotPrefab;
    [SerializeField] Transform playerParent;
    [SerializeField] Transform aiParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake() => Instance = this;
    // Update is called once per frame
    void Update()
    {
        
    }
    public int ShootTile(float[] coordinates, bool player)
    {
        float x = coordinates[0];
        float y = coordinates[1];

        RaycastHit2D[] results = Physics2D.RaycastAll(new Vector3(x,y,0), Vector2.zero);
        foreach (var result in results)
        {
            if (result && result.collider.gameObject.TryGetComponent(out Tile tileComp))
            {
                if(player)
                {
                    if(tileComp.aiShip)
                    {
                        tileComp.aiShip.hp -= 1;
                       
                        if (tileComp.aiShip.hp <= 0)
                        {
                            Instantiate(_crossPrefab, tileComp.gameObject.transform).transform.parent = aiParent;
                            tileComp.aiShip = null;
                            return 1;
                        }
                        else
                        {
                            Instantiate(_crossPrefab, tileComp.gameObject.transform).transform.parent = aiParent;
                            tileComp.aiShip = null;
                            return 0;
                        }
                        

                    }
                    Instantiate(_dotPrefab, tileComp.gameObject.transform).transform.parent = aiParent;
                    return -1;
                }
                else
                {
                    if (tileComp.playerShip)
                    {
                        tileComp.playerShip.hp -= 1;
                        
                        if (tileComp.playerShip.hp <= 0)
                        {
                            Instantiate(_crossPrefab, tileComp.gameObject.transform).transform.parent = playerParent;
                            tileComp.playerShip = null;
                            return 1;
                        }
                        else {
                            Instantiate(_crossPrefab, tileComp.gameObject.transform).transform.parent = playerParent;
                            tileComp.playerShip = null;
                            return 0;
                        }
                       
                    }
                    Instantiate(_dotPrefab, tileComp.gameObject.transform).transform.parent = playerParent;
                    return -1;
                }
            }
        }
        return -1;
    }

}


