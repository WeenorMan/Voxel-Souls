using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
   public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;
    Animator anim;

    public float speed;
    public float gravity;
    public float groundDistance = 0.4f;
    public float jumpHeight;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 velocity;
    bool isGrounded;
    bool isJumping;

    
    private void Start()
    {
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        print("isgrounded=" + isGrounded);
        print("yvel=" + velocity.y);

        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        isGrounded = controller.isGrounded;

        PlayerMovement();
    }

    public void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }


        //check for player walking
        float s = controller.velocity.magnitude;

        float moveSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        if (moveSpeed > 0.1f)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);

        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("run", true);
            speed = 8.5f;
        }
        else
        {
            anim.SetBool("run", false);
            speed = 5.93f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = 8;// Mathf.Sqrt(jumpHeight * -2f);
            anim.SetBool("isGrounded", true);
            anim.SetBool("idle", false);
        }
        else
        {
            //anim.SetBool("isGrounded", false);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(Input.GetKey(KeyCode.C) && isGrounded)
        {
            anim.SetBool("crouch", true);
        }
        else
        {
            anim.SetBool("crouch", false);
        }
    }


    



}
