using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    [SerializeField] private float speed;

    [Header("Bounds")]
    public Transform b_Min;
    public Transform b_Max;

    float width, height;
    float xPos, yPos;

    Vector3 newPos = new Vector3(0f,0f,-10f);

    Camera cam;

    void Start()
    {
        cam = Camera.main;

        width = cam.orthographicSize * (float)Screen.width / Screen.height;
        height = cam.orthographicSize;
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        //xPos += input.x * speed;
        //yPos += input.y * speed;

        //if (xPos + width < b_Max.position.x && xPos - width > b_Min.position.x)
        //    newPos.x = xPos;
        //if (yPos + height < b_Max.position.y && yPos - height > b_Min.position.y)
        //    newPos.y = yPos;


        //transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);

        transform.Translate(input * speed);
    }


}
