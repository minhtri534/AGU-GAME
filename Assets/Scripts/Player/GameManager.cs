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
        GunComponentWorldObjectInstancer.Spawn(new ExampleBehaviourComponent(), new Vector3(0, 1, 0));
        GunComponentWorldObjectInstancer.Spawn(new GhostAmmoItem(), new Vector3(3, 1, 0));
        GunComponentWorldObjectInstancer.Spawn(new HomingAmmoItem(), new Vector3(4, 1, 0));
        var component = GunComponentRepository.GetRandomGunComponent();
        Debug.Log(component.GetType().Name);
        GunComponentWorldObjectInstancer.Spawn(component, new Vector3(0, 1, 3));
        // Spawn chest
        LootChestInstancer.Spawn(Resources.Load<LootData>("LootData/CommonChest"), new Vector3(-5, 1, -5));

        // Play music
        var music = Resources.Load<AudioClip>("Audio/Music/Cat with a gun");
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = music;
        source.loop = true;
        source.volume *= 0.5f;
        source.Play();
        
        // END OF TEST CODE
    }
}
