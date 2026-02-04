public interface ICharacterStats
{
    float MaxHP { get; }
    float CurrentHP { get; }

    float MaxMP { get; }
    float CurrentMP { get; }

    float Damage { get; }
    float Speed { get; }

    bool IsDead { get; }

    void TakeDamage(float amount);
    void UseMP(float amount);
}
