using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementModule : MonoBehaviour
{
    #region Debug
    [Header("Testing")]
    public bool indicators = false;
    public bool playerControlled = false;
    #endregion


    [Header("Actual")]
    public float maxSpeed = 4;
    public float steerStrength { get { return maxSpeed * 5; } }

    public Rigidbody2D rb;

    Vector2 velocity;
    public Vector2 desiredDirection;

    public MovementBehaviour mb;
    private PlayerController playerController;

    private Vector2 currentTargetPos;
    public Vector2 CurrentTargetPos { get { return currentTargetPos;} set { /*Debug.Log("value being changed to" + value);*/ currentTargetPos = value; } }
    public LookTypes lookType;

    Path path;

    #region Properties
    public Vector2 FrontPos { get { if (rb != null) return (Vector2)transform.position + rb.velocity.normalized * rb.velocity.magnitude * 0.08f; else return transform.position; } }
    public Vector2 BackPos { get { if (rb != null) return (Vector2)transform.position - rb.velocity.normalized * rb.velocity.magnitude * 0.08f; else return transform.position; } }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (playerControlled)
        {
            playerController = GetComponent<PlayerController>();
        }
    }

    private void FixedUpdate()
    {
        Move();
        Look(lookType);
        #region Debugging
        //!! Debugging
        if (indicators)
        {
            DrawArrow.ForDebug(transform.position, desiredDirection, Color.black);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            PathRequestManager.RequestPath(transform.position, mousePos, OnPathFound);
        } 
        #endregion
    }

    #region Movement Functios
    private void Look(LookTypes look)
    {
        if(look == LookTypes.INDIRECTION)
        {
            if(rb.velocity.magnitude > 0)
            {
                RotateTo(rb.velocity);
            }
        }
        else if(look == LookTypes.ATTARGET)
        {
            Vector2 lookDir = (CurrentTargetPos - (Vector2)transform.position).normalized;
            RotateTo(lookDir);
        }

    }

    private void Move()
    {
        if (playerControlled)
        {
            desiredDirection = playerController.inputDirection;
        }
        else
        {
            desiredDirection = mb.CalculateDirection(this);
        }

        Vector2 desiredVel = desiredDirection * maxSpeed;
        Vector2 desiredSteeringForce = (desiredVel - velocity) * steerStrength;
        Vector2 acceleration = Vector2.ClampMagnitude(desiredSteeringForce, steerStrength) / 1;

        velocity = Vector2.ClampMagnitude(velocity + acceleration * Time.deltaTime, maxSpeed);

        rb.velocity = velocity * Time.deltaTime * maxSpeed;
    } 

    private void RotateTo(Vector2 lookDirection)
    {
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), (Time.deltaTime * rb.velocity.magnitude) + (Time.deltaTime * 15));
    }
    #endregion

    #region Path Finding
    public void OnPathFound(Vector2[] wayPoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(wayPoints, transform.position, rb.velocity.magnitude * 0.05f + 0.8f);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        bool followPath = true;
        int pathIndex = 0;

        //Why is this here
        //transform.LookAt(path.lookPoints[0]);

        while (followPath)
        {
            if (path.turnBoundaries[pathIndex].HasCrosssedLine(transform.position))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followPath = false;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followPath)
            {
                CurrentTargetPos = path.lookPoints[pathIndex];
            }
            yield return null;
        }
    } 
    #endregion

    public void OnDrawGizmos()
    {
        if (indicators)
        {
            if (path != null)
            {
                path.DrawWithGizmos();
            }
        }

        //Handles.color = Color.black;
        //Handles.DrawSolidArc(CurrentTargetPos, Vector3.forward, Vector3.up, 360, 0.5f);
    }
}

public enum LookTypes
{
    INDIRECTION,
    ATTARGET,
    OPPOSITEDIRECTION
}
