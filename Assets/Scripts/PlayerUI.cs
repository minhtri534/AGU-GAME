using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    private PlayerController player;
    public TMP_Text hpText;
    public TMP_Text mpText;

    void Update()
    {
        if (player == null) return;

        var stats = player.GetStats();

        if (stats == null) return;

        if (hpText != null)
            hpText.text = $"HP: {stats.CurrentHP:0}/{stats.MaxHP:0}";

        if (mpText != null)
            mpText.text = $"MP: {stats.CurrentMP:0}/{stats.MaxMP:0}";
    }

    public void SetPlayer(PlayerController p)
    {
        player = p;
    }
}