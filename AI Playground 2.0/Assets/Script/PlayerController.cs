using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 inputDirection;
    public float playerSpeed = 60f;

    void Update()
    {
        float xMovement;
        float yMovement;

        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");

        inputDirection = new Vector2(xMovement, yMovement).normalized;
    }

}
