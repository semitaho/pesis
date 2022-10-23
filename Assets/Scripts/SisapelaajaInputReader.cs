using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SisapelaajaInputReader : MonoBehaviour, SisapelaajaInputController.IPlayerActions
{

    public event Action startHittingBallEvent;

    public event Action releaseHittingBallEvent;


    public event Action<Vector2> movePointerEvent;

    private Boolean hold = false;

    private float currentAnimationSpeed;

    private SisapelaajaInputController inputActions;

    private Vector3 raypointInWorld;

    private Vector3 mouseInViewport;


    private void Awake()
    {
        inputActions = new SisapelaajaInputController();
        inputActions.Player.SetCallbacks(this);
    }

    // Update is called once per frame

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnFire(InputAction.CallbackContext context)
    {

        Debug.Log("phase: " + context.phase);

        if (context.canceled)
        {
            hold = false;
            releaseHittingBallEvent?.Invoke();
            return;
        }
        if (context.performed && !hold)
        {
            hold = true;
            startHittingBallEvent?.Invoke();

        };


    }

    public void OnPoint(InputAction.CallbackContext context)
    {

        var vector2 = context.ReadValue<Vector2>();
        movePointerEvent?.Invoke(vector2);
    }
}
