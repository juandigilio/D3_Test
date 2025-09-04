using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.Timeline.DirectorControlPlayable;


public class InputManager : MonoBehaviour
{
    [SerializeField] private string moveAction = "Move";
    [SerializeField] private string jumpAction = "Jump";
    [SerializeField] private string shootAction = "Shoot";
    [SerializeField] private string nextWeaponAction = "NextWeapon";
    [SerializeField] private string previousWeaponAction = "PreviousWeapon";

    //[SerializeField] private string inGameActionMap = "InGame";
    //[SerializeField] private string menuActionMap = "Menu";

    private PlayerInput playerInput;
    private PlayerController playerController;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing on this GameObject.");
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.RegisterInputManager(this);
    }

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not registered in GameManager.");
        }

        LoadActions();
    }

    private void Update()
    {

    }

    private void Move(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            playerController.SetInputDirection(callbackContext.ReadValue<Vector2>());
        }
        if (callbackContext.performed)
        {
            playerController.SetInputDirection(callbackContext.ReadValue<Vector2>());
        }
        if (callbackContext.canceled)
        {
            playerController.SetInputDirection(Vector2.zero);
        }
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            playerController.Jump();
        }
    }

    private void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            playerController.SetShooting(true);
        }
        if (callbackContext.canceled)
        {
            playerController.SetShooting(false);
        } 
    }

    private void NextWeapon(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            playerController.NextWeapon();
        }
    }

    private void PreviousWeapon(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            playerController.PreviousWeapon();
        }
    }

    private void LoadActions()
    {
        playerInput.ActivateInput();

        if (playerInput != null)
        {
            playerInput.currentActionMap.FindAction(moveAction).started += Move;
            playerInput.currentActionMap.FindAction(moveAction).performed += Move;
            playerInput.currentActionMap.FindAction(moveAction).canceled += Move;

            playerInput.currentActionMap.FindAction(jumpAction).started += Jump;

            playerInput.currentActionMap.FindAction(shootAction).started += Shoot;
            playerInput.currentActionMap.FindAction(shootAction).canceled += Shoot;

            playerInput.currentActionMap.FindAction(nextWeaponAction).started += NextWeapon;
            playerInput.currentActionMap.FindAction(previousWeaponAction).started += PreviousWeapon;
        }
    }
}
