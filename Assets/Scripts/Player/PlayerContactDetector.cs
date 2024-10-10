using System;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerContactDetector : MonoBehaviour
{
    private Player _player;

    public event Action CoinPickedUp;
    public event Action<int> HealingPotionPickedUp;
    public event Action<int> OnDeathZoneEnter;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<DeathZone>(out _))
        {
            OnDeathZoneEnter?.Invoke(_player.Health);
        }

        if (collision.TryGetComponent<ICollectable>(out _))
        {
            if (collision.TryGetComponent(out Coin coin))
            {
                coin.PickUp();
                CoinPickedUp?.Invoke();
            }

            else if (collision.TryGetComponent(out HealingPotion healingPotion))
            {
                if (_player.Health < Player.MaxHealth)
                {
                    healingPotion.PickUp();
                    HealingPotionPickedUp?.Invoke(healingPotion.HealingValue);
                }
                else
                {
                    Debug.Log("�������� ������. ������� �� ���������.");
                }
            }
        }
    }
}
