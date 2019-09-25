using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OH : MonoBehaviour
{
    public GameObject gamemanager;
    public float speed = 5;
    public bool facingRight = true;
    public float jumpPower = 250;
    public float moveX;
    public float health = 100.0f;
    private float timeBetweenspawns ;
    public float starttime=0.05f;
    public GameObject Slowmoshadow;
    public bool alive = true;
    public GameObject throwweapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputer();
        //Slowmotrail
        if (Time.timeScale < 1.0f)
        {
            if (timeBetweenspawns <= 0.0f)
            {
                GameObject Slowmoshadower = (GameObject)Instantiate(Slowmoshadow, transform.position, transform.rotation);
                Destroy(Slowmoshadower, 1.0f);
                timeBetweenspawns = starttime;
            }
            else
            {
                timeBetweenspawns -= Time.deltaTime;
            }
        }


        if(health <=0.0f && alive)
        {
            alive=false;
            gamemanager.GetComponent<GameManagerScript>().lose();
        }


    }


    void PlayerInputer()
    {

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButton("EquipThrow") && Input.GetButtonDown("Grab"))
        {
            SpawnWeapon();
        }


        moveX = Input.GetAxis("Horizontal");
        //if(moveX < 0.0f && facingRight == false)
        //{
        //    Flip();
        //}
        //else if (moveX > 0.0f && facingRight == true)
        //{
        //    Flip();
        //}
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * speed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }
    void Jump()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce((Vector2.up * jumpPower)/Time.timeScale);



            }
    void SpawnWeapon()
    {

        Instantiate(throwweapon, transform.GetChild(0).position, transform.GetChild(0).rotation);
            


    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        gameObject.transform.localScale = localScale;
    }


}
