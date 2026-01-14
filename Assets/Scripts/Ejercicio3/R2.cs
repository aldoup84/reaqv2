using UnityEngine;

public class R2 : MonoBehaviour
{
    public GameObject figura;
    public float speed;
    float x;

    void Update()
    {
        x += Time.deltaTime * 15;
        transform.rotation = Quaternion.Euler(0, x, 0);
        //transform.RotateAround(figura.transform.position, Vector3.forward, speed * Time.deltaTime);		
    }
}
