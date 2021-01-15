using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitpoint : MonoBehaviour
{
    public bool bombAvilble;
    public bool playerAvilble;
    int dir;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (transform.position.x > collider.transform.position.x)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<IDamageable>().GetHit(1);
            if (collider.CompareTag("Player") && playerAvilble)
            {
                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * 2, ForceMode2D.Impulse);
            }
        }

        if (collider.CompareTag("Bomb") && bombAvilble)
        {
            collider.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * 2,ForceMode2D.Impulse);
        }
    }
}
