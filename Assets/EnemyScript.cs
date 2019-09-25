using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject pickable;
    public GameObject blood;
    public float health = 100.0f;
    public bool alive = true;
    public GameObject targetobject;
    public float movespeed;
    private Rigidbody2D Rigid;
    private bool instatiated = false;
    public GameManagerScript GMS;
    //private RaycastHit2D hit;
    private RaycastHit2D[] hits;
    public bool blueonblue = false;
    public bool spotted = false;
    public int gunNo = 0;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void Instantiation()
    {
        GetComponentInChildren<EnemyHand>().Instantiation(gunNo);
        GameObject GM = GameObject.Find("GameManager");
        GMS = GM.GetComponent<GameManagerScript>();
        alive = true;
        targetobject = GameObject.FindGameObjectWithTag("Player");
        Rigid = GetComponent<Rigidbody2D>();
        instatiated = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!instatiated)
        { Instantiation(); }

        if (health <= 0.0f && alive)
        {
            die();
            alive = false;
        }
        else if(alive)
        {

            //hit = Physics2D.Linecast(transform.position, targetobject.transform.position,masker);
            //if (hit)
            //{
            //    //if (hit.collider.tag == "wall" || hit.collider.tag == "ground")
            //    //{
            //    //    Debug.Log("found wall/ground");
            //    //}

            //    //else
            //    //{
            //    //    Debug.Log("Found " + hit.collider.name);
            //    //}

            //    if (hit.collider.tag == "Player")
            //    {
            //        spotted = true;
            //        blueonblue = false;
            //    }
            //    else if (hit.collider.tag == "Enemy")
            //    {
            //        blueonblue = true;
            //        spotted = false;
            //    }
            //    else
            //    {
            //        blueonblue = false;
            //        spotted = false;
            //    }

            //    GetComponentInChildren<EnemyHand>().target = spotted;
            //    GetComponentInChildren<EnemyHand>().blueonblue = blueonblue;

            //}



            Vector3 target = targetobject.transform.position;
            Vector3 ParentPosition = transform.position;
            Vector3 Offsetter = target - ParentPosition;

            LayerMask mask = LayerMask.GetMask("hand");
            LayerMask mask2 = LayerMask.GetMask("pickable");
            LayerMask mask3 = LayerMask.GetMask("bullet");
            LayerMask mask4 = LayerMask.GetMask("gun");
            LayerMask masker = mask | mask2 | mask3 | mask4;
            masker = ~masker;


            hits = Physics2D.RaycastAll(ParentPosition,Offsetter,Vector2.Distance(ParentPosition,target), masker);
            //int i = 0;
            //foreach (RaycastHit2D hit in hits)
            //{
            //    ++i;
            //    Debug.Log("Hit " + i + " " + hit.collider.name);

            //}
            if (hits.Length > 0)
            {
                List<RaycastHit2D> hitList = new List<RaycastHit2D>(hits);
                hitList.Sort((y, x) => y.distance.CompareTo(x.distance));
                int i = 0;
                bool enemyHit = false;
                bool wallHit = false;
                bool playerHit = false;

                foreach (RaycastHit2D hit in hitList)
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        //Debug.Log("ENEMY");
                        enemyHit = true;
                    }
                    if (hit.collider.tag == "wall" || hit.collider.tag == "ground")
                    {
                        //Debug.Log("WALL");
                        wallHit = true;
                        break;
                    }
                    if (hit.collider.tag == "Player")
                    {
                        //Debug.Log("OH");
                        playerHit = true;
                        break;
                    }

                }
                if (enemyHit)
                {
                    if (playerHit)
                    {
                        GetComponentInChildren<EnemyHand>().target = true;
                        GetComponentInChildren<EnemyHand>().blueonblue = true;
                    }
                    if (wallHit)
                    {
                        GetComponentInChildren<EnemyHand>().target = false;
                        GetComponentInChildren<EnemyHand>().blueonblue = false;
                    }
                }
                else
                {
                    GetComponentInChildren<EnemyHand>().target = playerHit;
                    GetComponentInChildren<EnemyHand>().blueonblue = false;
                }
                }


            











            //Debug.Log("Offsetter Mag ="+Offsetter.magnitude);
            //if (Offsetter.magnitude >= 15.0f)
            //{
            //    Rigid.AddForce(Offsetter.normalized * movespeed * Time.deltaTime, ForceMode2D.Impulse);

            //}
            //else if (Offsetter.magnitude <= 5.0f)
            //{
            //    Rigid.AddForce(-1 * Offsetter.normalized * movespeed * Time.deltaTime, ForceMode2D.Impulse);
            //}
        }
    }

    void die()
    {
            GMS.enemysLeft -= 1;
            GameObject Hand = transform.GetChild(0).gameObject;
            Hand.transform.parent = null;
            Destroy(Hand, 1.0f);
        
        Destroy(gameObject, 2.0f);
    }


    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "pickable" || col.gameObject.tag == "Bullet" || col.gameObject.tag == "gun" || col.gameObject.tag == "melee")
        {
            if (!pickable)
            {
                pickable = col.gameObject;
               // Debug.Log("HITTTED");
            }




            if (pickable)
            {
                if (pickable.GetComponent<Rigidbody2D>().velocity.magnitude >= 1.0f)
                {

                    if (pickable.tag != "gun" && pickable.tag != "melee")
                        {
                        Destroy(pickable.GetComponent<Rigidbody2D>());
                        pickable.GetComponent<BoxCollider2D>().enabled = false;
                        pickable.transform.SetParent(gameObject.transform);
                        }
                    Instantiate(blood, pickable.transform.position, Quaternion.Inverse(pickable.transform.rotation));
                    pickable = null;
                    health -= 100.0f;
                }



            }
        }
    }

    //We can use trigger or Collision
    //void OnCollisionExit2D(Collision2D col)
    //{
    //    Debug.Log("EXITED");
    //    if (col.gameObject.tag == "pickable")
    //    {
    //    }
    //}
}
