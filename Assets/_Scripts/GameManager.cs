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
    public bool debugMode, canContinue = false;
    private void Awake() => Instance = this;

    public void UpdateState(GameState newState)
    {
        gameState = newState;
        switch(gameState)
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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            debugMode = !debugMode;
        }
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
}
