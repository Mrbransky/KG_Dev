using UnityEngine;
using System.Collections;

public class Furniture_FollowPlayer : MonoBehaviour
{
    [SerializeField] private float minFollowDistance = 0.1f;
    [SerializeField] private float followSpeed = 3.5f;
    public int BounceBackForce;

    private KissableFurniture _KissableFurniture;
    private Transform closestPlayerTransform;
    private Rigidbody2D furnitureRigidbody2D;
    private bool isInitialized = false;

    void Start()
    {
        _KissableFurniture = GetComponent<KissableFurniture>();
        furnitureRigidbody2D = GetComponent<Rigidbody2D>();
        this.enabled = false;
    }

    void Update()
    {
        if (!isInitialized)
        {
            return;
        }
        else if (closestPlayerTransform == null)
        {
            _KissableFurniture.UnkissFurniture();
            return;
        }

        Vector3 moveDir = closestPlayerTransform.position - transform.position;
        float distanceFromPlayer = moveDir.magnitude;
        moveDir.Normalize();

        if (distanceFromPlayer > minFollowDistance)
        {
            furnitureRigidbody2D.velocity = moveDir * followSpeed;
        }
        else
        {
            furnitureRigidbody2D.velocity = Vector2.zero;
        }
    }

    void OnDisable()
    {
        if (isInitialized)
        {
            furnitureRigidbody2D.velocity = Vector2.zero;
            Vector3 playVec = closestPlayerTransform.position;
            GetComponent<Rigidbody2D>().AddForce((transform.position - playVec).normalized * BounceBackForce);
            isInitialized = false;
        }
    }

    public void Initialize(Transform _closestPlayerTransform)
    {
        closestPlayerTransform = _closestPlayerTransform;
        isInitialized = true;
    }
}
