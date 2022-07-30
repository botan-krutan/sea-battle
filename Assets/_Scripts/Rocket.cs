using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int MoveAtTile(GameObject Tile)
    {
        Tile tileComp = Tile.GetComponent<Tile>();
        //MoveRocket(Tile);
        transform.DOMove(Tile.transform.position, 2).OnComplete(() => 
        {
            Destroy(Tile);
            Destroy(gameObject);
        });
/*        if (tileComp.occupiedShip != null)
        {
            tileComp.occupiedShip = null;
            return 0;
        }*/

        return -1;

    }

}
