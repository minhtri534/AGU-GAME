using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    [Header("Resources Prefab Names")]
    public string playerPrefab = "Player";
    public string centerRoom;
    public string[] upRoom, downRoom, leftRoom, rightRoom;
    public string[] upLeftRoom, upRightRoom, downLeftRoom, downRightRoom;

    [Header("Settings")]
    public float roomSize;

    private Dictionary<Vector2Int, GameObject> rooms = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int currentRoomCoord = new Vector2Int(99, 99);

    void Awake() => instance = this;

    void Start()
    {
        // Nếu chưa kết nối, tiến hành kết nối từ đầu (Cho trường hợp test scene đơn lẻ)
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Đang kết nối Server...");
            PhotonNetwork.ConnectUsingSettings();
        }
        // Nếu đã kết nối nhưng chưa vào phòng
        else if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InRoom)
        {
            OnConnectedToMaster();
        }
        // Nếu đã ở trong phòng (trường hợp hiếm khi reload scene)
        else if (PhotonNetwork.InRoom)
        {
            OnJoinedRoom();
        }
    }

    // Khi kết nối thành công tới Master Server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Đã kết nối Master! Đang tìm phòng...");
        PhotonNetwork.JoinRandomRoom();
    }

    // Nếu không tìm thấy phòng nào (vì bạn đang test 1 mình)
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Không có phòng, đang tạo phòng mới...");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("<color=green>Đã vào phòng thành công!</color>");

        // Spawn Player
        //GameObject p = PhotonNetwork.Instantiate(playerPrefab, new Vector3(0, 2, 0), Quaternion.identity);

        // Gán tên ngẫu nhiên
        PhotonNetwork.LocalPlayer.NickName = "Tester_" + Random.Range(10, 99);

        if (PhotonNetwork.IsMasterClient)
        {
            // QUAN TRỌNG: Gọi hàm này để xây ô Center (0,0) ngay lập tức
            UpdateCurrentRoomByWorldPos(Vector3.zero);
        }
    }

    public void UpdateCurrentRoomByWorldPos(Vector3 worldPos)
    {
        Vector2Int targetCoord = new Vector2Int(
            Mathf.RoundToInt((worldPos.x / roomSize)),
            Mathf.RoundToInt((worldPos.z / roomSize))
        );

        if (targetCoord != currentRoomCoord)
        {
            if (PhotonNetwork.IsMasterClient) HandleRoomTransition(targetCoord);
            currentRoomCoord = targetCoord;
        }
    }

    private void HandleRoomTransition(Vector2Int targetCoord)
    {
        // 1. Lấy ngẫu nhiên tên Prefab từ mảng tương ứng
        string pName = GetRandomPrefabName(targetCoord);
        if (string.IsNullOrEmpty(pName)) return;

        if (!rooms.ContainsKey(targetCoord))
        {
            Vector3 finalPos = new Vector3(targetCoord.x * roomSize, 0, targetCoord.y * roomSize);
            GameObject newRoom = PhotonNetwork.Instantiate(pName, finalPos, Quaternion.identity);

            photonView.RPC("RPC_SetupRoom", RpcTarget.AllBuffered, targetCoord.x, targetCoord.y, newRoom.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    void RPC_SetupRoom(int x, int y, int viewID)
    {
        Vector2Int coord = new Vector2Int(x, y);
        PhotonView targetView = PhotonView.Find(viewID);

        if (targetView != null)
        {
            GameObject roomObj = targetView.gameObject;
            rooms[coord] = roomObj;

            Debug.Log($"Current'{roomObj.name}' (ViewID={viewID})");

            // 1. Chạy hiệu ứng trồi lên (DOTween) ngay tại đây
            Vector3 targetPos = roomObj.transform.position;
            roomObj.transform.position = targetPos + Vector3.down * 12f;
            roomObj.transform.DOMove(targetPos, 0.8f).SetEase(Ease.OutBack);
            roomObj.transform.rotation = Quaternion.Euler(0, -10f, 0);
            roomObj.transform.DORotate(Vector3.zero, 0.8f).SetEase(Ease.OutBack);

           
      
        }
    }
    private string PickRandom(string[] options)
    {
        if (options == null || options.Length == 0) return "";
        int index = Random.Range(0, options.Length);
        return options[index];
    }

    private string GetRandomPrefabName(Vector2Int c)
    {
        if(c.x == 0 && c.y == 0) return centerRoom;
        if (c.x == 0 && c.y == 1) return PickRandom(upRoom);
        if (c.x == 0 && c.y == -1) return PickRandom(downRoom);
        if (c.x == -1 && c.y == 0) return PickRandom(leftRoom);
        if (c.x == 1 && c.y == 0) return PickRandom(rightRoom);
        if (c.x == -1 && c.y == 1) return PickRandom(upLeftRoom);
        if (c.x == 1 && c.y == 1) return PickRandom(upRightRoom);
        if (c.x == -1 && c.y == -1) return PickRandom(downLeftRoom);
        if (c.x == 1 && c.y == -1) return PickRandom(downRightRoom);
        return "";
    }
}