using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnnemyControler : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float moveSpeed = 5f;
    public bool faceRight;
    public float angleToFall = 15f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move;

        if (faceRight)
        {
            move = Vector2.right * moveSpeed;
        }
        else
        {
            move = Vector2.left * moveSpeed;
        }
        move.y = rb2d.velocity.y;


        rb2d.velocity = move;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 dir = collision.GetContact(0).normal;
            var fromTop = Vector2.Dot(dir, Vector2.down);
            if(fromTop == 1)
            {
                transform.GetComponent<BoxCollider2D>().enabled = false;
                rb2d.AddTorque((angleToFall * Mathf.Deg2Rad) * rb2d.inertia, ForceMode2D.Impulse);
                collision.gameObject.GetComponent<CharacterController>().Jump();
            }
            else
            {
                StartCoroutine(PlayerDeath(collision.gameObject));
            }
            return;
        }

        Vector2 norm = collision.GetContact(0).normal;
        var res = Vector2.Dot(norm, Vector2.right);
        if(res == 1)
        {
            faceRight = true;
        }
        if(res == -1)
        {
            faceRight = false;
        }

    }

    IEnumerator PlayerDeath(GameObject player)
    {
        Camera.main.GetComponent<SmoothFollow>().deadPlayer = true;
        player.GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<Rigidbody2D>().AddTorque((angleToFall * Mathf.Deg2Rad) * rb2d.inertia, ForceMode2D.Impulse);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);
    }
}
