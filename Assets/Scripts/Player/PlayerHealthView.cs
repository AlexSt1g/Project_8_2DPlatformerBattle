using TMPro;
using UnityEngine;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Player _player;
    
    private void Start()
    {
        DisplayHealth(_player.Health);
    }

    private void OnEnable()
    {
        _player.HealthChanged += DisplayHealth;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= DisplayHealth;
    }

    private void DisplayHealth(int health)
    {
        _text.text = $"Player Health: {health}";
    }
}
