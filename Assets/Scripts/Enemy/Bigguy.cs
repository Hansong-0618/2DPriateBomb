using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bigguy : Enemy, IDamageable
{
    public Transform pickUpPoint;
    public float power;
    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
            anim.SetTrigger("hit");
        }
    }
    public void PickUpBomb()//Animation Event
    {
        if (targetPoint.CompareTag("Bomb"))
        {
            targetPoint.position = pickUpPoint.position;
            //targetPoint.gameObject.transform.position = pickUpPoint.position;

            targetPoint.SetParent(pickUpPoint);

            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            hasBomb = true;
        }
    }
    public void ThrowAway()//Animation Event
    {
        if (hasBomb)
        {
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            targetPoint.SetParent(transform.parent.parent);

            if (FindObjectOfType<PlayerController>().gameObject.transform.position.x - transform.position.x < 0)
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * power, ForceMode2D.Impulse);
            else
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * power, ForceMode2D.Impulse);
        }
        hasBomb = false;
    }

}
