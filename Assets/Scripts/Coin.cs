using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private float _respawnTime = 20f;
    
    public event Action<ICollectable, float> Collected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out _))
            Collected?.Invoke(this, _respawnTime);
    }

    public void SetActive(bool activeStatus)
    {
        gameObject.SetActive(activeStatus);
    }
}
