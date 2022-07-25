using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public static GameManager Instance;
    public UnityEvent<GameState> OnStateUpdated = new UnityEvent<GameState>();

    private void Awake() => Instance = this;

    public void UpdateState(GameState newState)
    {
        gameState = newState;
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
