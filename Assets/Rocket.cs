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
    public void MoveAtTile(GameObject Tile)
    {
        transform.DOMove(Tile.transform.position, 2);
    }
}
