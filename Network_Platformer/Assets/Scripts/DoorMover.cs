using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMover : MonoBehaviour
{
    public Animator m_Animator;

    public void Move()
    {
        Debug.Log("Animation Played");
        m_Animator.SetTrigger("OpenDoor");

    }
}
