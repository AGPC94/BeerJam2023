using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPController : MonoBehaviour
{
    //TODO

    #region Variables
    [Header("Movement")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool canMove = true;
    [SerializeField] float walkSpeed;
    [SerializeField] float rotationSpeed = 720;
    [SerializeField] float ySpeedGrounded = -0.5f;

    [Header("Other")]
    [SerializeField] float pushForce;
    [SerializeField] float timeToRespawn;
    [SerializeField] float noiseRange;

    [Header("Run")]
    [SerializeField] float runSpeed;
    [SerializeField] bool isRunning;

    [Header("Slope")]
    [SerializeField] float raySlopeLength = 2;
    [SerializeField] LayerMask whatIsGround;

    [Header("Jump")]
    [SerializeField] float timeToJumpApex = .4f;
    [SerializeField] float maxJumpHeight = 4;
    [SerializeField] float minJumpHeight = 1;
    [SerializeField] float jumpBufferTime = 0.2f;
    float? lastJumpedTime;
    [SerializeField] float coyoteTime = 0.2f;
    float? lastGroundedTime;
    [SerializeField] float fallSpeedMax = -10f;

    [Header("Physics")]
    [SerializeField] float mass;
    [SerializeField] float impactMagnitudeMin = 0.2f;
    float impactSmoothTimeCurrent;

    //Jump
    float maxJumpVelocity;
    float minJumpVelocity;

    //Movement
    Vector3 moveInput;
    Vector3 moveDir;
    Vector3 velocity;
    float moveMagnitude;

    //Gravity
    float gravity;
    float ySpeed;

    //Components
    Camera cam;
    CharacterController controller;
    Animator animator;
    #endregion

    void Awake()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = -((2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2));
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = FixedJumpHeight(minJumpHeight);
    }

    float FixedJumpHeight(float height)
    {
        return Mathf.Sqrt(2 * Mathf.Abs(gravity) * height);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Inputs();
            Movement();
            Jump();
            Rotate();

            if (Vector3.Distance(transform.position, GameManager.instance.chef.transform.position) <= noiseRange
                &&
                isRunning && moveMagnitude >= 0.1f
                && GameManager.instance.chef.state == State.PATROL)
            {
                GameManager.instance.chef.ChangeState(State.SUSPECT, transform);
            }
        }

        Velocity();

        Collisions();

        Animations();
        //Check in inspector
        isGrounded = controller.isGrounded;

    }

    void Animations()
    {
        animator.SetBool("isGrounded", controller.isGrounded);
        animator.SetFloat("magnitude", moveMagnitude);
    }

    public void Die()
    {
        canMove = false;
        animator.SetTrigger("die");
        controller.enabled = false;
        Invoke("Respawn", timeToRespawn);
    }

    void Respawn()
    {
        Debug.Log("Respawn()");
        controller.transform.position = GameManager.instance.checkPoint;
        controller.enabled = true;
        canMove = true;
    }


    void Inputs()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveInput = new Vector3(h, 0, v);

        //Jump Buffer
        if (Input.GetButtonDown("Jump"))
            lastJumpedTime = Time.time;
    }

    void Movement()
    {
        moveDir = moveInput;

        //Magnitud entre 0 y 1 para que la velocidad sea distinta según cuanto inclinemos el stick
        moveMagnitude = Mathf.Clamp01(moveDir.magnitude);

        //Normalizar: hacer que la magnitud sea 1 para que tenga la misma velocidad en diagonal
        moveDir.Normalize();

        //Suma al vector de movimiento el eje de ángulo de arriba de la cámara para que se mueve según su rotación
        moveDir = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up) * moveDir;

        if (Input.GetButton("Fire1"))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        //moveSpeed = Mathf.SmoothDamp(moveSpeed * moveMagnitude, maxSpeed, ref velocitySmoothing, timeToMaxSpeed);
    }

    void Jump()
    {
        //Coyote Time
        if (controller.isGrounded)
            lastGroundedTime = Time.time;

        if (IsPossibleToJump())
        {
            ySpeed = maxJumpVelocity;
            animator.SetTrigger("jump");
        }

        if (Input.GetButtonUp("Jump") && velocity.y > minJumpVelocity)
            ySpeed = minJumpVelocity;


    }

    void Velocity()
    {
        ySpeed += gravity * Time.deltaTime;
        if (ySpeed <= -fallSpeedMax && !controller.isGrounded)
            ySpeed = -fallSpeedMax;

        if (canMove)
        {
            if (isRunning && controller.isGrounded)
                velocity = moveDir * runSpeed * moveMagnitude;
            else
                velocity = moveDir * walkSpeed * moveMagnitude;

        }
        else
            velocity = Vector3.zero;

        velocity = AdjustVelocityToSlope(velocity);
        velocity.y += ySpeed;

        controller.Move(velocity * Time.deltaTime);

    }

    void Rotate()
    {
        if (moveInput != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    bool IsMaxTime(float? lastTime, float maxTime)
    {
        return Time.time - lastTime <= maxTime;
    }

    bool IsJumpBuffer()
    {
        return IsMaxTime(lastJumpedTime, jumpBufferTime);
    }

    bool IsCoyoteTime()
    {
        return IsMaxTime(lastGroundedTime, coyoteTime) && ySpeed <= 0;
    }

    bool IsPossibleToJump()
    {
        bool condition = IsCoyoteTime() && IsJumpBuffer();
        if (condition)
            ResetBufferAndCoyoteTimes();
        return condition;
    }

    void ResetJumpTime()
    {
        lastJumpedTime = null;
    }

    void ResetCoyoteTime()
    {
        lastGroundedTime = null;
    }

    void ResetBufferAndCoyoteTimes()
    {
        ResetJumpTime();
        ResetCoyoteTime();
    }

    void Collisions()
    {
        if (controller.collisionFlags == CollisionFlags.Above)
        {
            ySpeed = -5;
        }

        if (controller.collisionFlags == CollisionFlags.Below)
        {
            ySpeed = ySpeedGrounded;
        }

    }

    Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, raySlopeLength, whatIsGround) && controller.isGrounded)
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * velocity;

            if (slopeRotation.y < 0)
                return slopeRotation;
        }

        return velocity;

    }

    Vector3 SlopeSlideVelocity()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, raySlopeLength, whatIsGround))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            if (angle >= controller.slopeLimit)
            {
                return Vector3.ProjectOnPlane(new Vector3(0, ySpeed, 0), hit.normal);
            }
        }
        return Vector3.zero;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * raySlopeLength);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, noiseRange);

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Bottle"))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            rb.velocity = hit.moveDirection * pushForce;
        }
    }

}