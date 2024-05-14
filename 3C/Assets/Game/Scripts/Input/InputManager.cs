using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action <Vector2> OnMoveInput; // sebuah Event, yaitu Event communication antara Class InputManager dengan Class PlayerMovement

    public Action<bool> OnSprintInput;  // Event OnSprintAction , tipe Action dengan tipe Generic Bool

    public Action OnJumpInput;  // untuk event type Action, karena tidak kirim data apapun jadi tidak pake Generic

    public Action OnClimbInput;

    public Action OnCancelClimb;

    public Action OnChangePOV;

    public Action OnCrouchInput;

    public Action OnGlideInput;

    public Action OnCancelGlide;

    public Action OnPunchInput;


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
            if (isPressPunchInput)
            {
                OnPunchInput();
            }
            
        }
    }

    private void CheckCancelInput()
    {
        bool isPressCancelInput = Input.GetKeyDown(KeyCode.C);

        if (isPressCancelInput)
        {
           if (isPressCancelInput)
            {
                if (OnCancelClimb != null)
                {
                    OnCancelClimb();
                }
            }
           if (OnCancelGlide != null)
            {
                OnCancelGlide();
            }
        }
    }

    private void CheckGlideInput()
    {
        bool isPressGlideInput = Input.GetKeyDown(KeyCode.G);

        if (isPressGlideInput)
        {
            if (OnGlideInput != null)
            {
                OnGlideInput();
            }
            
        }
    }

    private void CheckClimbInput()
    {
        bool isPressClimbInput = Input.GetKeyDown(KeyCode.E);

        if (isPressClimbInput)
        {
            if (OnClimbInput != null)
            {
                OnClimbInput();
            }
            
        }
    }

    private void CheckChangePOVInput()
    {
        bool isPressChangePOVInput = Input.GetKeyDown(KeyCode.Q);
        if (isPressChangePOVInput)
        {
            if (OnChangePOV != null)
            {
                OnChangePOV();
            }
        }
    }

    private void CheckCrouchInput()
    {
        bool isPressCrouchInput = Input.GetKeyDown(KeyCode.LeftControl) ||
                                      Input.GetKeyDown(KeyCode.RightControl);
    
        if (isPressCrouchInput)
        {
            OnCrouchInput();
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
