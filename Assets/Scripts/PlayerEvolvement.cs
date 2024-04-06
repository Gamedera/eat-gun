using System.Collections.Generic;
using UnityEngine;

public class PlayerEvolvement : MonoBehaviour
{
    enum EvolvementStage {Circle, Legs};

    [SerializeField] private EvolvementStage evolvementStage;
    [SerializeField] private float circleMoveSpeed = 5f;
    [SerializeField] private float legsMoveSpeed = 8f;
    [SerializeField] private Vector2 circleColliderSize;
    [SerializeField] private Vector2 evolvedColliderSize;
    [SerializeField] private Vector2 evolvedColliderOffset;
    [SerializeField] private PhysicsMaterial2D zeroFrictionMaterial;
    [SerializeField] private List<GameObject> ears;
    [SerializeField] private List<GameObject> hands;
    [SerializeField] private List<GameObject> legs;

    private Rigidbody2D myRigidBody;
    private CircleCollider2D myCircleCollider;
    private PlayerMovement myPlayerMovement;
    private BoxCollider2D myBoxCollider;

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myPlayerMovement = GetComponent<PlayerMovement>();
        myCircleCollider = GetComponent<CircleCollider2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();

        switch (evolvementStage)
        {
            case EvolvementStage.Circle:
                CircleStage();
                break;
            case EvolvementStage.Legs:
                LegsStage();
                break;
        }
    }

    private void Update()
    {
        switch (evolvementStage)
        {
            case EvolvementStage.Circle:
                CircleStage();
                break;
            case EvolvementStage.Legs:
                LegsStage();
                break;
        }
    }

    public void Evolve()
    {
        if (evolvementStage != EvolvementStage.Circle)
        {
            Debug.LogWarning("Can't evolve anymore.");
            return;
        }
        PlayEvolveSound();
        evolvementStage = EvolvementStage.Legs;
    }

    private void CircleStage()
    {
        foreach (GameObject ear in ears)
        {
            ear.SetActive(false);
        }
        foreach (GameObject hand in hands)
        {
            hand.SetActive(false);
        }
        foreach (GameObject leg in legs)
        {
            leg.SetActive(false);
        }
        myCircleCollider.sharedMaterial = null;
        myBoxCollider.size = circleColliderSize;
        myBoxCollider.offset = new Vector2(0, 0);
        myRigidBody.constraints = RigidbodyConstraints2D.None;
        myPlayerMovement.CanJump(false);
        myPlayerMovement.SetMoveSpeed(circleMoveSpeed);
    }

    private void LegsStage()
    {
        foreach (GameObject ear in ears)
        {
            ear.SetActive(true);
        }
        foreach (GameObject hand in hands)
        {
            hand.SetActive(true);
        }
        foreach (GameObject leg in legs)
        {
            leg.SetActive(true);
        }
        myCircleCollider.sharedMaterial = zeroFrictionMaterial;
        myBoxCollider.size = evolvedColliderSize;
        myBoxCollider.offset = evolvedColliderOffset;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        myPlayerMovement.CanJump(true);
        myPlayerMovement.SetMoveSpeed(legsMoveSpeed);
    }

    private void PlayEvolveSound()
    {
        SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        if (soundPlayer == null)
        {
            Debug.LogWarning("SoundPlayer is null, sound will not work, add SoundPlayer to the scene.");
            return;
        }
        else
        {
            soundPlayer.PlayEvolveSound();
        }
    }
}
