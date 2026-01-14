using UnityEngine;

public class rotacion : MonoBehaviour
{
    public GameObject figura;
    public float speed = 10f;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(figura.transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
