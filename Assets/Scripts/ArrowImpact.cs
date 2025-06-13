using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ArrowImpact : MonoBehaviour
{
    private Rigidbody rb;
    private TrailRenderer tr;
    private bool hasHit = false;

    public Transform impactSpawnPoint;
    public GameObject impactEffectPrefab;
    public GameObject impactHolePrefab;

    public AudioClip impactSound;
    public AudioClip bullseyeSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        Destroy(gameObject, 10f);
    }
    void Update()
    {
        if (!hasHit)
        {
            Destroy(gameObject, 5f); // clean up missed arrows
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return; // Prevent double collision handling
        hasHit = true;

        // Stop physics
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Collider col = GetComponentInChildren<Collider>();
        if (col != null) col.enabled = false;

        tr.enabled = false;
        // Get hit point and normal
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;
        Quaternion hitRotation = Quaternion.LookRotation(-contact.normal);


        // Handle impact sound
        AudioClip clipToPlay = null;

        // Check tag or collider name
        if (collision.gameObject.CompareTag("25"))
        {
            clipToPlay = bullseyeSound;
        }
        else
        {
            clipToPlay = impactSound;
        }
        // Create a temporary GameObject at the impact location
        if (clipToPlay != null)
        {
            GameObject tempGO = new GameObject("ImpactSound");
            tempGO.transform.position = hitPoint;

            AudioSource source = tempGO.AddComponent<AudioSource>();
            source.clip = clipToPlay;
            source.spatialBlend = 1f;
            source.volume = 1f;

            //diff pitch for each sound
            if (clipToPlay == impactSound)
                source.pitch = Random.Range(0.9f, 1.1f);
            else
                source.pitch = Random.Range(0.98f,1.02f); 

            source.minDistance = 1f;
            source.maxDistance = 20f;
            source.Play();

            Destroy(tempGO, clipToPlay.length / source.pitch + 0.1f);
        }

        // Spawn bullet hole
        if (impactHolePrefab != null)
        {
            GameObject hole = Instantiate(impactHolePrefab, hitPoint + contact.normal * 0.001f, hitRotation);
            hole.transform.SetParent(collision.transform);
            hole.transform.Rotate(0, 0, Random.Range(0, 360));
        }



        if (impactEffectPrefab != null && impactSpawnPoint != null)
        {
            Instantiate(impactEffectPrefab, impactSpawnPoint.position, impactSpawnPoint.rotation);
        }

        int score = 0;

        string tag = collision.gameObject.tag;
        switch (tag)
        {
            case "2": score = 2; GameManager.Instance.AddScore(score); break;
            case "4": score = 4; GameManager.Instance.AddScore(score); break;
            case "6": score = 6; GameManager.Instance.AddScore(score); break;
            case "8": score = 8; GameManager.Instance.AddScore(score); break;
            case "10": score = 10; GameManager.Instance.AddScore(score); break;
            case "25": score = 25; GameManager.Instance.AddScore(score); break;
            default:
                score = 0; GameManager.Instance.AddScore(score); break;
        }

        Target board = collision.transform.GetComponentInParent<Target>();
        if (board != null)
        {
            board.TakeHit(score);
        }




    }

  
}

