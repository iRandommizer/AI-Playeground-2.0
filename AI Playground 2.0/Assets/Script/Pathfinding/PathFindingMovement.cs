using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A component piece for the AI agents for requesting paths
public class PathFindingMovement : MonoBehaviour
{
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;

    public Transform target;

    // I won't be needing this later on since a different script will be hanlding it
    public float speed = 5;
    public float turnSpeed = 3;
    public float turnDistance = 4;

    Path path;

    private void Start()
    {
        // Request a path with current path you have
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        StartCoroutine(UpdatePath());
    }

    // When path is found, follow the path (NOt needed, I'll be using a separate script to handle all this)
    public void OnPathFound(Vector2[] wayPoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(wayPoints, transform.position, turnDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    // This is how a new path request is sent when the path has been changed
    IEnumerator UpdatePath()
    {
        if(Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }

        // Makes a new request at the beginning of a function
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector2 targetPosOld = target.position; // Just to keep track if the target has moved 

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime); // To prevent this function from updating all the time
            if(((Vector2)target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                //Request new path wuth the current positions
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
                Debug.Log("Called");
            }
        }
    }

    // This function just moves the agent towards the way points but it doesnt set it
    IEnumerator FollowPath()
    {
        bool followPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        while (followPath)
        {
            if (path.turnBoundaries[pathIndex].HasCrosssedLine(transform.position))
            {
                if(pathIndex == path.finishLineIndex)
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
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - (Vector2)transform.position);                
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        
        if(path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
