using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerAttacker))]
[RequireComponent(typeof(PlayerTargetDetector))]
[RequireComponent(typeof(PlayerSpawner))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    private const int MaxHealth = 100;

    private PlayerMover _mover;
    private InputReader _inputReader;
    private GroundDetector _groundDetector;
    private PlayerAnimationController _animator;
    private PlayerAttacker _attacker;
    private PlayerTargetDetector _targetDetector;
    private Enemy _target;
    private int _coinCount;
    private bool _isDead;

    public event Action<int> CoinCountChanged;
    public event Action Died;
    public event Action<int> HealthChanged;

    public int Health { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();
        _inputReader = GetComponent<InputReader>();
        _groundDetector = GetComponent<GroundDetector>();
        _animator = GetComponent<PlayerAnimationController>();
        _attacker = GetComponent<PlayerAttacker>();
        _targetDetector = GetComponent<PlayerTargetDetector>();
        Health = MaxHealth;
    }

    private void FixedUpdate()
    {
        if (_isDead == false)
        {
            if (_inputReader.Direction != 0)
                _mover.Move(_inputReader.Direction);

            if (_inputReader.GetIsJump() && _groundDetector.IsGround)
            {
                _mover.Jump();
                _animator.Jump();
            }

            if (_inputReader.GetIsAttack())
            {
                if (_targetDetector.TryGetTarget(_attacker.AttackRange, out _target))
                    _attacker.Attack(_target);

                _animator.Attack();
            }
        }
    }

    private void Update()
    {
        if (_isDead == false)
            _animator.UpdateMove(_inputReader.Direction != 0, _groundDetector.IsGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Coin>(out _))
        {
            AddCoin();
            CoinCountChanged?.Invoke(_coinCount);
        }

        else if (collision.TryGetComponent<DeathZone>(out _))
        {
            Health = 0;
            HealthChanged?.Invoke(Health);
            Die();
        }

        else if (collision.TryGetComponent<HealingPotion>(out HealingPotion healingPotion))
        {
            if (Health < MaxHealth)
            {
                if (healingPotion.HealingValue < 0)
                {
                    throw new InvalidOperationException(nameof(healingPotion.HealingValue));
                }
                else
                {
                    Health += healingPotion.HealingValue;
                    healingPotion.PickUp();
                }

                if (Health > MaxHealth)
                    Health = MaxHealth;

                HealthChanged?.Invoke(Health);
            }
            else
            {
                Debug.Log("�������� ������. ������� �� ���������.");
            }
        }
    }

    public void TakeHit(int damage)
    {
        _animator.TakeHit();

        if (damage < 0)
            throw new InvalidOperationException(nameof(damage));
        else if (damage > Health)
            Health = 0;
        else
            Health -= damage;

        HealthChanged?.Invoke(Health);

        if (Health == 0)
            Die();
    }

    public void Revive()
    {
        _isDead = false;
        _animator.SetLifeStatus(_isDead);

        Health = MaxHealth;
        HealthChanged?.Invoke(Health);
    }

    private void Die()
    {
        _isDead = true;
        _animator.SetLifeStatus(_isDead);

        Died?.Invoke();
    }

    private void AddCoin()
    {
        _coinCount++;
    }
}
