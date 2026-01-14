using UnityEngine;

public class R4 : MonoBehaviour
{
    public GameObject figura;
    public float speed = 100f;
    float x;

    void Update()
    {
        x += Time.deltaTime * 10;
        transform.rotation = Quaternion.Euler(0, 0, -x);
    }
}
