using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void OnInputContinous(float axis);
    public event OnInputContinous OnMouseX;
    public event OnInputContinous OnMouseY;
    public event OnInputContinous OnMoveForward;
    public event OnInputContinous OnMoveRight;

    public delegate void OnInputHold(bool axis);
    public event OnInputHold OnHoldShoot;
    public event OnInputHold OnHoldAim;

    public delegate void OnInputTrigger();
    public event OnInputTrigger OnTriggerShoot;
    public event OnInputTrigger OnTriggerAbility;

    private bool movingForward = false, movingRight = false, 
        holdingShoot = false, holdingAim = false, 
        triggeredShoot = false, triggeredAbility = false;


    // Update is called once per frame
    private void Update()
    {
        #region Continous Input
        float aux = Input.GetAxis("Mouse X");
        OnMouseX(aux);
        aux = Input.GetAxis("Mouse Y");
        OnMouseY(aux);

        aux = Input.GetAxis("Vertical");
        if (aux != 0)
        {
            OnMoveForward(aux);
            movingForward = true;
        }
        else if (movingForward)
        {
            OnMoveForward(0);
            movingForward = false;
        }

        aux = Input.GetAxis("Horizontal");
        if (aux != 0)
        {
            OnMoveRight(aux);
            movingRight = true;
        }
        else if (movingRight)
        {
            OnMoveRight(0);
            movingRight = false;
        }
        #endregion

        #region Hold and Trigger Input
        if (Input.GetAxisRaw("Shoot") == 1 && !holdingShoot)
        {
            OnHoldShoot(true);

            if (!triggeredShoot)
                OnTriggerShoot();

            holdingShoot = true;
            triggeredShoot = true;
        }
        else if(Input.GetAxisRaw("Shoot") == 0 && holdingShoot)
        {
            OnHoldShoot(false);

            holdingShoot = false;
            triggeredShoot = false;
        }

        if (Input.GetAxisRaw("Aim") == 1 && !holdingAim)
        {
            OnHoldAim(true);
            holdingAim = true;
        }
        else if (Input.GetAxisRaw("Aim") == 0 && holdingAim)
        {
            OnHoldAim(false);
            holdingAim = false;
        }

        if (Input.GetAxisRaw("Ability") == 1 && !triggeredAbility)
        {
            OnTriggerAbility();
            triggeredAbility = true;
        }
        else if (Input.GetAxisRaw("Ability") == 0 && triggeredAbility)
        {
            triggeredAbility = false;
        }
        #endregion

    }
}
