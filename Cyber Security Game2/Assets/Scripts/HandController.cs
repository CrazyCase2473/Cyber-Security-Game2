using UnityEngine;

public class HandController : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if(cam == null)
            Debug.LogError("no camera with MainCamera tag");
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = 10f;
        transform.position = cam.ScreenToWorldPoint(mousePos);
    }
}
