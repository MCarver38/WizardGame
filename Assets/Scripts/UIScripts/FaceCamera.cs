using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private void LateUpdate()
    {
        transform.LookAt(cam.transform);
        transform.Rotate(0, 180, 0);
    }
}
