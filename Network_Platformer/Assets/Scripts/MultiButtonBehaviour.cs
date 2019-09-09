using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;


public class MultiButtonBehaviour : NetworkBehaviour {

    [SyncVar]
    public float neededTriggers = 1f;
    [SyncVar]
    private float currentTriggers = 0f;

    private ButtonTrigger Button01Triggered;
    private ButtonTrigger Button02Triggered;

    public TextMesh Text;
    public GameObject Door;
    public GameObject Button01;
    public GameObject Button02;

    private void Start()
    {
        Button01Triggered = Button01.GetComponent<ButtonTrigger>();
        Button02Triggered = Button02.GetComponent<ButtonTrigger>();       
    }


    void Update()
    {
        Text.text = currentTriggers + " / " + neededTriggers;

        if (!isServer)
        {
            return;
        }



        if (!Button01Triggered.isTriggered && !Button02Triggered.isTriggered)
        {
            currentTriggers = 0;
        }
        else if (Button01Triggered.isTriggered && Button02Triggered.isTriggered)
        {
            currentTriggers = 2;
        }
        else
            currentTriggers = 1;


        if (Button01Triggered.isTriggered && Button02Triggered.isTriggered)
        {
            RpcOpenDoor();
        }
    }

    [ClientRpc]
    private void RpcOpenDoor()
    {
        Door.GetComponent<DoorMover>().Move();
    }

}
