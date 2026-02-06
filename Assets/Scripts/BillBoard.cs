using Unity.Mathematics;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera mainCamera;
    public bool isFlatMode = false;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (isFlatMode) // sprites copy camera rotation instead of looking at camera
        {
            transform.forward = mainCamera.transform.forward;
        }
        else {
            transform.LookAt(mainCamera.transform);
            var rotation = transform.rotation.eulerAngles;
            // lock rotation on y axis
            rotation.y = 0;
            rotation.x = -rotation.x; // fix mirrored x axis
            transform.rotation = Quaternion.Euler(rotation);
        }
        
        
    }
}
