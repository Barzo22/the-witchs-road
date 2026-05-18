using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    [SerializeField] private FloatingText _floatingTextPrefab;
    [SerializeField] private Canvas _canvas;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_OrbCollected, OnOrbCollected);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_OrbCollected, OnOrbCollected);
    }

    private void OnOrbCollected(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;

        float expGained = (float)parameters[0];
        Vector3 worldPos = (Vector3)parameters[1];
        Color orbColor = (Color)parameters[2];

        // Convertimos posición mundo a posición en pantalla
        Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);

        FloatingText text = Instantiate(_floatingTextPrefab, _canvas.transform);
        text.GetComponent<RectTransform>().position = screenPos;
        text.Initialize($"+{expGained} EXP", orbColor);
    }
}