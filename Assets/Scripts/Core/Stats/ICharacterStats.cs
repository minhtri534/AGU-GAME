public interface ICharacterStats
{
    float MaxHP { get; }
    float CurrentHP { get; }

    float MaxMP { get; }
    float CurrentMP { get; }

    float Damage { get; }
    float Speed { get; }

    void TakeDamage(float amount);
    void UseMP(float amount);
}
