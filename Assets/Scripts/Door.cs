using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    BoxCollider2D collider2d;

    private void Start()
    {
        anim = GetComponent<Animator>();
        collider2d = GetComponent<BoxCollider2D>();

        GameManager.instance.IsExitDoor(this);

        collider2d.enabled = false;
    }

    public void OpenDoor()//Game Manager 调用
    {
        anim.Play("door_open");
        collider2d.enabled = true;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.GameToNextLevel();
        }
    }
}
