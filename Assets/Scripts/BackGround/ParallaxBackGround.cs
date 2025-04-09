using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float length;

    void Start()
    {
        cam = Camera.main.gameObject;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x; // 무한맵은 x로만 확장
    }

    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if(distanceMoved > xPosition + length)
        {
            xPosition = xPosition + length;
        }
        else if(distanceMoved < xPosition - length)
        {
            xPosition = xPosition - length;
        }
    }
}
