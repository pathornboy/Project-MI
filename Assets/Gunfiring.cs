using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunfiring : MonoBehaviour
{
    private float nextActionTime;
    public float periodmin = 3.0f;
    public float periodmax = 5.0f;
    public GameObject Bullet;
    public int Gun =0;
    public bool target = false;
    private AudioManager audio;
    private EnemyHand enemyHand;
    // Start is called before the first frame update
    void Start()
    {
        nextActionTime = Time.time + periodmax;
        GameObject audior = GameObject.Find("AudioManager");
        audio = audior.GetComponent<AudioManager>();
        enemyHand = transform.parent.gameObject.GetComponent<EnemyHand>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime && target)
        {
            nextActionTime = Time.time + Random.Range(periodmin, periodmax);
            switch (Gun)
            {
                case 0:
                    audio.PlayClip(1);
                    Instantiate(Bullet, transform.position + transform.right *0.75f + transform.up * 0.1f, transform.rotation);
                    enemyHand.recoilforce = 175.0f;
                    enemyHand.firing = true;
                    enemyHand.firingframes = 0;
                    break;
                case 1:
                        nextActionTime += Random.Range(periodmin, periodmax);
                        enemyHand.recoilforce = 100.0f;
                        StartCoroutine(FireBurst(Bullet, 3, 4, 550.0f));
                    break;
                case 2:
                    nextActionTime += Random.Range(periodmin+1, periodmax+1);
                    audio.PlayClip(3);
                    enemyHand.recoilforce = 500.0f;
                    FirePellets(Bullet, 7, 8);
                    enemyHand.firing = true;
                    enemyHand.firingframes = 0;
                    break;
                default:
                    audio.PlayClip(1);
                    Instantiate(Bullet, transform.position, transform.rotation);
                    enemyHand.firing = true;
                    enemyHand.firingframes = 0;
                    break;
            }
        }

    }


    public IEnumerator FireBurst(GameObject bulletPrefab, int minburstSize,int maxburstSize ,float rateOfFire)
    {
        float bulletDelay = 60 / rateOfFire;
        int burstSize = Random.Range(minburstSize, maxburstSize);
        // rate of fire in weapons is in rounds per minute (RPM), therefore we should calculate how much time passes before firing a new round in the same burst.
        for (int i = 0; i < burstSize; i++)
        {
            GameObject bullet = Instantiate(Bullet, transform.position + transform.right * 1.25f + transform.up * 0.2f, transform.rotation); // It would be wise to use the gun barrel's position and rotation to align the bullet to.
            audio.PlayClip(1);
            enemyHand.firing = true;
            enemyHand.firingframes = 0;
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
