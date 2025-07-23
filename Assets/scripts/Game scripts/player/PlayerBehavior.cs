using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 900f;
    public float jumpForce = 2000f;
    private Vector3 move;
    private Vector3 v;

    private bool canJump = false;
    private bool isTouching = true;
    private float noContactTime = 0f;
    private Vector3 PositionOfLastGroundStandON = Vector3.zero;
    public bool CanJump => canJump;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsGround(collision))
        {
            canJump = true;
            Vector3 gravityDir = Physics.gravity.normalized;
            Vector3 velocity = rb.velocity;
            // 将速度向重力方向投影
            Vector3 gravityVelocity = Vector3.Project(velocity, gravityDir);
            // 减去这部分速度，只保留与重力垂直的速度
            rb.velocity = velocity - gravityVelocity;

            PositionOfLastGroundStandON = collision.transform.position;

        }
        if (IsChaser(collision))
        {
            FindObjectOfType<GameManager>().EndGame();
        }
        isTouching = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsGround(collision))
        {
            canJump = true;
        }
        if (IsChaser(collision))
        {
            FindObjectOfType<GameManager>().EndGame();
        }
        isTouching = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsGround(collision))
        {
            canJump = false;
        }
        isTouching = false;
    }

    private bool IsGround(Collision collision)
    {
        return collision.gameObject.CompareTag("Ground");
    }

    private bool IsChaser(Collision collision)
    {
        return collision.gameObject.CompareTag("chaser");
    }

    void Start()
    {
        move = new Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            move = transform.position + transform.right * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(move);
        }
        if (Input.GetKey(KeyCode.A))
        {
            move = transform.position - transform.right * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(move);
        }
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            rb.AddForce(-Physics.gravity.normalized * jumpForce, ForceMode.Impulse);
        }
    }

    void Update()
    {
        if (!isTouching)
        {
            noContactTime += Time.deltaTime;
            if (noContactTime >= 3f && Vector3.Distance(transform.position, PositionOfLastGroundStandON) >= 3400)
            {
                FindAnyObjectByType<GameManager>().EndGame();
                noContactTime = 0;
            }
        }
        else
        {
            noContactTime = 0;
        }
        //Debug.Log(Vector3.Distance(transform.position, PositionOfLastGroundStandON));
    }
}
