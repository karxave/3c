using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action <Vector2> OnMoveInput; // sebuah Event, yaitu Event communication antara Class InputManager dengan Class PlayerMovement

    public Action<bool> OnSprintInput;  // Event OnSprintAction , tipe Action dengan tipe Generic Bool

    public Action OnJumpInput;  // untuk event type Action, karena tidak kirim data apapun jadi tidak pake Generic

    private void Update()
    {
        CheckJumpInput();
        CheckSprintInput();
        CheckCrouchInput();
        CheckChangePOVInput();
        CheckClimbInput();
        CheckGlideInput();
        CheckCancelInput();
        CheckPunchInput();
        CheckMainMenuInput();

        CheckMovementInput();
    }

    private void CheckMovementInput()
    {
        float verticalAxis = Input.GetAxis("Vertical"); // nilai sumbu y
        float horizontalAxis = Input.GetAxis("Horizontal"); //nilai sumbu x

        // variabel Vetor untuk merubah float jadi Vector2
        Vector2 inputAxis = new Vector2(horizontalAxis, verticalAxis);

        //panggil Event OnMoveInput, lalu bikin method baru di Class PlayerMovement untuk gerakin player
        if (OnMoveInput != null)
        {
            OnMoveInput(inputAxis);
        }
    }


    private void CheckMainMenuInput() 
    {
        bool isPressMainMenuInput = Input.GetKeyDown(KeyCode.Escape);
        
        if (isPressMainMenuInput)
        {
            Debug.Log("Back to Main Menu");
        }
    }

    private void CheckPunchInput()
    {
        bool isPressPunchInput = Input.GetKeyDown(KeyCode.Mouse0);
        {
            Debug.Log("Punch");
        }
    }

    private void CheckCancelInput()
    {
        bool isPressCancelInput = Input.GetKeyDown(KeyCode.C);

        if (isPressCancelInput)
        {
            Debug.Log("Cancel Climb or Cancel Glide");
        }
    }

    private void CheckGlideInput()
    {
        bool isPressGlideInput = Input.GetKeyDown(KeyCode.G);

        if (isPressGlideInput)
        {
            Debug.Log("Glide");
        }
    }

    private void CheckClimbInput()
    {
        bool isPressClimbInput = Input.GetKeyDown(KeyCode.E);

        if (isPressClimbInput)
        {
            Debug.Log("Climb");
        }
    }

    private void CheckChangePOVInput()
    {
        bool isPressChangePOVInput = Input.GetKeyDown(KeyCode.Q);
        if (isPressChangePOVInput)
        {
            Debug.Log("Change POV");
        }
    }

    private void CheckCrouchInput()
    {
        bool isPressCrouchInput = Input.GetKeyDown(KeyCode.LeftControl) ||
                                      Input.GetKeyDown(KeyCode.RightControl);
    
        if (isPressCrouchInput)
        { 
            Debug.Log("Crouch"); 
        }
    }

    private void CheckSprintInput()
    {
        bool isHoldSprintInput = Input.GetKey(KeyCode.LeftShift) ||
                                  Input.GetKey(KeyCode.RightShift);

        if (isHoldSprintInput)
        {
            if (OnSprintInput != null)
            {
                OnSprintInput(true);
            }
        }
        else
        {
            if (OnSprintInput != null)
            {
                OnSprintInput(false);
            }

        }
    }

    private void CheckJumpInput()
    {
        bool isPressJumpInput = Input.GetKeyDown(KeyCode.Space);
        if (isPressJumpInput)
        {
            if (OnJumpInput != null)
            {
                OnJumpInput();
            }
        }
    }

   
}
