using Unity.Mathematics;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(90, 0, 0); // rotation is off idk
        // lock rotation on y axis
        var rotation = transform.rotation.eulerAngles;
        rotation.y = 0;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
