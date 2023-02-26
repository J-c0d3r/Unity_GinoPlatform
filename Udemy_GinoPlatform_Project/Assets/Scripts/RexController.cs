using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RexController : MonoBehaviour
{
    private bool isRight;
    [SerializeField] private float speed;
    [SerializeField] private int state;
    [SerializeField] private float distanceMinFollow;
    [SerializeField] private float distanceMinAttack;

    private Rigidbody2D rexRB;
    private SpriteRenderer rexSR;
    private CapsuleCollider2D rexCC;
    private BoxCollider2D rexBC;
    private Animator rexAnim;
    private Transform player;

    void Start()
    {
        isRight = true;
        rexRB = gameObject.GetComponent<Rigidbody2D>();
        rexSR = gameObject.GetComponent<SpriteRenderer>();
        rexCC = gameObject.GetComponent<CapsuleCollider2D>();
        rexBC = gameObject.GetComponent<BoxCollider2D>();
        rexAnim = gameObject.GetComponent<Animator>();
        player = FindObjectOfType<MoveCharacter>().transform;

        rexAnim.SetBool("walk", state == 0 ? true : false);
    }


    void Update()
    {
        switch (state)
        {
            case -1:
                StopAllMoviment();
                break;

            case 0:
                MoveSideSide();
                break;

            case 1:
                FollowAttackPlayer();
                break;
        }

    }

    private void StopAllMoviment()
    {
        rexRB.velocity = new Vector2(0f, rexRB.velocity.y);
        rexCC.enabled = false;
        rexBC.enabled = false;
    }

    private void MoveSideSide()
    {
        if (isRight)
        {
            rexSR.flipX = true;
            rexCC.offset = new Vector2(1.56f, rexCC.offset.y);
            rexBC.offset = new Vector2(1.56f, rexBC.offset.y);
            rexRB.velocity = new Vector2(speed, rexRB.velocity.y);
        }
        else
        {
            rexSR.flipX = false;
            rexCC.offset = new Vector2(-1.56f, rexCC.offset.y);
            rexBC.offset = new Vector2(-1.56f, rexBC.offset.y);
            rexRB.velocity = new Vector2(-speed, rexRB.velocity.y);
        }
    }

    private void FollowAttackPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= distanceMinFollow)
        {
            if (distance <= distanceMinAttack)
            {
                rexAnim.SetBool("attack", true);
                rexAnim.SetBool("walk", false);
                rexRB.velocity = new Vector2(0f, rexRB.velocity.y);
            }
            else
            {
                rexAnim.SetBool("walk", true);
                rexAnim.SetBool("attack", false);
                Vector2 direction = player.position - transform.position;
                direction = direction.normalized;

                rexRB.velocity = new Vector2(direction.x * speed, rexRB.velocity.y);

                if (rexRB.velocity.x > 0)
                {
                    isRight = true;
                    rexSR.flipX = true;
                    rexCC.offset = new Vector2(1.56f, rexCC.offset.y);
                    rexBC.offset = new Vector2(1.56f, rexBC.offset.y);
                }
                else
                {
                    isRight = false;
                    rexSR.flipX = false;
                    rexCC.offset = new Vector2(-1.56f, rexCC.offset.y);
                    rexBC.offset = new Vector2(-1.56f, rexBC.offset.y);
                }

            }
        }
        else
        {
            rexAnim.SetBool("walk", false);
            rexAnim.SetBool("attack", false);
            rexRB.velocity = new Vector2(0f, rexRB.velocity.y);
        }
    }

    private void Attack()
    {
        if (isRight)
        {
            rexCC.offset = new Vector2(6.3f, rexCC.offset.y);
        }
        else
        {
            rexCC.offset = new Vector2(-6.3f, rexCC.offset.y);
        }
    }

    private void resetAttack()
    {
        if (isRight)
        {
            rexCC.offset = new Vector2(1.56f, rexCC.offset.y);
        }
        else
        {
            rexCC.offset = new Vector2(-1.56f, rexCC.offset.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("RexWalls"))
        {
            isRight = !isRight;
        }
    }

    public void Die()
    {
        state = -1;
        rexAnim.SetBool("die", true);
        Destroy(gameObject, 1.1f);
    }

}
