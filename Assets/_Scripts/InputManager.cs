using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InputManager : MonoBehaviour
{
    ShipBase selectedShip;
    public static InputManager Instance;
    public Transform playerGroup;
    public bool alreadyShooted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake() => Instance = this;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D result = MouseRaycast();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            switch (GameManager.Instance.gameState)
                {
                    case GameManager.GameState.PlayerArrange:
                        if (!selectedShip)
                        {   
                            if(result)
                            {
                                if (result.collider.gameObject.TryGetComponent<ShipBase>(out ShipBase shipBase))
                                {
                                    selectedShip = shipBase;
                                    shipBase.gameObject.transform.DOScale(1.3f, 0.5f);
                                    Debug.Log(selectedShip.name);
                                }
                            }

                        }
                        else
                        {
                            Debug.Log("Placing Object");
                            
                            selectedShip.PlaceShip(mousePos.x, mousePos.y);
                            selectedShip = null;
                        }

                        break;
                    case GameManager.GameState.PlayerTurn:
                        if (alreadyShooted) return;
                        float[] coordinates = new float[2];
                        coordinates[0] = mousePos.x;
                        coordinates[1] = mousePos.y;
                        ShootingManager.Instance.ShootTile(coordinates);
               
                        break;
                    case GameManager.GameState.AiTurn:
                        alreadyShooted = true;
                        break;
                
            }
        }
    }

    public RaycastHit2D MouseRaycast()
    {
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }
}
