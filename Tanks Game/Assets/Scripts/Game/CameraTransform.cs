using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    private Transform Target;

    void Update()
    {
        transform.position = new Vector3(Target.position.x, transform.position.y, Target.position.z);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
}
