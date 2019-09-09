using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;


public class SingleButtonBehaviour : NetworkBehaviour
{
    [SyncVar]
    public float neededTriggers = 1f;
    [SyncVar]
    private float currentTriggers = 0f;

    public TextMesh Text;


    public GameObject Door;

    void Update()
    {
        if (currentTriggers >= neededTriggers)
        {
            Door.GetComponent<DoorMover>().Move();
        }
            Text.text = currentTriggers + " / " + neededTriggers;
    }

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box")
        {
            currentTriggers++;
        }
    }

    [Server]
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box")
        {
            currentTriggers--;
        }
    }


}
