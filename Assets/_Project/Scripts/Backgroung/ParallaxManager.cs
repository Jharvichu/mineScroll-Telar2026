using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    private float starPosition, length;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;
    void Start()
    {
        starPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);
        transform.position = new Vector3(starPosition + distance, transform.position.y, transform.position.z);

        if (movement > starPosition + length)
        {
            starPosition += length;
        }
        else if (movement < starPosition - length)
        {
            starPosition -= length;
        }

    }
}