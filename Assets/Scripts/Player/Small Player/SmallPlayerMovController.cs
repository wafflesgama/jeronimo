using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class SmallPlayerMovController : MonoBehaviour
{
    public float deac;
    public float springStrength = 1;
    public float dampenStrength = 1;
    public float floatHeight = 1;
    public float minGroundHeight;
    public float maxAccel = 10;
    public LayerMask mask;
    public float castDist = 1f;
    public PlayerInputManager inputManager;
    public float goalVelocity;
    public float sneakVelocity;

    public float jumpForce;
    public int jumpFrames = 200;
    public int jumpStartCheckFrames = 200;
    public UEvent OnJump = new UEvent();
    public float jumpDownForce = 2f;

    public UEvent OnLand = new UEvent();

    public Vector3 vel;
    public Vector3 horizontalVel;
    public float horizontalVelMag;

    public bool isFrozen { get; private set; }
    public bool isFloatGrounded { get; private set; }
    public bool isGrounded { get; private set; }
    public bool isSprinting { get; private set; }

    bool isJumping;
    int jumpCounter;
    int jumpStartCounter;

    bool isLanding;


    public Rigidbody rb { get; private set; }
    RaycastHit groundHit;

    UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        inputManager.input_jump.Onpressed.Subscribe(eventHandler, () => jumpCounter = jumpFrames);
        inputManager.input_sprint.Onpressed.Subscribe(eventHandler, () => isSprinting = true);
        inputManager.input_sprint.Onreleased.Subscribe(eventHandler, () => isSprinting = false);
    }
    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }


    private void FixedUpdate()
    {
        CheckGround();

        CheckStartLanding();
        Jump();
        JumpDownForce();
        CheckEndLanding();

        Float();
        Move();

        AirbourneDecel();

        if (jumpCounter > 0)
            jumpCounter--;
    }

    private void CheckGround()
    {
        isFloatGrounded = Physics.Raycast(transform.position, -transform.up, out groundHit, castDist, mask, QueryTriggerInteraction.Ignore);
        isGrounded = isFloatGrounded ? groundHit.distance <= minGroundHeight : false;
    }

    private void Move()
    {
        if (!isGrounded || isFrozen) return;

        var move = inputManager.input_move.value;
        var moveRet = new Vector3(move.x, 0, move.y);
        var transformedMove = inputManager.playerCamera.transform.TransformDirection(moveRet);
        transformedMove.y = 0;

        //var velocity = goalVelocity;
        var velocity = isSprinting ? goalVelocity : sneakVelocity;
        var aceleration = transformedMove * velocity - rb.velocity;
        aceleration = Vector3.ClampMagnitude(aceleration, maxAccel);

        var force = rb.mass * (aceleration / Time.fixedDeltaTime);
        force.y = 0;

        rb.AddForce(force);
    }

    private void Float()
    {
        if (!isFloatGrounded || isJumping) return;

        Vector3 outsideVel = Vector3.zero;

        if (isFloatGrounded && groundHit.rigidbody != null)
            outsideVel = groundHit.rigidbody.velocity;

        float dirOwnVel = Vector3.Dot(-transform.up, rb.velocity);
        float dirOutsideVel = Vector3.Dot(-transform.up, outsideVel);



        float dirRelation = dirOwnVel - dirOutsideVel;
        float x = groundHit.distance - floatHeight;

        float spring = (x * springStrength) - (dirRelation * dampenStrength);

        rb.AddForce(-transform.up * rb.mass * spring);

        if (outsideVel != Vector3.zero)
        {
            groundHit.rigidbody.AddForceAtPosition(-transform.up * -springStrength, groundHit.point);
        }

    }



    private void Jump()
    {
        if (!isFloatGrounded || jumpCounter <= 0 || isJumping || isFrozen) return;

        jumpCounter = 0;
        isJumping = true;
        jumpStartCounter = jumpStartCheckFrames;
        OnJump.TryInvoke();

        rb.AddForce(Vector3.up * rb.mass * jumpForce, ForceMode.Impulse);
    }

    private void JumpDownForce()
    {
        if (isFloatGrounded || inputManager.input_jump.value > 0 || jumpStartCounter > 0) return;

        rb.AddForce(Vector3.down * rb.mass * jumpDownForce);

    }

    private void CheckStartLanding()
    {
        jumpStartCounter--;
        if (!isJumping || jumpStartCounter > 0) return;

        if (rb.velocity.y > 0) return;

        isLanding = true;
        isGrounded = false;
        isJumping = false;
    }
    private void CheckEndLanding()
    {
        if (!isLanding || !isGrounded) return;

        isLanding = false;
        OnLand.TryInvoke();
    }

    private void AirbourneDecel()
    {
        if (isFloatGrounded || rb.velocity == Vector3.zero) return;
        var force = -rb.velocity * rb.mass * deac;
        force.y = 0f;
        rb.AddForce(force);
    }

    private void LateUpdate()
    {
        vel = rb.velocity;

        horizontalVel = vel;
        horizontalVel.y = 0f;

        horizontalVelMag = horizontalVel.magnitude;
    }

    public Vector3 getMoveDirection()
    {
        return rb.velocity;
    }

    public void FreezePlayer(bool unfreeze=false)
    {
        isFrozen = !unfreeze;
        rb.isKinematic = !unfreeze;
    }
}
