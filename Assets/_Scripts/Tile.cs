using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Tile : MonoBehaviour
{
    public ShipBase playerShip;
    public ShipBase aiShip;
    public Color occupiedColor;
    public ShipBase nearShip;
    // Start is called before the first frame update
    void Start()
        
    {   
        // Tile scaling animation
        //transform.DOScale(1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

/*        if (aiShip)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        }
        else if (nearShip)
        {
            GetComponent<SpriteRenderer>().color = occupiedColor;
        }

        else if (playerShip)
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
        else GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);*/
    }
}
