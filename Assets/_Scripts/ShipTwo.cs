using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTwo : ShipBase
{   

    // Start is called before the first frame update
    void Start()
    {
        hp = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void AddOffset()
    {
        base.AddOffset();
        Debug.Log("translated ship");
        gameObject.transform.Translate(0, 0.5f, 0);
    }
}
