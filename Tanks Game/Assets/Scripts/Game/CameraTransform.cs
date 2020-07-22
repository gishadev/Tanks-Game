using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    #region Singleton
    public static CameraTransform Instance { private set; get; }
    #endregion

    public enum CameraModes
    {
        Freefly,
        Following
    }

    public CameraModes CameraMode
    {
        get { return mode; }
        set
        {
            mode = value;
            if (value == CameraModes.Following)
                UIManager.Instance.ShowFollowingBtn();
            else if (value == CameraModes.Freefly)
                UIManager.Instance.ShowFreeflyBtn();
        }
    }
    private CameraModes mode;

    [SerializeField] private float translateSpeed = 0.25f;
    [SerializeField] private float lerpSpeed = 25f;

    [Header("Bounds")]
    public Transform b_Min;
    public Transform b_Max;

    float width, height;

    Camera cam;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cam = Camera.main;
        mode = CameraModes.Following;

        width = cam.orthographicSize * (float)Screen.width / Screen.height;
        height = cam.orthographicSize;
    }
    void Update()
    {
        if (CameraMode == CameraModes.Freefly)
            FreeflyMode();
        else if (CameraMode == CameraModes.Following)
            FollowingMode(TurnsController.Instance.NowPlayer.SelectedUnit.transform);

        // Clamping.
        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, b_Min.position.x + width, b_Max.position.x - width),
        Mathf.Clamp(transform.position.y, b_Min.position.y + height, b_Max.position.x - height),
        -10f);
    }

    void FreeflyMode()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 1f);

        transform.Translate(input * translateSpeed);
    }

    void FollowingMode(Transform target)
    {
        transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime * lerpSpeed);
    }

    public void ChangeCameraMode(CameraModes _cameraMode)
    {
        CameraMode = _cameraMode;
    }
}
