using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Start()
    {
        transform.LookAt(Camera.main.transform.position, -transform.up);
    }
}
