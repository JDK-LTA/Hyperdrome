using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public delegate void OnInputContinous(float axis);
    public event OnInputContinous OnMouseX;
    public event OnInputContinous OnMouseY;
    public event OnInputContinous OnMoveForward;
    public event OnInputContinous OnMoveRight;

    public delegate void OnInputHold(bool axis);
    public event OnInputHold OnHoldShoot;
    public event OnInputHold OnHoldAim;
    public event OnInputHold OnHoldRun;
    public event OnInputHold OnHoldJet;

    public delegate void OnInputTrigger();
    public event OnInputTrigger OnTriggerJump;
    public event OnInputTrigger OnTriggerShoot;
    public event OnInputTrigger OnTriggerAbility;

    public delegate void OnInputRepTrigger();
    public event OnInputRepTrigger OnRepTriggerShoot;

    private bool movingForward = false, movingRight = false, 
        holdingShoot = false, holdingAim = false, 
        holdingRun = false, holdingJet = false, triggeredJump = false,
        triggeredShoot = false, triggeredAbility = false;


    // Update is called once per frame
    private void Update()
    {
        #region Continous Input
        float aux = Input.GetAxis("Mouse X");
        OnMouseX?.Invoke(aux);
        aux = Input.GetAxis("Mouse Y");
        OnMouseY?.Invoke(aux);

        aux = Input.GetAxis("Vertical");
        if (aux != 0)
        {
            OnMoveForward?.Invoke(aux);
            movingForward = true;
        }
        else if (movingForward)
        {
            OnMoveForward?.Invoke(0);
            movingForward = false;
        }

        aux = Input.GetAxis("Horizontal");
        if (aux != 0)
        {
            OnMoveRight?.Invoke(aux);
            movingRight = true;
        }
        else if (movingRight)
        {
            OnMoveRight?.Invoke(0);
            movingRight = false;
        }
        #endregion

        #region Hold and Trigger Input
        
        //HOLDS
        if (Input.GetAxisRaw("Shoot") == 1)
        {
            OnRepTriggerShoot?.Invoke();

            if (!holdingShoot)
            {
                OnHoldShoot?.Invoke(true);
                holdingShoot = true;
            }
            if (!triggeredShoot)
            {
                OnTriggerShoot?.Invoke();
                triggeredShoot = true;
            }
        }
        else if(Input.GetAxisRaw("Shoot") == 0 && holdingShoot)
        {
            OnHoldShoot?.Invoke(false);

            holdingShoot = false;
            triggeredShoot = false;
        }
        if (Input.GetAxisRaw("Aim") == 1 && !holdingAim)
        {
            OnHoldAim?.Invoke(true);
            holdingAim = true;
        }
        else if (Input.GetAxisRaw("Aim") == 0 && holdingAim)
        {
            OnHoldAim?.Invoke(false);
            holdingAim = false;
        }
        if (Input.GetAxisRaw("Run") == 1 && !holdingRun)
        {
            OnHoldRun?.Invoke(true);
            holdingRun = true;
        }
        else if (Input.GetAxisRaw("Run") == 0 && holdingRun)
        {
            OnHoldRun?.Invoke(false);
            holdingRun = false;
        }
        if (Input.GetAxisRaw("Jet") == 1 && !holdingJet)
        {
            OnHoldJet?.Invoke(true);
            holdingJet = true;
        }
        else if (Input.GetAxisRaw("Jet") == 0 && holdingJet)
        {
            OnHoldJet?.Invoke(false);
            holdingJet = false;
        }
        //

        //TRIGGERS
        if (Input.GetAxisRaw("Jump") == 1 && !triggeredJump)
        {
            OnTriggerJump?.Invoke();
            triggeredJump = true;
        }
        else if (Input.GetAxisRaw("Jump") == 0 && triggeredJump)
        {
            triggeredJump = false;
        }
        if (Input.GetAxisRaw("Ability") == 1 && !triggeredAbility)
        {
            OnTriggerAbility?.Invoke();
            triggeredAbility = true;
        }
        else if (Input.GetAxisRaw("Ability") == 0 && triggeredAbility)
        {
            triggeredAbility = false;
        }
        //
        #endregion

    }
}
