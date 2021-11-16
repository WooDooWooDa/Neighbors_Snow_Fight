using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : NetworkBehaviour
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
    [SyncVar]
    private float speed;
    private bool isWalking;

    public void SetSpeed(float pourcentage)
    {
        speed = baseSpeed * pourcentage;
    }

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
        if (!isLocalPlayer) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        UpdateAnimator();

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * z + transform.forward * -x;
        isWalking = (Vector3.Distance(move, Vector3.zero) > 0);

        if (move.z != 0 && head.localRotation.y != 0) {
            transform.Rotate(Vector3.up * head.localRotation.y * 100);
            head.transform.localRotation = Quaternion.identity;
        }

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            Jump();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (isGrounded && !isWalking) {
            animator.SetBool("Walk", false); //return to idle
            animator.SetBool("Sprint", false);
        } else if (isWalking && speed == baseSpeed) {
            animator.SetBool("Sprint", false);
            animator.SetBool("Walk", true);
        } else if (isWalking && speed != baseSpeed) {
            animator.SetBool("Walk", false);
            animator.SetBool("Sprint", true);
        }
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

}
