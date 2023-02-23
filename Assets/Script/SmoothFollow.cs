using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    GameObject player;
    BoxCollider2D bounds;

    public Vector3 newPos;
    public float smoothTime = 0.3f;
    Vector3 velocity;
    public bool deadPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bounds = GameObject.FindGameObjectWithTag("CameraBound").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!deadPlayer)
        {
            Vector2 pos = new Vector2(player.transform.position.x,player.transform.position.y);

            newPos = bounds.ClosestPoint(pos);
            newPos.z = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
        }
    }
}
