using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public MonoBehaviour Player;
    public float distance = 100;
    void Start()
    {
        transform.position = Player.transform.position + new Vector3(0, distance, distance);
        transform.rotation = Quaternion.Euler(45, 180, 0);
    }

    void Update()
    {
        var new_pos = Player.transform.position + new Vector3(0, distance, distance);
        transform.position = Vector3.Lerp(transform.position, new_pos, Time.deltaTime * 10);
    }
}
