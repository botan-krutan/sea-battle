using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThree : ShipBase
{ 
    
    // Start is called before the first frame update
    void Start()
    {
        hp = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void AddOffset()
    {
        base.AddOffset();
        gameObject.transform.Translate(0, 1, 0);
    }
}
