using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public readonly Vector2[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;

    public Path(Vector2[] wayPoints, Vector2 startPos, float turnDist)
    {
        lookPoints = wayPoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        Vector2 previousPoint = startPos;
        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = lookPoints[i];
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint 
                = (i == finishLineIndex) 
                ? currentPoint
                : currentPoint - dirToCurrentPoint * turnDist;
            turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDist);
            previousPoint = turnBoundaryPoint;

        }
    }

    public void DrawWithGizmos()
    {
        Vector2 lastPos = Vector2.zero;
        Gizmos.color = Color.cyan;
        foreach(Vector2 p in lookPoints){
            Gizmos.DrawLine(lastPos, p);
            Gizmos.DrawCube(p, Vector2.one * 0.5f);
            lastPos = p;
        }

        Gizmos.color = Color.black;
        foreach (Line l in turnBoundaries)
        {
            l.DrawWithGizmos(2);
        }
    }
}
