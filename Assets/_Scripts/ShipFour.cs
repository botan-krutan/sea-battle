using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFour : ShipBase
{
    // Start is called before the first frame update
    void Start()
    {
        hp = 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void AddOffset()
    {
        base.AddOffset();
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 1.35f, gameObject.transform.localPosition.z);
    }
}
