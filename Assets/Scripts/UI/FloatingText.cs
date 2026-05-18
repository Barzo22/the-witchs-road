using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _floatSpeed = 1f;
    [SerializeField] private float _duration = 1f;

    private float _elapsed = 0f;
    private Color _startColor;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    public void Initialize(string message, Color color)
    {
        _text.text = message;
        _text.color = color;
        _startColor = color;
        _elapsed = 0f;
    }

    private void Update()
    {
        _elapsed += Time.deltaTime;

        transform.position += Vector3.up * _floatSpeed * Time.deltaTime;

        float alpha = 1f - (_elapsed / _duration);
        _text.color = new Color(_startColor.r, _startColor.g, _startColor.b, alpha);

        if (_elapsed >= _duration)
            gameObject.SetActive(false);
    }
}