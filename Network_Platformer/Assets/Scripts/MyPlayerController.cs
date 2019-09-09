using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;

public class MyPlayerController : NetworkBehaviour
{
    
    public float movementSpeed = 5.0f;
    public float turnSpeed = 45.0f;
    public float jumpForce = 5f;

    private NetworkStartPosition[] spawnPoints;
    private NetworkManager networkManager;
    private bool isGrounded = false;
    private bool isJumped = false;

    Rigidbody localRigidbody;
    Vector3 appliedForce;

    void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        else
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();

        networkManager = NetworkManager.singleton;

        localRigidbody = GetComponent<Rigidbody>();

        appliedForce = new Vector3(0f, jumpForce, 0f);
      
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumped = true;
            isGrounded = false;
        }
        
    }

    void FixedUpdate()
    {
        float turnAmount = CrossPlatformInputManager.GetAxis("Horizontal");
        float moveAmount = CrossPlatformInputManager.GetAxis("Vertical");


        Vector3 deltaTranslation = transform.position + transform.forward * movementSpeed * moveAmount * Time.deltaTime;
        localRigidbody.MovePosition(deltaTranslation);

        Quaternion deltaRotation = Quaternion.Euler(turnSpeed * new Vector3(0, turnAmount, 0) * Time.deltaTime);
        localRigidbody.MoveRotation(localRigidbody.rotation * deltaRotation);

        if(isJumped)
        {
            AddJumpForce();
            isJumped = false;
        }


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveGame();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    /// <summary>
    /// Applies an upward force to the Rigidbody of the Player
    /// </summary>
    void AddJumpForce()
    {
        localRigidbody.AddForce(appliedForce, ForceMode.Impulse);
    }
   


    /// <summary>
    /// Disconnects the player from the server
    /// </summary>
    public void LeaveGame()
    {
        Debug.Log(this.gameObject + " left the game");
        NetworkManager.singleton.StopClient();
    }

    /// <summary>
    /// Respawns the player
    /// </summary>
    [ClientRpc]
    public void RpcRespawn()
    {
        if (isLocalPlayer)
        {

            Vector3 spawnPoint = Vector3.zero;

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            transform.position = spawnPoint;
        }
    }
}