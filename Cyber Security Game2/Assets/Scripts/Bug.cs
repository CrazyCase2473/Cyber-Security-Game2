using UnityEngine;

public class Bug : MonoBehaviour
{
    public float speed = 3f;
    public float moveRadius = 5f;

    private Vector3 targetPosition;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if ((transform.position - targetPosition).sqrMagnitude < 0.01f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        Vector3 newTarget;
        int attempts = 0;

        do
        {
            float randomX = transform.position.x + Random.Range(-moveRadius, moveRadius);
            float randomY = transform.position.y + Random.Range(-moveRadius, moveRadius);
            newTarget = new Vector3(randomX, randomY, transform.position.z);
            attempts++;
        }
        while (Physics2D.OverlapCircle(newTarget, 0.5f) != null && attempts < 10);

        targetPosition = newTarget;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hand"))
        {
            GameManager.Instance.AddScore(1);
            GameManager.Instance.PlayBugSquash();
            Destroy(gameObject);
        }
    }
}
