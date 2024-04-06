using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject teeth;

    private GameObject enemyToEat;
    private GameObject itemToEat;

    private BoxCollider2D myBoxCollider;
    private Animator myAnimator;
    private PlayerEvolvement myPlayerEvolvement;

    private void Start()
    {
        myBoxCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        myPlayerEvolvement = GetComponent<PlayerEvolvement>();
    }

    private void Update() 
    {
        ShowOrHideTeeth();
        RemoveUnreachableEatingTargets();
    }

    private void OnEat() 
    {
        if (enemyToEat != null)
        {
            TriggerEatAnimationAndSound();
            StartCoroutine(EatEnemy());
            return;
        }

        if (itemToEat != null)
        {
            if (itemToEat.GetComponent<ItemPickup>().ShouldPlayerEvolve())
            {
                myPlayerEvolvement.Evolve();
            }
            TriggerEatAnimationAndSound();
            StartCoroutine(EatItem());
            return;
        }
    }

    private void RemoveUnreachableEatingTargets()
    {
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            enemyToEat = null;
        }

        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("PickupItem")))
        {
            itemToEat = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.transform.localScale.x > Mathf.Epsilon)
            {
                if (gameObject.transform.position.x <= other.gameObject.transform.position.x)
                {
                    enemyToEat = other.gameObject;
                }
            }
            else
            {
                if (gameObject.transform.position.x > other.gameObject.transform.position.x)
                {
                    enemyToEat = other.gameObject;
                }
            }
        }

        if (other.gameObject.tag == "PickupItem")
        {
            itemToEat = other.gameObject;
        }

        if (other.gameObject.tag == "Death")
        {
            Die();
        }
    }

    private IEnumerator EatEnemy()
    {
        GameObject tempEnemy = enemyToEat;
        enemyToEat = null;
        tempEnemy.GetComponent<EnemyBehaviour>().StopMovement();
        yield return new WaitForSeconds(0.1f);
        tempEnemy.GetComponent<EnemyBehaviour>().Die();
    }

    private IEnumerator EatItem()
    {
        GameObject tempItem = itemToEat;
        itemToEat = null;
        yield return new WaitForSeconds(0.1f);
        tempItem.GetComponent<ItemPickup>().ProcessItemPickup();        
    }

    private void TriggerEatAnimationAndSound()
    {
        myAnimator.SetTrigger("eat");

        SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        if (soundPlayer == null)
        {
            Debug.LogWarning("SoundPlayer is null, sound will not work, add SoundPlayer to the scene.");
            return;
        }
        else
        {
            soundPlayer.PlayEatSound();
        }
    }

    private void ShowOrHideTeeth()
    {
        if (enemyToEat != null || itemToEat != null)
        {
            teeth.SetActive(true);
        }
        else
        {
            teeth.SetActive(false);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        ProcessPlayerDeath();
    }

    public void ProcessPlayerDeath()
    {
        GameSession gameSession = FindObjectOfType<GameSession>();
        if (gameSession == null)
        {
            Debug.LogError("GameSession is null, add GameSession to the scene.");
            return;
        }

        gameSession.ProcessPlayerDeath();
    }
}
