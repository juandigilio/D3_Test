using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private InputManager inputManager;
    private PlayerController playerController;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterInputManager(InputManager inputManager)
    {
        this.inputManager = inputManager;
    }

    public void RegisterPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public InputManager GetInputManager()
    {
        return inputManager;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }
}
