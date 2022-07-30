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
        gameObject.transform.Translate(new Vector3(0, 1.35f, 0));
    }
}
