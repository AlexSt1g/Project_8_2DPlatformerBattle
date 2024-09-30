using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealingPotion : MonoBehaviour, ICollectable
{
    [SerializeField] private float _respawnTime = 60f;
    [SerializeField] private int _healingValue = 50;

    public event Action<ICollectable, float> Collected;

    public int HealingValue => _healingValue;    

    public void PickUp()
    {
        Collected?.Invoke(this, _respawnTime);
    }

    public void SetActive(bool activeStatus)
    {
        gameObject.SetActive(activeStatus);
    }
}
