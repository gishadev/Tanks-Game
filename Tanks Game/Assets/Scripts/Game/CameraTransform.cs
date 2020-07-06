using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    private Transform Target;

    void Update()
    {
        transform.position = new Vector3(Target.position.x, Target.position.y, -10f);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
}
