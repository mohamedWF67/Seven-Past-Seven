using UnityEngine;

public class UpDown : MonoBehaviour
{
    public float height = 2f; // you can change this

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Sin(Time.time) * height,
            transform.position.z
        );
    }
}

