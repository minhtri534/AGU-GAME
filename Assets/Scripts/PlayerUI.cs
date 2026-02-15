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

        hpText.text = $"HP: {stats.CurrentHP}/{stats.MaxHP}";
        mpText.text = $"MP: {stats.CurrentMP}/{stats.MaxMP}";
    }

    public void SetPlayer(PlayerController p)
    {
        player = p;
    }
}
