using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Tile : MonoBehaviour
{
    public ShipBase playerShip;
    public ShipBase aiShip;
    public Color occupiedColor;

    // Start is called before the first frame update
    void Start()
        
    {   
        // Tile scaling animation
        //transform.DOScale(1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (aiShip != null || playerShip != null)
        {
            gameObject.GetComponent<SpriteRenderer>().color = occupiedColor;
        }
        else gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
    }
}
