using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InputManager : MonoBehaviour
{
    ShipBase selectedShip;
    public static InputManager Instance;
    public Transform playerGroup;
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
                            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            selectedShip.PlaceShip(mousePos.x, mousePos.y);
                            selectedShip = null;
                        }

                        break;
                    case GameManager.GameState.AiArrange:
                        break;
                    case GameManager.GameState.PlayerTurn:
                        break;
                    case GameManager.GameState.AiTurn:
                        break;
                    case GameManager.GameState.PlayerWin:
                        break;
                    case GameManager.GameState.PlayerLoose:
                        break;
                
            }
        }
    }

    public RaycastHit2D MouseRaycast()
    {
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }
}
