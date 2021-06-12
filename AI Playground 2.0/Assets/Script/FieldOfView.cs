using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine.U2D;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    //!!
    public Color originalColor;
    public Color foundColor;
    
    public List<Transform> visibleTarget = new List<Transform>();

    void Start()
    {
        StartCoroutine("FindTargetWithDelay", 0.1f);
        originalColor = GetComponentInChildren<SpriteShapeRenderer>().color;
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }

    void FindVisibleTarget()
    {
        // Clear the list everytime the function runs to not have any duplicates
        visibleTarget.Clear();
        GetComponentInChildren<SpriteShapeRenderer>().color = originalColor;

        // Array to store found targets
        Collider2D[] targetInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        // Filter all the found targets if they are being blocked by obstacles and is not within the field of view
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            if(Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector2.Distance(transform.position, target.position);
                if(!Physics2D.Raycast(transform.position, dirToTarget, distToTarget,obstacleMask))
                {
                    if (target == this.transform) continue;
                    else
                    {
                        visibleTarget.Add(target);
                        if (target.gameObject.CompareTag("Player"))
                        {
                            GetComponentInChildren<SpriteShapeRenderer>().color = foundColor;
                        }
                    }
                }
            }
        }
    }

    public Vector2 DirectionFromAngle(float angleInDeg, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDeg -= transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }


    void OnDrawGizmos()
    {
        //Handles.color = Color.black;    
        //Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, viewRadius);
        //Vector3 viewAngleA = DirectionFromAngle(-viewAngle / 2, false);
        //Vector3 viewAngleB = DirectionFromAngle(viewAngle / 2, false);

        //Handles.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        //Handles.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

        //if (!GetComponent<BaseEnemy>().exception)
        //{
        //    Handles.color = Color.red;
        //    foreach (Transform visibleTarget in visibleTarget)
        //    {
        //        Handles.DrawLine(transform.position, visibleTarget.position);
        //    }
        //}

    }
}
