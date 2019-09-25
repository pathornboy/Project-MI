using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OHH : MonoBehaviour
{
    public float armReach = 2.0f;
    public float controlReach = 8.0f;
    public GameObject pickable;
    public GameObject Bullet;
    public GameObject sword;
    public GameObject swordHold;
    public bool Equipsword = false;
    public bool canHold = true;
    public float handspeed = 20.0f;
    Color highlightcolor = Color.red;
    Color savedcolor = Color.red;
    private AudioManager audio;
    private GameManagerScript GMS;
    private float cooldown = 0.0f;
    private bool flipped = false;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(transform.parent.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        GameObject audior = GameObject.Find("AudioManager");
        audio = audior.GetComponent<AudioManager>();
        GameObject GM = GameObject.Find("GameManager");
        GMS = GM.GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //var Mouser = Input.mousePosition;
        //var ScreenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);

        //transform.position = transform.parent.gameObject.transform.position + new Vector3(1.5f, 1.5f, 0);

        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 ParentPosition = transform.parent.gameObject.transform.position;
        target.z = transform.position.z;
        Rigidbody2D rigider = GetComponent<Rigidbody2D>();

        Vector3 Offsetter = target - ParentPosition;

        //transform.position = ParentPosition - (Vector3.Normalize(ParentPosition - target) * armReach);
        if (Offsetter.magnitude <= controlReach)
        {
            //transform.position = target;
            //rigider.MovePosition(target);
            Vector3 NewTarget = ParentPosition - (Vector3.Normalize(ParentPosition - target) * (Offsetter.magnitude * armReach / controlReach));
            //rigider.MovePosition(NewTarget);

            rigider.velocity = ((NewTarget - transform.position) * handspeed);
            if(pickable)
            {
                if(pickable.tag =="melee")
                {
                    pickable.GetComponent<Rigidbody2D>().AddForce(rigider.velocity);
                }
            }

        }
        else if (Offsetter.magnitude > armReach)
        {

            Vector3 NewTarget = ParentPosition - (Vector3.Normalize(ParentPosition - target) * armReach);
            //rigider.MovePosition(NewTarget);

            rigider.velocity = ((NewTarget - transform.position) * handspeed);

        }

        var offset = new Vector2(target.x - ParentPosition.x, target.y - ParentPosition.y);
        var Angler = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        //rigider.MoveRotation(Angler);
        float zz = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Angler), Time.deltaTime * handspeed);


        if (Input.GetButton("Grab"))
        {
            if (pickable && canHold && !Equipsword)
                Pickup();
        }
        if (Input.GetButtonUp("Grab"))
        {
            if(!Equipsword)
            throw_drop();
        }
        if (Input.GetButtonDown("ChangeWeapon") && !Equipsword)
        {
            swordHold =Instantiate(sword, transform.position,transform.rotation,transform);
            Physics2D.IgnoreCollision(transform.parent.GetComponent<BoxCollider2D>(), swordHold.GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), swordHold.GetComponent<BoxCollider2D>());
            Equipsword = true;

            if(pickable && !canHold)
            {
                throw_drop();
            }
                canHold = false;
            pickable = swordHold;
        }
        else if (Input.GetButtonDown("ChangeWeapon") && Equipsword)
        {
            Destroy(swordHold);
            Equipsword = false;
            canHold = true;
        }



        if (pickable)
        {
            if (!canHold)
            {
                if (pickable.tag == "gun")
                {
                    pickable.transform.rotation = transform.rotation;
                    pickable.transform.position = transform.position + transform.right * 0.2f;
                    if (Time.time > cooldown)
                    {
                        if (Input.GetButton("EquipThrow"))
                            fireGun();
                    }



                }
                else if (pickable.tag == "melee")
                {
                    pickable.transform.position = transform.position;
                    pickable.transform.rotation = transform.rotation;

                }
                else
                {
                    pickable.transform.position = transform.position;
                }


            }
            else if(canHold)
            {

            }

            //if (pickable.tag == "melee")
            //    if (transform.rotation.eulerAngles.z > 90 && !flipped)
            //    {
            //        flipped = !flipped;
            //        Vector2 localScale = gameObject.transform.localScale;
            //        pickable.GetComponent<SpriteRenderer>().flipY = true;
            //        gameObject.transform.localScale = localScale;
            //    }
            //if (transform.rotation.eulerAngles.z <= 90 && flipped)
            //{
            //    flipped = !flipped;
            //    Vector2 localScale = gameObject.transform.localScale;
            //    pickable.GetComponent<SpriteRenderer>().flipY = false;
            //    gameObject.transform.localScale = localScale;
            //}



        }









    }

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    Debug.Log("ENTERED");
    //    if (col.gameObject.tag == "pickable")
    //    {
    //        if (!pickable) // if we don't have anything holding
    //        {
    //            pickable = col.gameObject;


    //            SpriteRenderer m_SpriteRenderer = pickable.GetComponent<SpriteRenderer>();
    //            //Set the GameObject's Color quickly to a set Color (blue)
    //            savedcolor = m_SpriteRenderer.color;
    //            m_SpriteRenderer.color = highlightcolor;



    //            //  pickable.GetComponent<Rigidbody2D>().isKinematic = true;
    //            //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
    //        }
    //    }
    //    }

    ////We can use trigger or Collision
    //void OnCollisionExit2D(Collision2D col)
    //{
    //    Debug.Log("EXITED");
    //    if (col.gameObject.tag == "pickable")
    //    {
    //        if (canHold)
    //        {
    //            SpriteRenderer m_SpriteRenderer = pickable.GetComponent<SpriteRenderer>();
    //            //Set the GameObject's Color quickly to a set Color (blue)
    //            m_SpriteRenderer.color = savedcolor;
    //            //  pickable.GetComponent<Rigidbody2D>().isKinematic = false;
    //            pickable = null;
    //            //gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

    //        }
    //    }
    //}


    public void Collided(Collider2D col)
    {
        // Debug.Log("ENTERED");
        if (col.gameObject.tag == "pickable" || col.gameObject.tag == "gun")
        {
            if (!pickable) // if we don't have anything holding
            {
                pickable = col.gameObject;
                Physics2D.IgnoreCollision(transform.parent.GetComponent<Collider2D>(), pickable.GetComponent<Collider2D>());

                SpriteRenderer m_SpriteRenderer;

                    m_SpriteRenderer = pickable.GetComponent<SpriteRenderer>();
                    savedcolor = m_SpriteRenderer.color;
                    m_SpriteRenderer.color = highlightcolor;




                //  pickable.GetComponent<Rigidbody2D>().isKinematic = true;
                //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }
    }

    //We can use trigger or Collision
    public void UnCollided(Collider2D col)
    {
        //Debug.Log("EXITED");
        if (col.gameObject.tag == "pickable" || col.gameObject.tag == "gun")
        {
            if (canHold)
            {

                    SpriteRenderer m_SpriteRenderer = pickable.GetComponent<SpriteRenderer>();
                    m_SpriteRenderer.color = savedcolor;
                    pickable = null;

            }
        }
    }







    private void Pickup()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        pickable.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        pickable.AddComponent<FixedJoint2D>();
        pickable.GetComponent<FixedJoint2D>().connectedBody = gameObject.GetComponent<Rigidbody2D>();





            SpriteRenderer m_SpriteRenderer = pickable.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.color = savedcolor;




        canHold = false;
    }

    private void throw_drop()
    {
        if (!pickable)
        {
            return;
        }
        pickable.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        pickable.GetComponent<Rigidbody2D>().AddTorque(-1*GetComponent<Rigidbody2D>().velocity.x * Time.timeScale/10, ForceMode2D.Impulse);
        Destroy(pickable.GetComponent<FixedJoint2D>());
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        pickable = null;
        canHold = true;
    }



    public void fireGun(){
        if(pickable)
        {
            if (pickable.tag == "gun")
                    {
                GunType gunType = pickable.GetComponent<GunType>();
                int Gun = gunType.gunType;
                if (gunType.AmmoLeft > 0)
                {
                    switch (Gun)
                    {
                        case 0:
                            audio.PlayClip(1);
                            cooldown = Time.time + 0.5f;
                            Vector2 Spawnposition = transform.position + transform.right * 0.75f + transform.up * 0.1f;
                            Instantiate(Bullet, Spawnposition, transform.rotation);
                            break;
                        case 1:
                            audio.PlayClip(1);
                            cooldown = Time.time + 60.0f/550.0f;
                            Instantiate(Bullet, transform.position + transform.right * 1.25f + transform.up * 0.2f, transform.rotation);
                            break;
                        case 2:
                            cooldown = Time.time + 1.5f;
                            audio.PlayClip(3);
                            FirePellets(Bullet, 7, 8);
                            break;
                        default:
                            audio.PlayClip(1);
                            Instantiate(Bullet, transform.position, transform.rotation);
                            break;
                    }
                    gunType.AmmoLeft -= 1;
                }
                else
                {
                    cooldown = Time.time + 0.5f;
                    audio.PlayClip(4);
                }





            }
        }
    }


    public IEnumerator FireBurst(GameObject bulletPrefab, int minburstSize, int maxburstSize, float rateOfFire)
    {
        float bulletDelay = 60 / rateOfFire;
        int burstSize = Random.Range(minburstSize, maxburstSize);
        // rate of fire in weapons is in rounds per minute (RPM), therefore we should calculate how much time passes before firing a new round in the same burst.
        for (int i = 0; i < burstSize; i++)
        {
            GameObject bullet = Instantiate(Bullet, transform.position+transform.right*1.25f, transform.rotation); // It would be wise to use the gun barrel's position and rotation to align the bullet to.
            audio.PlayClip(1);
            yield return new WaitForSeconds(bulletDelay); // wait till the next round
        }
    }

    public void FirePellets(GameObject bulletPrefab, int pelletSizeMin, int pelletSizeMax)
    {
        int pelletSize = Random.Range(pelletSizeMin, pelletSizeMax);
        // rate of fire in weapons is in rounds per minute (RPM), therefore we should calculate how much time passes before firing a new round in the same burst.
        for (int i = 0; i < pelletSize; i++)
        {
            GameObject bullet = Instantiate(Bullet, transform.position + transform.right * 2.5f + transform.up * 0.1f, transform.rotation); // It would be wise to use the gun barrel's position and rotation to align the bullet to.

            // yield return new WaitForSeconds(bulletDelay); // wait till the next round
        }
    }


}
