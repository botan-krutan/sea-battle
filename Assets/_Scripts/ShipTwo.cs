using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTwo : ShipBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void AddOffset()
    {
        base.AddOffset();
        gameObject.transform.Translate(0, 0.5f, 0);
    }
}
