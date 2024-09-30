using TMPro;
using UnityEngine;

public class PlayerCoinCountView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Player _player;

    private void Awake()
    {
        DisplayCount(0);
    }

    private void OnEnable()
    {
        _player.CoinCountChanged += DisplayCount;
    }

    private void OnDisable()
    {
        _player.CoinCountChanged -= DisplayCount;
    }

    private void DisplayCount(int value)
    {
        _text.text = $"Coins: {value}";
    }
}
