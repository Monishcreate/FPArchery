using UnityEngine;

public class Target : MonoBehaviour
{
    private int hp = 25;
    private int damageTaken = 0; // Track total non-bullseye damage
    public GameObject destructionEffect;

    public void TakeHit(int score)
    {
        if (score == 25 && hp == 25) // Fresh bullseye kill
        {
            GameManager.Instance.AddScore(100);
        }
        else if (score == 25) // Bullseye after partial damage
        {
            int adjustedScore = 100 - damageTaken;
            GameManager.Instance.AddScore(Mathf.Max(adjustedScore, 0));
        }
        else
        {
            GameManager.Instance.AddScore(score);
            damageTaken += score;
        }

        hp -= score;

        if (hp <= 0)
        {
            DestroyBoard();
        }
    }

    void DestroyBoard()
    {
        if (destructionEffect != null)
            Instantiate(destructionEffect, transform.position, Quaternion.identity);

        GameManager.Instance.OnTargetDestroyed();
        Destroy(gameObject);
    }
}

