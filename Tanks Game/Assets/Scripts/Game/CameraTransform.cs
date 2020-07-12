using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    [SerializeField] private float speed;

    [Header("Bounds")]
    public Transform b_Min;
    public Transform b_Max;

    float width, height;

    Camera cam;

    void Start()
    {
        cam = Camera.main;

        width = cam.orthographicSize * (float)Screen.width / Screen.height;
        height = cam.orthographicSize;
    }

    void Update()
    {

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 1f);

        transform.Translate(input * speed);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, b_Min.position.x + width, b_Max.position.x - width),
            Mathf.Clamp(transform.position.y, b_Min.position.y + height, b_Max.position.x - height),
            -10f);
    }


}
