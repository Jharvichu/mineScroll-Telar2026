using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public Transform player;
    private float spriteWidth;

    void Start()
    {
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
 
        if (player.position.x > transform.position.x + spriteWidth)
        {
            transform.position += new Vector3(spriteWidth * 2f, 0, 0);
        }
    }
}
