using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AutoConnect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("DevRoom", new RoomOptions { MaxPlayers = 5 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Đã vào phòng! Bây giờ bạn có thể nhấn chọn nhân vật.");
    }
}