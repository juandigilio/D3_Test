using UnityEngine;

public class Enemy_Basic : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    
    private PlayerController playerController;
    private Rigidbody2D rigidBody;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        playerController = GameManager.Instance.GetPlayerController();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        FollowCharacter();
    }

    private void FollowCharacter()
    {
        Vector2 direction = (playerController.transform.position - transform.position).normalized;

        rigidBody.AddForce(direction * Time.deltaTime);
    }
}
