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
        //transform.Rotate(45, 0, 0); // rotation is off idk
        // lock rotation on y axis
        var rotation = transform.rotation.eulerAngles;
        rotation.y = 0;
        rotation.x = -rotation.x; // fix mirrored x axis
        transform.rotation = Quaternion.Euler(rotation);
    }
}
