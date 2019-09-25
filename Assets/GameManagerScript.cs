using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject pickable;
    public GameObject Enemy;
    public GameObject Player;
    public float slowdown = 0.5f;
    private bool slowed = false;
    private float fixedDel;
    public bool loser = false;
    public int maxEnemy = 5;
    public int enemysLeft = 1;
    private float nextEnemySpawnTime;
    public float periodminE = 6.0f;
    public float periodmaxE = 10.0f;
    GameObject[] pauseObjects;
    GameObject[] loseObjects;
    // Start is called before the first frame update
    void Start()
    {
     Player =   GameObject.FindGameObjectWithTag("Player");
    fixedDel = Time.fixedDeltaTime;
    Time.timeScale = 1.0f;
    pauseObjects = GameObject.FindGameObjectsWithTag("Menu");
        loseObjects = GameObject.FindGameObjectsWithTag("Lose");
        hidePaused();

        nextEnemySpawnTime = Time.time + periodmaxE;
    }

    // Update is called once per frame
    void Update()
    {

        //EnemySpawner
        //if(enemysLeft <5 && Time.time > nextEnemySpawnTime)
        //{
        //    nextEnemySpawnTime = Time.time + Random.Range(periodminE, periodmaxE);
        //    Vector3 spawnPos = Player.transform.position;
        //    spawnPos.x+=10.0f;
        //    GameObject newEnemy = Instantiate(Enemy, spawnPos,Quaternion.identity) as GameObject;
        //    EnemyScript enemyScript = newEnemy.GetComponent<EnemyScript>();
        //    enemyScript.Instantiation();
        //    enemysLeft += 1;
        //}



        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;


        if (Input.GetKeyDown("[1]"))
        {
            Instantiate(pickable,target,transform.rotation);
        }

        if (Input.GetButtonDown("Slowmo") && !slowed && !loser)
        {
            Time.timeScale = slowdown;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            slowed = true;
        }



        if (Input.GetButtonUp("Slowmo") && slowed && !loser)
        {

            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = fixedDel;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.0f, 1.0f);
            slowed = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !loser)
        {
            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
                showPaused();
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                hidePaused();
            }
        }







    }




    //Reloads the Level
    public void Reload()
    {


        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = fixedDel;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0.0f, 1.0f);
        slowed = false;

        SceneManager.LoadScene(Application.loadedLevel);
    }

    //controls the pausing of the scene
    public void pauseControl()
    {
        if (Time.timeScale !=0)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in loseObjects)
        {
            g.SetActive(false);
        }
    }

    public void lose()
    {
        Time.timeScale = 0;
        loser = true;
        foreach (GameObject g in loseObjects)
        {
            g.SetActive(true);
        }
    }


    //loads inputted level
    public void LoadLevel(string level)
    {

        SceneManager.LoadScene(level);
    }


}
