using UnityEngine;

public class UpDown : MonoBehaviour
{
    public float height = 2f;
    public float speed = 1f;

    private float startY;
    private float direction = 1f;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y += direction * speed * Time.deltaTime;

        
        if (pos.y > startY + height)
            direction = -1f;
        else if (pos.y < startY - height)
            direction = 1f;

        transform.position = pos;
    }
}
