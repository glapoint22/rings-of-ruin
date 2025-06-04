using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RuneflareProjectile : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float launchForceMin = 10f;
    [SerializeField] private float launchForceMax = 16f;
    [SerializeField] private float upwardBias = 1.5f;
    [SerializeField] private float impactRadius = 1.5f;
    //[SerializeField] private GameObject impactVFX;

    private Rigidbody rb;
    private bool hasLaunched = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //rb.useGravity = true;
    }

    public void Launch(Vector3 from, Vector3 to)
    {
        transform.position = from;
        hasLaunched = true;

        float arcTime = Random.Range(2.5f, 3.0f);

        Vector3 velocity = CalculateLaunchVelocity(from, to, arcTime);
        rb.linearVelocity = velocity;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (!hasLaunched) return;

        hasLaunched = false;
        //Explode();
    }


    private Vector3 CalculateLaunchVelocity(Vector3 from, Vector3 to, float arcTime)
    {
        Vector3 toTarget = to - from;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float xzDistance = toTargetXZ.magnitude;

        float g = Mathf.Abs(Physics.gravity.y);
        float heightDifference = toTarget.y;

        // Vertical component
        float vY = (heightDifference + 0.5f * g * arcTime * arcTime) / arcTime;

        // Horizontal component
        float vXZ = xzDistance / arcTime;
        Vector3 directionXZ = toTargetXZ.normalized;

        Vector3 velocity = directionXZ * vXZ;
        velocity.y = vY;

        return velocity;
    }


    //private void Explode()
    //{
    //    if (impactVFX != null)
    //    {
    //        Instantiate(impactVFX, transform.position, Quaternion.identity);
    //    }

    //    Collider[] hits = Physics.OverlapSphere(transform.position, impactRadius);
    //    foreach (Collider col in hits)
    //    {
    //        if (col.CompareTag("Player"))
    //        {
    //            PlayerState player = col.GetComponent<PlayerState>();
    //            player?.TakeDamage(25);
    //        }
    //    }

    //    Destroy(gameObject); // To be pooled later
    //}
}
