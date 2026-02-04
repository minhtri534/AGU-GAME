using UnityEngine;

public abstract class CharacterStatsData : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHP = 100;
    public float maxMP = 50;
    public float damage = 10;
    public float speed = 5;
}
