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
    [Header("Configuración de Seguimiento")]
    [Tooltip("El objeto que el parallax seguirá (Cámara, Jugador, etc.)")]
    public List<Transform> availableTargets = new List<Transform>();

    [Header("Capas")]
    [SerializeField] public List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    private Transform _currentTarget;
    private Dictionary<GameObject, float> startPositions = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, float> lengths = new Dictionary<GameObject, float>();

    void Start()
    {
        UpdateActiveTarget();

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
        if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy)
        {
            UpdateActiveTarget();
        }

        if (_currentTarget == null) return;

        float targetPosX = _currentTarget.position.x;

        foreach (ParallaxLayer pl in parallaxLayers)
        {
            if (pl.background == null) continue;

            float length = lengths[pl.background];
            float startPos = startPositions[pl.background];

            float temp = (targetPosX * (1 - pl.parallaxEffect));
            float dist = (targetPosX * pl.parallaxEffect);

            pl.background.transform.position = new Vector3(
                startPos + dist,
                pl.background.transform.position.y,
                pl.background.transform.position.z
            );

            if (temp > startPos + length)
                startPositions[pl.background] += length;
            else if (temp < startPos - length)
                startPositions[pl.background] -= length;
        }
    }

    private void UpdateActiveTarget()
    {
        foreach (Transform t in availableTargets)
        {
            if (t != null && t.gameObject.activeInHierarchy)
            {
                _currentTarget = t;
                return;
            }
        }

        if (Camera.main != null) _currentTarget = Camera.main.transform;
    }
}
