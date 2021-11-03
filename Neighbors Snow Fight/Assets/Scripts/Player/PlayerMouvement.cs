using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    [SerializeField] private Transform head;

    public CharacterController controller;
    Animator animator;
    public float baseSpeed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 2f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    private float speed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        speed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        

        Vector3 move = transform.right * z + transform.forward * -x;

        if (move.z != 0 && head.localRotation.y != 0) {
            transform.Rotate(Vector3.up * head.localRotation.y * 100);
            head.transform.localRotation = Quaternion.identity;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            animator.SetBool("IsWalking", true);
        } else if (Input.GetKeyUp(KeyCode.Q)) {
            animator.SetBool("IsWalking", false);
        }

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void SetSpeed(float pourcentage)
    {
        speed = baseSpeed * pourcentage;
    }

}
