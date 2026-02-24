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

        // WARNING: TEST CODE ONLY
        // THIS CODE SPAWNS AN EXAMPLE GUN COMPONENT IN THE WORLD WHEN THE GAME STARTS
        // REMOVE AFTER TESTING!!!!!!!!!

        // Get the inventory of the player and add it to the inventory manager
        GunInventoryManager.SelectedInventory = playerObj.GetComponent<Gun>().inventory;
        // Spawn an example gun component
        GameObject testComponent = Instantiate(Resources.Load<GameObject>("Prefabs/ComponentObject"), new Vector3(0, 1, 0), Quaternion.identity);
        GunComponentWorldObject c = testComponent.GetComponent<GunComponentWorldObject>();
        c.SetGunComponent(new ExampleBehaviourComponent());

        // Play music
        var music = Resources.Load<AudioClip>("Audio/Music/Cat with a gun");
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = music;
        source.loop = true;
        source.Play();
    }
}
