using UnityEngine;
using UnityEngine.Events;

public class RuntimeCharacterStats :  ICharacterStats
{
    public float MaxHP { get; }
    public float CurrentHP { get; private set; }

    public float MaxMP { get; }
    public float CurrentMP { get; private set; }

    public float Damage { get; private set; }
    public float Speed { get; }

    public UnityEvent IsDead = new();
    public UnityEvent IsHurt = new();

    public RuntimeCharacterStats(CharacterStatsData data)
    {
        MaxHP = data.maxHP;
        MaxMP = data.maxMP;
        Damage = data.damage;
        Speed = data.speed;

        CurrentHP = MaxHP;
        CurrentMP = MaxMP;
    }

    public void TakeDamage(float amount)
    {
        CurrentHP = Mathf.Max(0, CurrentHP - amount);
        IsHurt.Invoke();
        if (CurrentHP == 0)
        {
            IsDead.Invoke();
        }
    }

    public void UseMP(float amount)
    {
        CurrentMP = Mathf.Max(0, CurrentMP - amount);
    }

    public void SetDamage(float newDamage)
    {
        Damage = newDamage;
    }

    public void SetCurrentHP(float value)
    {
        CurrentHP = Mathf.Clamp(value, 0, MaxHP);
    }
}
