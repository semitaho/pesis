using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SisapelaajaController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 0.5f;

    [SerializeField] private float rotateSpeed = 1f;


    [SerializeField] private float maxLeftTurningRotation = 45;

    [SerializeField] private float maxRightTurningRotation = 60;


    [SerializeField] private Vector3 maxLeftTurningMoveVector = new Vector3(0, 0, -3);

    [SerializeField] private Vector3 maxRightTurningMoveVector;


    private bool firingStarted = false;
    private float currentAnimationSpeed;
    private Animator sisapelaajaAnimator;

    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private SisapelaajaInputController inputActions;

    private CharacterController characterController;

    private bool leftLock = false;
    private bool rightLock = false;

    private LayerMask fieldMask;


    private void Awake()
    {
        inputActions = new SisapelaajaInputController();
        characterController = GetComponent<CharacterController>();

    }
    // Start is called before the first frame update
    void Start()
    {
        fieldMask = LayerMask.NameToLayer("Alue");
        initialRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        initialPosition = transform.position;
        sisapelaajaAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Look();

    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Look()
    {
        var mousePosition = inputActions.UI.Point.ReadValue<Vector2>();
        LookAtMouse(mousePosition);
    }

    private void LookAtMouse(Vector2 mousePosition)
    {

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit[] raycastHits = Physics.RaycastAll(ray, Mathf.Infinity);

        foreach (var raycastHit in raycastHits)
        {
            if (raycastHit.collider.gameObject.layer != LayerMask.NameToLayer("Alue")) continue;
            var rayhitPositionWithoutY = new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z);
            var towardsRotation = Quaternion.LookRotation(rayhitPositionWithoutY - transform.position);

            var currentAngle = transform.eulerAngles.y;
            var initialAngleY = initialRotation.eulerAngles.y;
            var relativeAngle = initialAngleY - currentAngle;
            Move(rayhitPositionWithoutY);

            if (IsPointerInRotateArea(towardsRotation.eulerAngles.y, initialAngleY))
            {
                Rotate(relativeAngle, towardsRotation);
                return;

            }
            if (IsRotatedTooMuchLeft(currentAngle, relativeAngle, initialAngleY))
            {
                leftLock = true;
                rightLock = false;
                transform.rotation = Quaternion.Euler(0, initialRotation.eulerAngles.y - maxLeftTurningRotation, 0);
                return;
            }
            if (IsRotatedTooMuchRight(currentAngle, relativeAngle))
            {
                leftLock = false;
                rightLock = true;
                transform.rotation = Quaternion.Euler(0, initialRotation.eulerAngles.y + maxRightTurningRotation, 0);
                return;
            }

        }
    }

    private bool IsPointerInRotateArea(float towardsAngleY, float initialAngleY)
    {
        var relativeAngle = initialAngleY - towardsAngleY;
        if (relativeAngle >= 0 && relativeAngle <= maxLeftTurningRotation) return true;
        if (relativeAngle < 0 && Math.Abs(relativeAngle) < maxRightTurningRotation) return true;
        return false;
    }

    private bool IsRotatedTooMuchRight(float currentAngle, float relativeAngle)
    {
        if (rightLock)
        {
            return true;
        }
        return relativeAngle < 0 && currentAngle - initialRotation.eulerAngles.y > maxRightTurningRotation;
    }

    private bool IsRotatedTooMuchLeft(float currentAngle, float relativeAngle, float initialAngleY)
    {
        if (leftLock)
        {
            return true;
        }
        return relativeAngle > 0 && currentAngle > initialAngleY - maxLeftTurningRotation;
    }

    private void Rotate(float relativeAngle, Quaternion completeRotation)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, completeRotation, rotateSpeed * Time.deltaTime);
        Vector3 moveVector = Vector3.MoveTowards(
            transform.position,
        initialPosition + (relativeAngle > 0 ? maxLeftTurningMoveVector : maxRightTurningMoveVector),
        Time.deltaTime * moveSpeed);
        Vector3 completeMovement = Vector3.Lerp(transform.position, moveVector, Time.deltaTime * moveSpeed);
        leftLock = false;
        rightLock = false;
    }

    private void Move(Vector3 targetMove)
    {
        Debug.Log("target move" + targetMove);
        Vector3 moveVector = Vector3.MoveTowards(
            transform.position, targetMove,
        Time.deltaTime * moveSpeed);
        transform.position = moveVector;
    }

    public void PauseToGainPower()
    {
        //  currentAnimationSpeed = sisapelaajaAnimator.speed;
        //  sisapelaajaAnimator.speed = 0;
    }

    public void TeeJotain(InputAction.CallbackContext context)
    {

        if (context.canceled)
        {
            firingStarted = false;
            sisapelaajaAnimator.SetBool("lyonti", false);
        }
        else if (!firingStarted)
        {
            firingStarted = true;
            sisapelaajaAnimator.SetBool("lyonti", true);
        }

    }
}
