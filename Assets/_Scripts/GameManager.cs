using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public static GameManager Instance;
    public UnityEvent<GameState> OnStateUpdated = new UnityEvent<GameState>();
    [SerializeField] GameObject playerGroup, aiGroup;
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
}
