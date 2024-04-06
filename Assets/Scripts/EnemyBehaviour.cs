using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    enum AIType {Patrolling, Standing, Turning};

    [SerializeField] AIType aiType;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float timeBetweenShots = 1f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject pickUpGun;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float shootSpeed = 5f;
    [SerializeField] private float turnDellay = 1f;

    private GameObject player;
    private bool shouldMove = true;
    private bool canShoot = true;
    private bool shouldShoot = false;
    private float moveDirection = 1f;
    private bool canTurn = true;

    private CapsuleCollider2D myCapsuleCollider;
    private CircleCollider2D myCircleCollider;
    private BoxCollider2D myBoxCollider;
    private Animator myAnimator;
    private Rigidbody2D myRigidBody;

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myCircleCollider = GetComponent<CircleCollider2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        moveDirection = transform.localScale.x;
    }

    private void Update()
    {
        switch (aiType) {
            case AIType.Patrolling:
                PatrollingMovement();
                break;
            case AIType.Standing:
                StandingMovement();
                break;
            case AIType.Turning:
                TurningMovement();
                break;
        }

        player = GameObject.FindWithTag("Player");

        if(player == null) {
            shouldMove = true;
            return;
        }
   
        if(canShoot && shouldShoot) {
            StartCoroutine(Shoot());
        }
            
    }

    private void PatrollingMovement() 
    {
        if(shouldMove) {
            myAnimator.SetBool("isWalking", true);
            Move();
        } else {
            myAnimator.SetBool("isWalking", false);
            myRigidBody.velocity = new Vector2(0f, 0f);
        }
    }

    private void StandingMovement() 
    {
        
    }

    private void TurningMovement() 
    {
        if(canTurn) {
            StartCoroutine(Turn());
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(myCircleCollider.IsTouchingLayers(LayerMask.GetMask("Wall"))) {
            moveDirection *= -1;
            transform.localScale = new Vector2(moveDirection, transform.localScale.y);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
            if(IsFacingRight()) {
                if (gameObject.transform.position.x <= player.transform.position.x) {
                    shouldShoot = true;
                    shouldMove = false;
                }
            } else {
                if (gameObject.transform.position.x > player.transform.position.x) {
                    shouldShoot = true;
                    shouldMove = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (!myCircleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            moveDirection *= -1;
            transform.localScale = new Vector2(moveDirection, transform.localScale.y);
        }

        if(!myCircleCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
            shouldShoot = false;
            shouldMove = true;
        }
    }

    private bool IsFacingRight() 
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private void Move() 
    {
        if(IsFacingRight()) {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        } else {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f); 
        }
    }

    private IEnumerator Turn() 
    {
        shouldMove = false;
        canTurn = false;
        yield return new WaitForSeconds(turnDellay);
        moveDirection *= -1;
        transform.localScale = new Vector2(moveDirection, transform.localScale.y);
        shouldMove = true;
        canTurn = true;
    }

    private IEnumerator Shoot() 
    {
        canShoot = false;

        GameObject newBullet = Instantiate(bullet, shootPosition.position, Quaternion.identity);
        newBullet.transform.localScale = new Vector2(newBullet.transform.localScale.x * transform.localScale.x, newBullet.transform.localScale.y);
        

        if(IsFacingRight()) {
            newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * moveSpeed * Time.fixedDeltaTime, 0f);
        } else {
            newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * -moveSpeed * Time.fixedDeltaTime, 0f);
        }

        PlayLaserSound();

        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    public void Die() 
    {
        Destroy(gameObject);
        GameObject gun = Instantiate(pickUpGun, transform.position, Quaternion.identity);
    }

    public void StopMovement() 
    {
        shouldMove = false;
    }

    private void PlayLaserSound()
    {
        SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        if (soundPlayer == null)
        {
            Debug.LogWarning("SoundPlayer is null, sound will not work, add SoundPlayer to the scene.");
            return;
        }
        else
        {
            soundPlayer.PlayLaserSound();
        }
    }
}
