using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public Vector2 direction = Vector2.right;

    public float speed = 1f;

    public int size = 1;

    private Vector3 leftEdge;
    private Vector3 rightEdge;
    private void Start()
    {
        // -1 for y axis
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);

        // 1 for x axis
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    private void Update()
    {
        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x)
        {
            Vector3 position = transform.position;
            position.x = leftEdge.x - size;

            transform.position = position;
        }

        else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            Vector3 position = transform.position;
            position.x = rightEdge.x + size;

            transform.position = position;
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
