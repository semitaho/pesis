using UnityEngine;

public class SisapelaajaPhysics : MonoBehaviour
{


    [SerializeField] private float waitTimeToResetLyontiasento = 1f;

    [SerializeField] private float hitSpeed = 1.5f;


    [SerializeField] private Transform handTransformer;

    [SerializeField] private float headAngle = 20;

    [SerializeField] private float animationSpeed = 20f;

    [SerializeField] float angleLeftHit = -35f;
    [SerializeField] float angleRightHit = 40f;
    private float minViewportPointerX = 0.4f;
    private float maxViewportPointerX = 0.55f;


    [field: SerializeField] public SisapelaajaInputReader SisapelaajaInputReader { get; private set; }

    private Animator sisapelaajaAnimator;

    private Vector2 currentPointerPosition;

    private bool firingStarted = false;

    private Vector3 raypointInWorld;

    private Vector3 mouseInViewport;

    private Vector3 initialPosition;

    private float currentY;

    private float currentZ;


    private void Start()
    {
        SisapelaajaInputReader.startHittingBallEvent += StartHittingBall;
        SisapelaajaInputReader.releaseHittingBallEvent += ReleaseHittingBall;
        SisapelaajaInputReader.movePointerEvent += MovePointer;
        sisapelaajaAnimator = GetComponent<Animator>();
        initialPosition = handTransformer.position;



    }

    private void Update()
    {
        if (currentPointerPosition != null)
        {
            Debug.Log("current pointer position: " + currentPointerPosition);
            var playerScreenpoint = Camera.main.WorldToScreenPoint(transform.position);
            var pointerViewportPosition = Camera.main.ScreenToViewportPoint(currentPointerPosition);
            Vector3 v3 = Camera.main.ScreenToWorldPoint(new Vector3(currentPointerPosition.x, currentPointerPosition.y, playerScreenpoint.z));
            raypointInWorld = v3;
            mouseInViewport = pointerViewportPosition;
        }

    }

    private void LateUpdate()
    {
        RotateSpine();
        //sisapelaajaKasiMover.MoveHands(raypointInWorld, mouseInViewport);
    }

    private void RotateSpine()
    {
        RotateKoppiMaalyonti();
        RotateLyontisuunta();

    }

    private void RotateKoppiMaalyonti()
    {
        var differVector = new Vector3(raypointInWorld.x, raypointInWorld.y, initialPosition.z) - initialPosition;
        var towards = Quaternion.LookRotation(differVector);
        var angle = towards.eulerAngles.x;
        if (angle > 90)
        {
            currentZ = Mathf.LerpAngle(currentZ, Mathf.Clamp(angle, 360 - headAngle, 360), animationSpeed * Time.deltaTime);
        }
        else if (angle < 90)
        {
            currentZ = Mathf.LerpAngle(currentZ, Mathf.Clamp(angle, 0, headAngle), animationSpeed * Time.deltaTime);

        }

        handTransformer.localRotation = Quaternion.Euler(0, currentY, currentZ);

    }

    private void RotateLyontisuunta()
    {
        var clampedMouseX = Mathf.Clamp(mouseInViewport.x, minViewportPointerX, maxViewportPointerX);
        var arvo = ((angleRightHit - angleLeftHit) * ((clampedMouseX - minViewportPointerX) / (maxViewportPointerX - minViewportPointerX))) + angleLeftHit;
        currentY = Mathf.Lerp(currentY, arvo, animationSpeed * Time.deltaTime);
        handTransformer.localRotation = Quaternion.Euler(0, currentY, currentZ);
    }


    public void StartGainingHitPower()
    {
        firingStarted = true;
        sisapelaajaAnimator.speed = 0;
    }

    private void StartHittingBall()
    {
        sisapelaajaAnimator.SetBool("lyonti", true);
    }



    private void ReleaseHittingBall()
    {
        Debug.Log("Release hitting ball..:" + firingStarted);
        if (!firingStarted)
        {
            sisapelaajaAnimator.speed = 1;
            sisapelaajaAnimator.SetBool("lyonti", false);
        }
        else
        {
            sisapelaajaAnimator.speed = hitSpeed;
            Invoke("ResetToOriginalHitState", waitTimeToResetLyontiasento);
        }
    }

    private void ResetToOriginalHitState()
    {
        Debug.Log("reset original...");
        sisapelaajaAnimator.SetBool("lyonti", false);
        sisapelaajaAnimator.speed = 1;
        firingStarted = false;

    }

    private void MovePointer(Vector2 mousePointer)
    {
        this.currentPointerPosition = mousePointer;
    }

}