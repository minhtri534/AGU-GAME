using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject characterSelectPanel;
    public PlayerCamera playerCamera;
    public PlayerUI playerUI;
    public Transform spawnPoint;
    public EnemySpawner spawner;

    private string playerPrefabPath = "Prefabs/Player";

    public void SpawnHealer()
    {
        SpawnPlayer(CharacterClass.Healer);
    }

    public void SpawnBerserker()
    {
        SpawnPlayer(CharacterClass.Berserker);
    }

    void SpawnPlayer(CharacterClass character)
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Photon not connected!");
            return;
        }

        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoint.position, Quaternion.identity);

        var controller = playerObj.GetComponent<PlayerController>();
        photonView.RPC("SetupCharacterClassRPC", RpcTarget.AllBuffered, playerObj.GetComponent<PhotonView>().ViewID, (int)character);

        if (playerObj.GetComponent<PhotonView>().IsMine)
        {
            playerCamera.SetPlayer(controller);
            playerUI.SetPlayer(controller);

            var gun = playerObj.GetComponent<Gun>();
            if (gun != null)
            {
                GunInventoryManager.SelectedInventory = gun.inventory;
            }
        }

        spawner.enabled = PhotonNetwork.IsMasterClient;
        characterSelectPanel.SetActive(false);

        SetupAudio();
    }

    [PunRPC]
    void SetupCharacterClassRPC(int viewID, int characterIndex)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView == null) return;

        GameObject playerObj = targetView.gameObject;
        var controller = playerObj.GetComponent<PlayerController>();
        CharacterClass character = (CharacterClass)characterIndex;

        switch (character)
        {
            case CharacterClass.Berserker:
                controller.statsData = Resources.Load<PlayerStatsData>("CharacterStats/PlayerStats/Player_Berserker");
                if (playerObj.GetComponent<BerserkerSkill>() == null)
                    playerObj.AddComponent<BerserkerSkill>();
                break;
            case CharacterClass.Healer:
                controller.statsData = Resources.Load<PlayerStatsData>("CharacterStats/PlayerStats/Player_Healer");
                if (playerObj.GetComponent<HealerSkill>() == null)
                    playerObj.AddComponent<HealerSkill>();
                break;
        }
    }

    void SetupAudio()
    {
        var music = Resources.Load<AudioClip>("Audio/Music/Cat with a gun");
        if (GetComponent<AudioSource>() == null)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = music;
            source.loop = true;
            source.volume = 0.5f;
            source.Play();
        }
    }
}

public enum CharacterClass
{
    Berserker,
    Healer,
}