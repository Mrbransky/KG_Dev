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
        //Vector2 moveDir = closestPlayerTransform.position - transform.position;
        //moveDir.Normalize();
        //float distanceFromPlayer = moveDir.magnitude;
        followSpeed += Time.deltaTime*2;
        furnitureRigidbody2D.velocity = (lastKnownPlayerPosition - transform.position) * followSpeed;
        if(followSpeed >= 3.5f)
        { followSpeed = 3.5f; }
        //if (distanceFromPlayer > minFollowDistance)
        //{
        //    furnitureRigidbody2D.velocity = moveDir * followSpeed;
        //}
        //else
        //{
        //    furnitureRigidbody2D.velocity = Vector2.zero;
        //}
    }

    void OnDisable()
    {
        if (isInitialized)
        {
            isInitialized = false;
            //furnitureRigidbody2D.velocity = Vector2.zero;
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
        bool temp = true;
        closestPlayerTransform = _closestPlayerTransform;


        if(temp)
        {
            lastKnownPlayerPosition = _closestPlayerTransform.position;
            temp = false;
        }

        isInitialized = true;
    }

    public Vector2 GetLastKnownPlayerPosition()
    {
        return lastKnownPlayerPosition;
    }
}
