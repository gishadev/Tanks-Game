using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image healthUI;

    Transform target;
    Camera cam;

    public void SetTarget(Transform _target)
    {
        target = _target;
        SetPosition(target.position);
    }

    void Awake()
    {
        healthUI = GetComponent<Image>();
        cam = Camera.main;
    }

    void Update()
    {
        if (target != null)
            SetPosition(target.position);
    }

    public float Percentage
    {
        get { return percentage; }
        set { percentage = Mathf.Clamp01(value); }
    }

    float percentage;

    void UpdateHealthUI()
    {
        healthUI.fillAmount = Percentage;
    }

    public void UpdatePercentage(float current, float max)
    {
        Percentage = current / max;
        UpdateHealthUI();
    }

    public void SetPosition(Vector2 worldPosition)
    {
        Vector2 screenPos = cam.WorldToScreenPoint(worldPosition);
        transform.position = screenPos;
    }
}
