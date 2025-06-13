using UnityEngine;

public class ArrowFlight : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude > 0.1f)
        {
            transform.forward = rb.velocity.normalized;
        }
    }
}
