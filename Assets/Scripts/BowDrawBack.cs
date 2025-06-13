using UnityEngine;
using UnityEngine.UI;

public class BowDrawback : MonoBehaviour
{
    public float maxPower = 50f;
    public float chargeRate = 20f;
    public Slider powerBarFill;

    private float currentPower = 0f;
    private bool isCharging = false;

    public Transform arrowSpawnPoint;
    public GameObject arrowPrefab;



    public AudioClip chargeClip;
    public AudioClip releaseClip;

    public AudioSource audioSource;

    [SerializeField] private Camera mainCam;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float zoomFOV = 40f;
    [SerializeField] private float zoomSpeed = 5f;

    [SerializeField] private Transform bow;
    [SerializeField] private Vector3 loweredBowPos = new Vector3(0, -0.1f, -0.05f);
    [SerializeField] private Vector3 loweredBowEuler = new Vector3(-10, 0, 0);
    private Vector3 defaultBowPos;
    private Quaternion defaultBowRot;
    [SerializeField] private float transitionSpeed = 5f;

    [SerializeField] private Transform arrow;
    [SerializeField] private float pulledZ = -0.3f;      // how far back arrow pulls
    [SerializeField] private float loweredY = -0.05f;    // how far down arrow goes when bow is lowered
    private Vector3 initialArrowLocalPos;


    private void Start()
    {
        if (bow != null)
        {
            defaultBowPos = bow.localPosition;
            defaultBowRot = bow.localRotation;
        }

       
        if (arrow != null)
            initialArrowLocalPos = arrow.localPosition;
       
    }


    void Update()
    {
        if (isCharging && bow != null)
        {
            // Smoothly move + rotate into lowered position while charging
            bow.localPosition = Vector3.Lerp(bow.localPosition, loweredBowPos, Time.deltaTime * transitionSpeed);
            bow.localRotation = Quaternion.Lerp(bow.localRotation, Quaternion.Euler(loweredBowEuler), Time.deltaTime * transitionSpeed);
        }
        else if (!isCharging && bow != null)
        {
            // Instantly reset to default
            bow.localPosition = defaultBowPos;
            bow.localRotation = defaultBowRot;
        }



        if (isCharging)
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, zoomFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
        }


        if (arrow == null) return;

        Vector3 currentPos = arrow.localPosition;

        // Smoothly interpolate Y like bow lowering
        float targetY = isCharging ? loweredY : initialArrowLocalPos.y;
        float y = Mathf.Lerp(currentPos.y, targetY, Time.deltaTime * transitionSpeed);

        // Smooth Z pullback based on power
        float z = isCharging
            ? Mathf.Lerp(initialArrowLocalPos.z, pulledZ, currentPower / maxPower)
            : initialArrowLocalPos.z;

        // Apply new position
        arrow.localPosition = new Vector3(initialArrowLocalPos.x, y, z);

        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            currentPower = 0f;
            if (audioSource != null && chargeClip != null)
            {
                audioSource.PlayOneShot(chargeClip);
               
            }
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            currentPower += chargeRate * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, 0f, maxPower);

            if (powerBarFill != null)
            {
                powerBarFill.value = currentPower;
            }
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            FireArrow(currentPower);
            isCharging = false;
            currentPower = 0f;

            if (audioSource != null) 
            {
                audioSource.Stop();
            } 

            if (audioSource != null && releaseClip != null)
            {
                audioSource.PlayOneShot(releaseClip);
            }

            if (powerBarFill != null)
            {
                powerBarFill.value = 0f;
            }
        }
    }

    void FireArrow(float power)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetDirection;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            targetDirection = (hit.point - arrowSpawnPoint.position).normalized;
        else
            targetDirection = ray.direction.normalized;

        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.LookRotation(targetDirection));
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.mass = 0.2f;
        rb.drag = 0f;
        rb.angularDrag = 0f;

        rb.AddForce(targetDirection * (power * 0.4f), ForceMode.Impulse);
    }

}