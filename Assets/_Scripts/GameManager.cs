using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public static GameManager Instance;
    public UnityEvent<GameState> OnStateUpdated = new UnityEvent<GameState>();
    [SerializeField] GameObject playerGroup, aiGroup, continueMessage, gameEnd, endText;
    public static bool debugMode;
    public static bool canContinue;
    private void Awake() => Instance = this;

    private void Start()
    {
        TileManager.Instance.InitTileManager();
        AiManager.Instance.InitAiManager();
        InputManager.Instance.InitInputManager();
        UpdateState(GameState.PlayerArrange);
    }

    public void UpdateState(GameState newState)
    {
        gameState = newState;
        switch (gameState)
        {
            case GameState.PlayerTurn:
                playerGroup.SetActive(false);
                aiGroup.SetActive(true);
                break;
            case GameState.AiTurn:
                InputManager.Instance.alreadyShooted = false;
                playerGroup.SetActive(true);
                aiGroup.SetActive(false);
                break;
            case GameState.PlayerWin:
                EndGame(true);
                break;
            case GameState.PlayerLoose:
                EndGame(false);
                break;
        }

        OnStateUpdated.Invoke(newState);
    }

    public enum GameState
    {
        PlayerArrange,
        AiArrange,
        PlayerTurn,
        AiTurn,
        PlayerWin,
        PlayerLoose
    }
    public void ContinueMessage(bool activate)
    {
        continueMessage.SetActive(activate);
        canContinue = activate;
    }

    public void EndGame(bool playerWins)
    {
        gameEnd.SetActive(true);
        if (playerWins) endText.GetComponent<TextMeshProUGUI>().text = "PLAYER WINS!";
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void StartGame()
    {
        UpdateState(GameState.PlayerTurn);
    }
}
