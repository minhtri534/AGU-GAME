using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private PlayerController player;

    public float distance = 100;

    void Update()
    {
        if (player == null) return;

        var new_pos = player.transform.position + new Vector3(0, distance, distance * -0.75f);
        transform.position = Vector3.Lerp(transform.position, new_pos, Time.deltaTime * 10);
    }


    public void SetPlayer(PlayerController p)
    {
        player = p;
    }
}
