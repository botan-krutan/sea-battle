using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Button _autoPlaceShipsButton;
    [SerializeField] private Button _playButton;

    ShipBase selectedShip;
    public static InputManager Instance;
    public bool alreadyShooted;

    public void InitInputManager()
    {
        _autoPlaceShipsButton.onClick.AddListener(TileManager.Instance.AutoPlaceShipsButtonPressHandler);
        _playButton.onClick.AddListener(PlayButtonPressHandler);
        _playButton.gameObject.SetActive(false);
        _autoPlaceShipsButton.gameObject.SetActive(true);
        TileManager.OnBattlefieldChanged += SwitchButtons;
    }
    void Awake() => Instance = this;

    void Update()
    {
        if (selectedShip != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            selectedShip.transform.position = pos;
        }

        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            GameManager.debugMode = !GameManager.debugMode;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D result = MouseRaycast();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            switch (GameManager.Instance.gameState)
            {
                case GameManager.GameState.PlayerArrange:
                    if (!selectedShip && result != null)
                    {
                        RaycastHit2D[] results = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        foreach (var item in results)
                        {
                            if (item.collider.gameObject.TryGetComponent<ShipBase>(out ShipBase shipBase))
                            {
                                selectedShip = shipBase;
                                TileManager.Instance.RemoveShipOnBattlefield(selectedShip);
                                Debug.Log(selectedShip.name);
                            }
                        }
                    }
                    else
                    {
                        bool isCoordinatesReceived = MousePosToBattlefieldPos(mousePos.x, mousePos.y, out int x, out int y);

                        if (isCoordinatesReceived &&
                            TileManager.Instance.CanPlaceShip(selectedShip, x, y, Battlefield.Player))
                        {
                            Debug.Log("Placing Object");
                            TileManager.Instance.PlaceShip(selectedShip, x, y, Battlefield.Player);
                            selectedShip = null;
                        }
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

        if (Input.GetMouseButtonDown(1) && selectedShip != null && GameManager.Instance.gameState == GameManager.GameState.PlayerArrange)
        {
            selectedShip.RotateShip();
        }
    }
    private void PlayButtonPressHandler()
    {
        _autoPlaceShipsButton.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(false);
        GameManager.Instance.StartGame();
    }

    private void SwitchButtons(bool isAllShipArranged)
    {
        _autoPlaceShipsButton.gameObject.SetActive(!isAllShipArranged);
        _playButton.gameObject.SetActive(isAllShipArranged);
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

    private bool MousePosToBattlefieldPos(float mousePosX, float mousePosY, out int x, out int y)
    {
        x = -1;
        y = -1;

        if (mousePosX > 9.5 && mousePosY > 9.5 && mousePosX < -0.5 && mousePosY < -0.5) return false;

        RaycastHit2D[] results = Physics2D.RaycastAll(new Vector3(mousePosX, mousePosY, 0), Vector2.zero);

        foreach (var result in results)
        {
            if (result.collider.gameObject.TryGetComponent(out Tile tile))
            {
                x = tile.x;
                y = tile.y;
                return true;
            }
        }

        return false;
    }
}
