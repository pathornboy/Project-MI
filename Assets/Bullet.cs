using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float Bulletspeed = 850.0f;  // Start is called before the first frame update
    private Rigidbody2D rigid;
    private BoxCollider2D boxer;
    public bool hit = false;
    public float spinspeed = 5.0f;
    private AudioManager audio;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxer = GetComponent<BoxCollider2D>();
        Vector3 v3Force = (Bulletspeed / Time.timeScale) * transform.right;
        rigid.AddForce(v3Force);

        GameObject audior = GameObject.Find("AudioManager");
        audio = audior.GetComponent<AudioManager>();

    }

    // Update is called once per frame
    void Update() {

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "hand" || col.gameObject.tag == "pickable" || col.gameObject.tag == "melee")
        {
            //Debug.Log("ENTERED");
            rigid.AddTorque(0.2f, ForceMode2D.Impulse);
            rigid.AddForce(50.0f* -rigid.velocity, ForceMode2D.Force);
            hit = true;
            audio.PlayClip(2);
        }
        if (col.gameObject.tag == "Player" && !hit)
        {
            audio.PlayClip(0);
            col.gameObject.GetComponent<OH>().health -= 50.0f;
            Destroy(GetComponent<BoxCollider2D>());
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 2.0f);
        }

        if (col.gameObject.tag == "Enemy" && hit)
        {
            audio.PlayClip(0);
            Destroy(GetComponent<BoxCollider2D>());
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 2.0f);
        }

        if ((col.gameObject.tag == "ground" || col.gameObject.tag == "wall") && !hit)
        {
           // Vector2 orthogonalVector = col.GetContact(0).point - (Vector2)transform.position;
            float angler = Vector2.Angle(col.GetContact(0).normal, rigid.velocity);
           // Debug.Log("HIT ANGLE= " + angler);
            if (angler < 60)
            {
                audio.PlayClip(0);
                Destroy(gameObject);
            }
        }


    }
}
