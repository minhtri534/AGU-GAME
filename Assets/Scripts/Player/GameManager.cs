using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject characterSelectPanel;
    public GameObject healerPrefab;
    public GameObject berserkerPrefab;
    public PlayerCamera playerCamera;
    public PlayerUI playerUI;

    public Transform spawnPoint;

    public EnemySpawner spawner;

    public void SpawnHealer()
    {
        SpawnPlayer(healerPrefab);
    }

    public void SpawnBerserker()
    {
        SpawnPlayer(berserkerPrefab);
    }

    void SpawnPlayer(GameObject prefab)
    {
        GameObject playerObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        PlayerController controller = playerObj.GetComponent<PlayerController>();

        playerCamera.SetPlayer(controller);
        playerUI.SetPlayer(controller);

        spawner.enabled = true;
        characterSelectPanel.SetActive(false);
    }
}
