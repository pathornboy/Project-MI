using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    public float armReach = 1.5f;
    public GameObject[] weapon;
    private bool flipped = false;
    public GameObject gun;
    public int Gun =0;
    public bool target = false;
    public bool blueonblue = false;
    public bool firing = false;
    public float recoilforce = 500.0f;
    public float handspeed = 1.5f;
    public bool instatiated = false;
    public int firingframes = 0;
    private int maxfiringframes = 10;
    public int Holdfireframes = 0;
    private int maxholdfireframes = 100;
    // Start is called before the first frame update
    void Start()
    {
        //gun = Instantiate(weapon[Random.Range(0, weapon.Length-1)],new Vector3(transform.position.x+0.3f, transform.position.y, transform.position.z), transform.rotation, transform);
        //Instantiation();
    }
    public void Instantiation()
    {
        int gunNo = Random.Range(0, weapon.Length);
        Gun = gunNo;
        gun = Instantiate(weapon[gunNo], new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z), transform.rotation, transform);
        gun.GetComponent<Gunfiring>().Gun = gunNo;
        instatiated = true;
    }

    public void Instantiation(int gunNo)
    {
        Gun = gunNo;
        gun = Instantiate(weapon[gunNo], new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z), transform.rotation, transform);
        gun.GetComponent<Gunfiring>().Gun = gunNo;
        instatiated = true;
    }

    public void DropWeapon(int gunNo)
    {
        Gun = gunNo;
        gun = Instantiate(weapon[gunNo], new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z),Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {
        //if(!instatiated)
        //{ Instantiation(); }

        if (transform.parent != null)
        {
            Vector3 ParentPosition = transform.parent.gameObject.transform.position;
            Rigidbody2D rigider = GetComponent<Rigidbody2D>();
            Vector3 targeter = GameObject.FindGameObjectWithTag("Player").transform.position;
            if (target)
            {
                targeter.z = transform.position.z;

                Vector3 Offsetter = targeter - ParentPosition;
                //transform.position = ParentPosition - (Vector3.Normalize(ParentPosition - target) * armReach);
                Vector3 NewTarget = ParentPosition - (Vector3.Normalize(ParentPosition - targeter) * armReach);
                //rigider.MovePosition(NewTarget);
                rigider.velocity = ((NewTarget - transform.position) * handspeed);
            }
            else
            {
                if (Holdfireframes < 0)
                {
                    Vector3 NewTarget = ParentPosition + (new Vector3(-1.0f, 0.0f, 0.0f) * armReach/2.0f);
                    rigider.velocity = ((NewTarget - transform.position) * handspeed);
                }
                else
                {
                    Holdfireframes--;
                    Vector3 Offsetter = targeter - ParentPosition;
                    Vector3 NewTarget = ParentPosition - (Vector3.Normalize(ParentPosition - targeter) * armReach);
                    rigider.velocity = ((NewTarget - transform.position) * handspeed);
                }
            }

            if (gun)
            {
                if (target && !blueonblue)
                {
                    if (Holdfireframes > maxholdfireframes)
                    {
                        gun.GetComponent<Gunfiring>().target = true;
                    }
                    else
                    {
                        Holdfireframes++;
                    }
                }
                else if (target && blueonblue)
                {
                    Holdfireframes = 0;
                    gun.GetComponent<Gunfiring>().target = false;
                }
                else if (!target)
                {
                    gun.GetComponent<Gunfiring>().target = false;
                }
            }



            if (firing)
            {
                if (flipped)
                {
                    rigider.AddForce(((ParentPosition - transform.position).normalized + transform.up * -0.5f) * recoilforce, ForceMode2D.Force);
                    var Anglerer = Mathf.Atan2(1.0f, 0) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Anglerer), Time.deltaTime * handspeed*0.1f);
                }
                else
                {
                    rigider.AddForce(((ParentPosition - transform.position).normalized + transform.up * 0.5f) * recoilforce, ForceMode2D.Force);
                    var Anglerer = Mathf.Atan2(1.0f, 0) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Anglerer), Time.deltaTime * handspeed * 0.1f);
                }
                if (firingframes > maxfiringframes)
                {
                    firingframes = 0;
                    firing = false;
                }
                else
                {
                    ++firingframes;
                }
            }


            else if (target){
                
                var offset = new Vector2(targeter.x - ParentPosition.x, targeter.y - ParentPosition.y);
                var Angler = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Angler), Time.deltaTime * handspeed * 1.5f);
            }
            else
            {
                if (Holdfireframes < 0)
                {
                    var Angler = Mathf.Atan2(1.0f, 0.0f) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Angler), Time.deltaTime * handspeed * 1.5f);
                }
                else
                {
                    var offset = new Vector2(targeter.x - ParentPosition.x, targeter.y - ParentPosition.y);
                    var Angler = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Angler), Time.deltaTime * handspeed * 1.5f);
                }
            }
                //rigider.MoveRotation(Angler);
            //transform.rotation = Quaternion.Euler(0, 0, Angler);

            if (transform.rotation.eulerAngles.z > 90 && !flipped)
            {
                flipped = !flipped;
                Vector2 localScale = gameObject.transform.localScale;
                gun.GetComponent<SpriteRenderer>().flipY = true;
                gameObject.transform.localScale = localScale;
            }
            if (transform.rotation.eulerAngles.z <= 90 && flipped)
            {
                flipped = !flipped;
                Vector2 localScale = gameObject.transform.localScale;
                gun.GetComponent<SpriteRenderer>().flipY = false;
                gameObject.transform.localScale = localScale;
            }
        }
    }
}
