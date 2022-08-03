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
                                RaycastHit2D[] results = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                            foreach (var item in results)
                            {
                                if (item.collider.gameObject.TryGetComponent<ShipBase>(out ShipBase shipBase))
                                {
                                    selectedShip = shipBase;
                                    shipBase.gameObject.transform.DOScale(1.3f, 0.5f);
                                    Debug.Log(selectedShip.name);
                                }
                            }

                            }

                        }
                        else
                        {
                            Debug.Log("Placing Object");
                            //selectedShip.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
                            selectedShip.PlaceShip(mousePos.x, mousePos.y, selectedShip.hp, selectedShip.curDir, true);
                            selectedShip = null;
                        }

                        break;
                    case GameManager.GameState.PlayerTurn:
                        if (alreadyShooted) return;
                        float[] coordinates = new float[2];
                        coordinates[0] = mousePos.x;
                        coordinates[1] = mousePos.y;
                        int res = 0;
                        res = ShootingManager.Instance.ShootTile(coordinates, true);
                    if (res == -1)
                    {
                        StartCoroutine(UpdateOn3Secs());
                        alreadyShooted = true;
                    }
                    else
                    {
                        alreadyShooted = false;
                    }
                        break;
                    case GameManager.GameState.AiTurn:
                        alreadyShooted = false;
                        break;
                
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GameManager.Instance.gameState == GameManager.GameState.PlayerArrange)
            {

             RaycastHit2D[] results = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                foreach (var item in results)
                {
                    if (item.collider.gameObject.TryGetComponent(out Tile tile))
                    {
                        if (tile.playerShip != null)
                        {
                            ShipBase ship = tile.playerShip;
                            switch (ship.curDir)
                            {
                                case "right":
                                    ship.PlaceShip(mousePos.x, mousePos.y, ship.hp, "down", true);
                                    break;
                                case "down":
                                    ship.PlaceShip(mousePos.x, mousePos.y, ship.hp, "left", true);
                                    break;
                                case "left":
                                    ship.PlaceShip(mousePos.x, mousePos.y, ship.hp, "up", true);
                                    break;
                                case "up":
                                    ship.PlaceShip(mousePos.x, mousePos.y, ship.hp, "right", true);
                                    break;
                            }
                        }


                    }
                }

            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.gameState == GameManager.GameState.PlayerArrange)
            {
                GameManager.Instance.UpdateState(GameManager.GameState.AiArrange);
            }
        }
    }
    IEnumerator UpdateOn3Secs()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.UpdateState(GameManager.GameState.AiTurn);
    }
    public RaycastHit2D MouseRaycast()
    {
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }
}
