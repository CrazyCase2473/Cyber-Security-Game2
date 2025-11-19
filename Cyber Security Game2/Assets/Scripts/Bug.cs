using UnityEngine;

public class Bug : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 direction;

    void Start()
    {
        PickRandomDirection();
    }

    void Update()
{
    transform.Translate(direction * speed * Time.deltaTime);

    Vector3 pos = transform.position;
    Vector3 min = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
    Vector3 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

    pos.x = Mathf.Clamp(pos.x, min.x, max.x);
    pos.y = Mathf.Clamp(pos.y, min.y, max.y);

    transform.position = pos;
}

    void PickRandomDirection()
    {
        direction = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized;
        Invoke("PickRandomDirection", Random.Range(1f,3f));
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if(other.CompareTag("Hand"))
    {
        GameManager.Instance.BugSquashed(this);
        Destroy(gameObject);
    }
}
}
