using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;
    private int desiredLane = 1;//0:left, 1:middle, 2:right
    public float laneDistance = 2.5f;//The distance between tow lanes
    public float jumpForce = 11; 
    public float gravity = -20;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    private bool isGrounded;
    private bool isJumping;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
       direction.z = forwardSpeed;

        if (!PlayerManager.isGameStarted)
            return;
        animator.SetBool("isGameStarted", true);
       
        controller.Move(direction*Time.fixedDeltaTime);

        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }

       if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        if (SwipeManager.swipeDown)
        {
            StartCoroutine(Slide());
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * laneDistance;

        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
                if (moveDir.sqrMagnitude < diff.magnitude)
                    controller.Move(moveDir);
                else
                controller.Move(diff);
        }
        
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundLayer);
       
        
        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isGrounded", false);
            isJumping = false;
            animator.SetBool("isFalling", false);
            if (SwipeManager.swipeUp)
        {
                direction.y = jumpForce;
                animator.SetBool("isJumping", true);
                isJumping = true;
        }
        } else {
            direction.y += gravity * Time.deltaTime;
            animator.SetBool("isGrounded", false);
            isGrounded = false;
            if((isJumping && direction.y <0)||direction.y <-2)
           {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping",false);
            isJumping = false;
           }
            }
        
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
          return;  
    }

    private IEnumerator Slide()
    {
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0,-0.5f,0);
        controller.height = 1;
        yield return new WaitForSeconds(1.3f);
        controller.center = new Vector3(0,0,0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.GameOver = true;
            forwardSpeed = 0;
            direction.y = 0;
            gravity = -100;
        }
    }
}

