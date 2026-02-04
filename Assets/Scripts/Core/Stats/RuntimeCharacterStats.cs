using UnityEngine;

public class RuntimeCharacterStats : ICharacterStats
{
    public float MaxHP { get; }
    public float CurrentHP { get; private set; }

    public float MaxMP { get; }
    public float CurrentMP { get; private set; }

    public float Damage { get; }
    public float Speed { get; }

    public bool IsDead => CurrentHP <= 0;

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
    }

    public void UseMP(float amount)
    {
        CurrentMP = Mathf.Max(0, CurrentMP - amount);
    }
}
