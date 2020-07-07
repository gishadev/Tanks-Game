using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    [SerializeField] private float speed;

    [Header("Bounds")]
    public Transform b_Min;
    public Transform b_Max;

    float width, height;
    float xPos, yPos;

    private Transform Target;

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    Camera cam;

    void Start()
    {
        cam = Camera.main;

        width = cam.orthographicSize * (float)Screen.width / Screen.height;
        height = cam.orthographicSize;
    }

    void Update()
    {
        Vector3 targetPoint = new Vector3(Target.position.x, Target.position.y, -10f);
        if (targetPoint.x + width < b_Max.position.x && targetPoint.x - width > b_Min.position.x)
            xPos = targetPoint.x;
        if (targetPoint.y + height < b_Max.position.y && targetPoint.y - height > b_Min.position.y)
            yPos = targetPoint.y;

        Vector3 newPos = new Vector3(xPos, yPos, -10f);
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }


}
