using UnityEngine;

public class TerrainSnap : MonoBehaviour
{
    public float heightOffset = 1.0f;
    public LayerMask terrainLayer; // Nên tạo một Layer tên là "Terrain" cho các Terrain

    void LateUpdate()
    {
        // Bắn một tia từ trên cao xuống dưới đất
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 10f; // Bắt đầu từ trên cao 10m

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 20f, terrainLayer))
        {
            // Kiểm tra xem thứ vừa đụng phải có phải là Terrain không
            Terrain terrain = hit.collider.GetComponent<Terrain>();

            if (terrain != null)
            {
                float terrainY = terrain.SampleHeight(transform.position) + terrain.transform.position.y;
                transform.position = new Vector3(transform.position.x, terrainY + heightOffset, transform.position.z);
            }
        }
    }
}