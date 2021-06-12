using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Behaviour/Avoidance")]
public class AvoidanceBehaviour : MovementBehaviour
{
    //TESTING
    public List<Vector2> positionsT;

    public float avoidanceDistance = 3;
    public float numOfRays = 8;
    public float detectionAngelSpread = 210;
    public LayerMask avoidanceLayer;
    public override Vector2 CalculateDirection(MovementModule movementModule)
    {
        float modifiedAvoidanceDistance;
        float t = 1;
        Vector2 undesiredDirection = Vector2.zero;
        float angle = detectionAngelSpread / numOfRays;

        for (int i = 0; i < numOfRays; i++)
        {
            RaycastHit2D hit;

            Vector2 dir = RotateBy(movementModule.transform.up, i * angle - detectionAngelSpread/2);

            Vector2 predictedPosition = movementModule.transform.position
                + (Vector3)movementModule.rb.velocity.normalized * (0.1f * (detectionAngelSpread / 360) * movementModule.rb.velocity.magnitude) + (Vector3)movementModule.rb.velocity.normalized * 0.3f
                + (Vector3)dir.normalized * (0.014f * movementModule.rb.velocity.magnitude) + (Vector3)dir.normalized * 0.3f;

            modifiedAvoidanceDistance = (avoidanceDistance + ((avoidanceDistance * Mathf.Pow(Vector2.Dot(movementModule.rb.velocity.normalized, dir.normalized), 2)) * 3)) / 4;

            hit = Physics2D.Raycast(predictedPosition, dir.normalized, modifiedAvoidanceDistance , avoidanceLayer);

            if (hit)
            {
                if (hit.collider == movementModule.GetComponent<Collider2D>()) continue;
                float dist = (hit.point - predictedPosition).magnitude;
                t = dist / (Vector2.Distance(predictedPosition, dir.normalized * modifiedAvoidanceDistance + predictedPosition));             
                if (t > 1) t = 1;
                if (t == 0)
                {
                    return Vector2.Perpendicular(movementModule.rb.velocity);
                }
                undesiredDirection += (hit.point - (Vector2)movementModule.transform.position);
                if (movementModule.indicators)
                {
                    Debug.DrawLine(predictedPosition, dir.normalized * modifiedAvoidanceDistance + predictedPosition, Color.red);
                }                
            }
            else 
            {
                if (movementModule.indicators)
                {
                     Debug.DrawLine(predictedPosition, dir.normalized * modifiedAvoidanceDistance + predictedPosition, Color.blue);
                }
            } 
        }
        //Debug.Log((movementModule.maxSpeed - movementModule.rb.velocity.magnitude) / movementModule.maxSpeed);
        //DrawArrow.ForDebug(movementModule.transform.position, undesiredDirection.normalized / t / t, Color.red);
        Vector2 prevPos = movementModule.transform.position;
        List<Vector2> positions = PredictPos(movementModule.transform.position, 70, 0.6f, movementModule);
        for (int i = 0; i < positions.Count; i++)
        {
            if (positionsT.Count > 50)
            {
                positionsT.RemoveAt(0);
            }

            if (i == positions.Count - 1 && movementModule.rb.velocity.magnitude > 0)
            {
                positionsT.Add(positions[i]);
            }
            //Debug.DrawLine(prevPos, positions[i], Color.cyan);
            prevPos = positions[i];

        }

        if (positionsT.Count > 0)
        {
            Vector2 tempPOS = Vector2.zero;

            if (Vector2.Distance(movementModule.transform.position, FindAveragePos(positionsT)) > 6)
            {
                Vector2 tempdir = (FindAveragePos(positionsT) - (Vector2)movementModule.transform.position).normalized;
                movementModule.tempPos = tempdir * 6;
                tempPOS = tempdir * 6;
            }
            else
            {
                movementModule.tempPos = FindAveragePos(positionsT);
                tempPOS = FindAveragePos(positionsT);
            }
            if (movementModule.baseEnemyTemp.exception)
            {
                Debug.DrawLine(movementModule.transform.position, tempPOS, Color.red);    
            }

        }
        return (-undesiredDirection.normalized + movementModule.desiredDirection.normalized) / t / (t * 16 * ((movementModule.maxSpeed - movementModule.rb.velocity.magnitude)/movementModule.maxSpeed));
    }

    public Vector2 RotateBy(Vector2 v, float a, bool bUseRadians = false)
    {
        if (!bUseRadians) a *= Mathf.Deg2Rad;
        var ca = Mathf.Cos(a);
        var sa = Mathf.Sin(a);
        var rx = v.x * ca - v.y * sa;

        return new Vector2((float)rx, (float)(v.x * sa + v.y * ca));
    }

    public List<Vector2> PredictPos(Vector2 pos, int numOfIterations, float distancePerIterations, MovementModule movementModule)
    {
        Vector2 currentPos = movementModule.transform.position;
        Vector2 currentDir = movementModule.rb.velocity.normalized;
        Vector2 prevDir = movementModule.rb.velocity.normalized;
        Vector2 prevPos = movementModule.transform.position;
        List<Vector2> positions = new List<Vector2>();
        for (int i = 0; i < numOfIterations; i++)
        {
            float t = 1;
            Vector2 undesiredDirection = Vector2.zero;
            float angle = detectionAngelSpread / numOfRays;

            for (int j = 0; j < numOfRays; j++)
            {
                RaycastHit2D hit;

                Vector2 dir = RotateBy(currentDir, j * angle - detectionAngelSpread/2);

                Vector2 predictedPosition = (Vector3) currentPos;

                hit = Physics2D.Raycast(predictedPosition, dir.normalized, 1.5f , avoidanceLayer);

                if (hit)
                {
                    if (hit.collider == movementModule.GetComponent<Collider2D>()) continue;
                    float dist = Vector2.Distance(hit.point, predictedPosition);
                    t = dist / 2;             
                    if (t > 1) t = 1;
                    undesiredDirection += (hit.point - (Vector2)currentPos);
                }
            }
            //Debug.Log((movementModule.maxSpeed - movementModule.rb.velocity.magnitude) / movementModule.maxSpeed);
            //DrawArrow.ForDebug(movementModule.transform.position, undesiredDirection.normalized / t / t, Color.red);
            currentDir = (-undesiredDirection.normalized + prevDir);
            currentDir = currentDir.normalized;
            currentPos += currentDir * distancePerIterations;
            positions.Add(currentPos);
            prevDir = currentDir;
            prevPos = currentPos;
            //Debug.DrawLine();
        }

        return positions;
    }

    public Vector2 FindAveragePos(List<Vector2> positions)
    {
        Vector2 avePos = positions[0];
        for (int i = 1; i < positions.Count; i++)
        {
            avePos += positions[i];
        }

        avePos /= positions.Count;
        return avePos;
    }
}
