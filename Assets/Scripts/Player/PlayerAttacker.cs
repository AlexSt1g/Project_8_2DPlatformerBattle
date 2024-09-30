using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private int _damage = 50;
    [SerializeField] private float _attackRange = 1f;

    public float AttackRange => _attackRange;

    public void Attack(Enemy target)
    {        
        target.TakeHit(_damage);
    }
}
