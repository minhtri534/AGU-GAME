using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public PlayerController player;
    public TMP_Text hpText;
    public TMP_Text mpText;

    void Update()
    {
        var stats = player.GetStats();

        hpText.text = $"HP: {stats.CurrentHP}/{stats.MaxHP}";
        mpText.text = $"MP: {stats.CurrentMP}/{stats.MaxMP}";
    }
}
