using System;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    // Queue was used to systematically and chronologically handle the pathfinding requests(first in, first out)
    Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest; // Reference to the current request

    static PathRequestManager instance;
    Pathfinding pathFinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<Pathfinding>();
    }
    // This is the entry point for requesting a path. The PathRequest is the ticket with all the information and when put through this function, it is added to the queue
    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestsQueue.Enqueue(newRequest); // Add the new request inside the queue
        instance.TryProcessNext(); // Process the next request at the bottom of the queue(aka, the oldest in queue)
    }

    // Procedures that needed to be ran before finding a path is done here
    void TryProcessNext()
    {
        // If there are more paths to be processed and if there are not pathings being processed
        if(!isProcessingPath && pathRequestsQueue.Count > 0)
        {
            currentPathRequest = pathRequestsQueue.Dequeue(); // Dequeue gets the first item in the queue and takes it out
            isProcessingPath = true; // Update processing status
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd); // Call the pathfinding script to do it's job
        }
    }

    // Updates the queue to know that it's already done processing the current path and goes on and call the function which tries to check if there is anything else in the queue
    public void FinishProcessingPath(Vector2[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    // A struct which dictates the variables of PathRequest
    struct PathRequest
    {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callback;
        
        // This constructor is taking in an action arguement which refers to a function which specifically has Vector2[] and bool for that functions parameters
        public PathRequest(Vector2 _start, Vector2 _end, Action<Vector2[] , bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
