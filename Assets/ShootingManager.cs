using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public static ShootingManager Instance;
    [SerializeField] GameObject _rocketPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake() => Instance = this;
    // Update is called once per frame
    void Update()
    {
        
    }
    public int ShootTile(float[] coordinates)
    {
        float x = coordinates[0];
        float y = coordinates[1];
        RaycastHit2D[] results = Physics2D.RaycastAll(new Vector3(x,y,0), Vector2.zero);
        foreach (var result in results)
        {
            if (result)
            {
                GameObject tile = result.collider.gameObject;
                if (result.collider.gameObject.GetComponent<Tile>())
                {
                    Rocket rocket = Instantiate(_rocketPrefab).GetComponent<Rocket>();
                    rocket.MoveAtTile(tile);
                    break;
                }
            }
        }

                return -1;
    }
}
