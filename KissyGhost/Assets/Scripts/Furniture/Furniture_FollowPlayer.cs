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
    private Vector3 moveDir;
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

        moveDir = closestPlayerTransform.position - transform.position;
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
            isInitialized = false;
            furnitureRigidbody2D.velocity = Vector2.zero;

            if (closestPlayerTransform != null)
            {
                Vector3 furnitureToPlayerDir = closestPlayerTransform.position - transform.position;
                float distanceToPlayer = furnitureToPlayerDir.magnitude;

                if (distanceToPlayer <= minFollowDistance)
                {
                    closestPlayerTransform.GetComponent<Rigidbody2D>().AddForce(furnitureToPlayerDir.normalized * BounceBackForce);
                }
            }
        }
    }

    public void Initialize(Transform _closestPlayerTransform)
    {
        closestPlayerTransform = _closestPlayerTransform;
        isInitialized = true;
    }

    public Vector2 GetFurnitureMoveDir()
    {
        return moveDir;
    }
}
