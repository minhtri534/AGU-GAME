using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    public List<WaveEnemiesData> Waves = new();
}