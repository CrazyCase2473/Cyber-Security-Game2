using UnityEngine;

public class Bug : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hand"))
        {
            GameManager.Instance.AddScore(1);
            Destroy(gameObject);
        }
    }
}
