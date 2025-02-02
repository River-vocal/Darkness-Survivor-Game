using System.Collections;
using System.Collections.Generic;
using MyEventArgs;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject bossTreasure;
    [SerializeField] private GameObject obstacle;

    private int curHealth;
    public bool bossIsFlipped;
    public int attackDamage = 20;
    // Start is called before the first frame update
    public Transform playerTransform;

    public int underLightColor = 0;

    private Health health;

    private void Awake() {
        health = GetComponent<Health>();
        health.OnDamaged += health_OnDamaged;
        health.OnDead += health_OnDead;
    }

    void Start()
    {
        bossIsFlipped = false;
    }

    private void health_OnDamaged(object sender, System.EventArgs e)
    {
        int damage = ((IntegerEventArg) e).Value;

    }
    private void health_OnDead(object sender, System.EventArgs e)
    {
        //Analysis Data
        // GlobalAnalysis.is_boss_killed = true;

        int currLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (currLevelIndex == 6)
        {
                gameObject.SetActive(false);
            Invoke("GotoAllPassMenu", 1.5f);
        }
        else
        {
            Invoke("beatBoss", 0.5f);
        }
    }
    
    public void lookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1;
        if (transform.position.x > playerTransform.position.x && bossIsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            bossIsFlipped = false;
        }
        else if (transform.position.x < playerTransform.position.x && !bossIsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            bossIsFlipped = true;
        }
    }
    
    public void DirectionChange()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1;
        if (bossIsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            bossIsFlipped = false;
        }
        else if (!bossIsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            bossIsFlipped = true;
        }
    }

    void GotoAllPassMenu()
    {
        // allPassMenu.SetActive(true);
        Time.timeScale = 0f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void beatBoss()
    {
        // Instantiate(bossTreasure, bossFinalTransform.position, bossFinalTransform.rotation);
        gameObject.SetActive(false);
        bossTreasure.SetActive(true);
        // GameObject obstacle = GameObject.Find("Obstacle");
        // if (obstacle != null)
        // {
            // obstacle.SetActive(false);
        // }

        // wonMenu.SetActive(true);
        // Time.timeScale = 0f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
