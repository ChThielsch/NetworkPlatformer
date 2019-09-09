using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;

public class ButtonTrigger : NetworkBehaviour
{
    public bool isTriggered;

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box")
        {
            isTriggered = true;
        }
    }

    [Server]
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box")
        {
            isTriggered = false;
        }
    }
}
