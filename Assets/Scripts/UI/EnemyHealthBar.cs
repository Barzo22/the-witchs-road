using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private Transform _canvasTransform;
    private Transform _cameraTransform;

    private void Awake()
    {
        _canvasTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (_cameraTransform == null) return;
        _canvasTransform.LookAt(
            _canvasTransform.position + _cameraTransform.forward
        );
    }

    public void UpdateHealth(float current, float max)
    {
        _fillImage.fillAmount = current / max;
    }
}