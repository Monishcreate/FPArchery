using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int hp = 25;
    public bool isBullseyeHit = false;
    public GameObject destructionEffect;
    public GameObject triggerToNextLevel;

    public void TakeHit(int score)
    {
        if (score == 25) // Bullseye hit
        {
            isBullseyeHit = true;
            hp = 0;
        }
        else
        {
            hp -= score;
        }

        if (hp <= 0)
        {
            DestroyBoard();
        }
    }

    void DestroyBoard()
    {
        if (destructionEffect != null)
            Instantiate(destructionEffect, transform.position, Quaternion.identity);

        if (triggerToNextLevel != null)
            triggerToNextLevel.SetActive(true); // activates collider/trigger for next boards

        Destroy(gameObject);
    }
}
