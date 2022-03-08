using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Transform cam;
    private float value = 1;

    private void Start()
    {
        transform.localScale = Vector3.zero;
         cam = Camera.main.transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.forward + cam.position);
        slider.value = Mathf.Lerp(slider.value, value, 6 * Time.deltaTime);
    }

    public void Hide()
    {
        transform.DOScale(0, 0.4f);
    }


    public void Show()
    {
        transform.DOScale(1, 0.4f);
    }

    public void SetValue(float health, float max = 1)
    {
        value = health / max;
    }
}
