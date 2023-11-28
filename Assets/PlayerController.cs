using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }

            if (movementInput.y < 0)
            {
                animator.SetBool("bIsMovingDown", success);
            }
            else
            {
                animator.SetBool("bIsMovingDown", false);
            }

            if (movementInput.y > 0)
            {
                animator.SetBool("bIsMovingUp", success);
            }
            else
            {
                animator.SetBool("bIsMovingUp", false);
            }

            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
                animator.SetBool("bIsMovingSide", success);

            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
                animator.SetBool("bIsMovingSide", success);

            }
            else
            {
                animator.SetBool("bIsMovingSide", false);

            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {

        movementInput = movementValue.Get<Vector2>();
    }
}
