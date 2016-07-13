using UnityEngine;
using System.Collections;

public class Furniture_RhinoCharge : MonoBehaviour
{
    [SerializeField]
    private float minFollowDistance = 0.1f;
    [SerializeField]
    private float followSpeed = 0;
    public int BounceBackForce;

    private KissableFurniture _KissableFurniture;
    private Transform closestPlayerTransform;
    private Rigidbody2D furnitureRigidbody2D;
    private bool isInitialized = false;
    private Vector3 lastKnownPlayerPosition;

    public float shake = 0.5f;
    float shakeAmount = 0.05f;
    float decreaseFactor = 1.0f;
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
        ShakeyShake();
        if (shake <= 0)
        {
            followSpeed += Time.deltaTime * 2;
            furnitureRigidbody2D.velocity = (lastKnownPlayerPosition - transform.position) * followSpeed;
            if (followSpeed >= 3.5f)
            { followSpeed = 3.5f; }
        }
    }

    void OnDisable()
    {
        if (isInitialized)
        {
            isInitialized = false;
            followSpeed = 0;
            _KissableFurniture.amountKissed = 0;
            Vector3 furnitureToPlayerDir = closestPlayerTransform.position - transform.position;
            float distanceToPlayer = furnitureToPlayerDir.magnitude;

            if (distanceToPlayer <= minFollowDistance)
            {
                closestPlayerTransform.GetComponent<Rigidbody2D>().AddForce(furnitureToPlayerDir.normalized * BounceBackForce);
            }
        }
    }


    void OnCollisionEnter2D(Collision2D col) 
    {
        if(col.gameObject.tag == "Furniture" && isInitialized == true)
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce((col.transform.position - transform.position) * (BounceBackForce/2));
        }
    }

    public void Initialize(Transform _closestPlayerTransform)
    {
        bool savePos = true;
        closestPlayerTransform = _closestPlayerTransform;


        if (savePos)
        {
            lastKnownPlayerPosition = _closestPlayerTransform.position;
            savePos = false;
        }

        isInitialized = true;
    }

    void ShakeyShake()
    {
        if (shake > 0)
        {
            Vector3 rand = Random.insideUnitCircle * shakeAmount;
            transform.position = (rand + transform.position);
            shake -= Time.deltaTime * decreaseFactor;
            _KissableFurniture.kissedDuration = 3f;
        }
        else
        {
            shake = 0.0f;
        }
    }

    public Vector2 GetLastKnownPlayerPosition()
    {
        return lastKnownPlayerPosition;
    }
}
