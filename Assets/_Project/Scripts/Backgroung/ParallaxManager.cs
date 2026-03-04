using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;

    private Transform cam;
    private Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    void LateUpdate()
    {
        float deltaX = cam.position.x - lastCamPos.x;

        transform.position += new Vector3(deltaX * parallaxSpeed, 0f, 0f);

        lastCamPos = cam.position;
    }
}