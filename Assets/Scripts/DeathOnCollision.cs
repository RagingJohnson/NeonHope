using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathOnCollision : MonoBehaviour
{
    bool Dead = false;
    [SerializeField]float Cooldown = 1.5f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Death Collision");
            Dead = true;
            collision.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(!Dead)
        {
            return;
        }

        if(Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
        }
        else
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
