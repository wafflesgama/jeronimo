using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMover : MonoBehaviour
{
    
    public float deac;
    public float springStrength = 1;
    public float dampenStrength = 1;
    public float floatHeight = 1;
    public LayerMask mask;
    public float castDist = 1f;
    public PlayerInputManager playerInputManager;
    public float goalVelocity;
    public Vector3 velocity;
    bool isGrounded;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }
    
    private void FixedUpdate()
    {
        Levitate();

        Move();

        Deacel();
    }

    private void Deacel()
    {
        if (isGrounded || rb.velocity == Vector3.zero) return;
        var force = -rb.velocity * rb.mass * deac;
        force.y = 0f;
        rb.AddForce(force);
    }

    private void Move()
    {
        if (!isGrounded) return;

        var move = playerInputManager.input_move.value;
        var moveRet = new Vector3(move.x, 0, move.y);
        var transformedMove = Camera.main.transform.TransformDirection(moveRet);
        transformedMove.y = 0;

        var force = rb.mass * ((transformedMove * goalVelocity - rb.velocity) / Time.fixedDeltaTime);
        force.y = 0;

        rb.AddForce(force);
    }

    private void Levitate()
    {
        var hasHit = Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, castDist, mask, QueryTriggerInteraction.Ignore);
        isGrounded = hasHit;
        if (!hasHit) return;

        Vector3 outsideVel = Vector3.zero;

        //if (hasHit && hit.rigidbody != null)
        //{
        //    outsideVel = hit.rigidbody.velocity;
        //}

        float dirOwnVel = Vector3.Dot(-transform.up, rb.velocity);
        float dirOutsideVel = Vector3.Dot(-transform.up, outsideVel);

        float dirRelation = dirOwnVel - dirOutsideVel;
        float x = hit.distance - floatHeight;

        float spring = (x * springStrength) - (dirRelation * dampenStrength);

        rb.AddForce(-transform.up * rb.mass * spring);

        //if (outsideVel != Vector3.zero)
        //{
        //    hit.rigidbody.AddForceAtPosition(-transform.up * -springStrength, hit.point);
        //}

    }

    private void LateUpdate()
    {
        velocity = rb.velocity;
    }
}
