using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    private Transform firstPerson_View;
    private Transform firstPerson_Camera;

    private Vector3 firstPerson_View_Rotation = Vector3.zero;

    public float walkSpeed = 6.57f;
    public float runSpeed = 10f;
    public float crouchSpeed = 4f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;

    private float speed;
    private bool is_Moving, is_Grounded, is_Crouching;

    private float inputX, inputY;
    private float inputX_Set, inputY_Set;
    private float inputModifyFactor;

    private bool limitDiagonalSpeed = true;
    private float antiBumpFactor = 0.75f;

    private CharacterController charController;
    private Vector3 moveDirection = Vector3.zero; 

    // Start is called before the first frame update
    void Start()
    {
        // transform.Find() searches children of a game object
        firstPerson_View = transform.Find("FPS View").transform;
        charController = GetComponent<CharacterController>();
        speed = walkSpeed;
        is_Moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        
    }

    private void PlayerMovement()
    {
       if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
       {
            Debug.Log("HEYYYY");
            if (Input.GetKey(KeyCode.W))
                inputY_Set = 1f;
            else
                inputY_Set = -1f;
       }
       else
       {
            inputY_Set = 0f;
       }

       if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
       {
            if (Input.GetKey(KeyCode.D)) 
                inputX_Set = 1f;
            else
                inputX_Set = -1f;        
       }
       else
       {
            inputX_Set = 0f;
       }

        inputY = Mathf.Lerp(inputY, inputY_Set, Time.deltaTime * 19f);
        inputX = Mathf.Lerp(inputX, inputX_Set, Time.deltaTime * 19f);

        // Limit the moving speed if the player is moving horizontally/vertically & diagonally at the same time
        // Example is a player hits key W,A at the same time, we limit the moving speed so that it won't get above the normal speed
        inputModifyFactor = Mathf.Lerp(inputModifyFactor,
        (inputY_Set != 0 && inputX_Set != 0 && limitDiagonalSpeed) ? 0.75f : 1.0f,
        Time.deltaTime * 19f);

        firstPerson_View_Rotation = Vector3.Lerp(firstPerson_View_Rotation, Vector3.zero, Time.deltaTime * 5f);
        firstPerson_View.localEulerAngles= firstPerson_View_Rotation; // View relative to the parent game object, hence localEularAngles

        if (is_Grounded)
        {
            moveDirection = new Vector3(
                inputX * inputModifyFactor, 
                -antiBumpFactor, // smooth the player's movement when bumping into things
                inputY * inputModifyFactor);
            // Transform the direction from local coord to Unity's world coord
            moveDirection = transform.TransformDirection(moveDirection) * speed;
        }
        // Character Controller does not apply gravity like Rigidbody so we need to apply it on our own
        moveDirection.y -= gravity * Time.deltaTime;

        is_Grounded = (charController.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
        is_Moving = charController.velocity.magnitude > 0.15f;
    }
}
