using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RuneflareProjectile : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;

    private Rigidbody rb;
    private float spawnTimer;
    private bool hasLaunched = false;
    public float SpawnTimer => spawnTimer;
    public event Action<RuneflareProjectile> OnRuneflareDestroyed;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



    public void UpdateSpawnTimer(float newTimer)
    {
        spawnTimer = newTimer;
    }



    public void DecreaseSpawnTimer(float deltaTime)
    {
        spawnTimer -= deltaTime;
    }



    public void Launch(Vector3 from, Vector3 to)
    {
        transform.position = from;
        hasLaunched = true;

        float arcTime = UnityEngine.Random.Range(2.5f, 3.0f);

        Vector3 velocity = CalculateLaunchVelocity(from, to, arcTime);
        rb.linearVelocity = velocity;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (!hasLaunched) return;
        hasLaunched = false;
        OnRuneflareDestroyed?.Invoke(this);
        if (collision.collider.CompareTag("Player")) DamageManager.UpdateDamage(damageAmount);
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
}