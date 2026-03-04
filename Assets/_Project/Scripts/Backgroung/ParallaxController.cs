using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ParallaxLayer
{
    public GameObject background;
    [Range(0, 1)] public float parallaxEffect;
}

public class ParallaxController : MonoBehaviour
{
    [SerializeField] public List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();
    [SerializeField] private GameObject camera;

    private Dictionary<GameObject, float> startPositions = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, float> lengths = new Dictionary<GameObject, float>();

    void Start()
    {
        if (camera == null) camera = Camera.main.gameObject;

        foreach (ParallaxLayer pl in parallaxLayers)
        {
            if (pl.background == null) continue;

            startPositions[pl.background] = pl.background.transform.position.x;

            SpriteRenderer sr = pl.background.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                lengths[pl.background] = sr.bounds.size.x;
            }
        }
    }

    void LateUpdate()
    {
        foreach (ParallaxLayer pl in parallaxLayers)
        {
            if (pl.background == null) continue;

            float length = lengths[pl.background];
            float startPos = startPositions[pl.background];

            float temp = (camera.transform.position.x * (1 - pl.parallaxEffect));

            float dist = (camera.transform.position.x * pl.parallaxEffect);

            pl.background.transform.position = new Vector3(startPos + dist, pl.background.transform.position.y, pl.background.transform.position.z);

            if (temp > startPos + length)
                startPositions[pl.background] += length;
            else if (temp < startPos - length)
                startPositions[pl.background] -= length;
        }
    }
}
