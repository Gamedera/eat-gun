using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float dieTime = 5f;
    
    private void Start()
    {
        StartCoroutine(CountDownTimer());
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.tag == "Player") 
        {
            other.gameObject.GetComponent<PlayerCombat>().Die();
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Enemy") 
        {
            other.gameObject.GetComponent<EnemyBehaviour>().Die();
            Destroy(gameObject);
        }
    }

    private IEnumerator CountDownTimer() 
    {
        yield return new WaitForSeconds(dieTime);
        Destroy(gameObject);
    }
}
