using System.Collections;
using UnityEngine;

public class Control : MonoBehaviour
{
    private bool noKeyDown = false;

    private Vector2 vec;
    private Vector2 sightVec;

    Animator animator;
    Rigidbody2D rigidbody;
    Transform entity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        entity = this.gameObject.transform;
    }

    private void Update()
    {
        if (!noKeyDown)
            moveKeyPress();
    }

    private void moveKeyPress()
    {
        if (isOtherMove())
        {
            // 키 타입에 따른 입력방지
            return;
        }

        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");

        animator.SetInteger("axisH", (int)(sightVec.x));
        animator.SetInteger("axisV", (int)(sightVec.y));
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = vec.normalized * speed;
    }

    private bool isOtherMove()
    {
        return (Input.GetKey(Option.getKey(Key.up)) ||
                Input.GetKey(Option.getKey(Key.down)) ||
                Input.GetKey(Option.getKey(Key.left)) ||
                Input.GetKey(Option.getKey(Key.right)));
    }

}