using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
    public virtual Vector2 moveDir { get; set; }
    protected Rigidbody2D rigidBody;

    public Vector2 debugMoveDir;
    private Vector2 cachedMoveDir;

    public float topSpeed;
    public float currentSpeed;

    public float accelRate, decelRate;

    public virtual void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        currentSpeed = 0;
    }
	
	public virtual void Update () 
    {
        if (moveDir != Vector2.zero && topSpeed > 0)
            ApplyMovement();

#if UNITY_EDITOR
        else if (debugMoveDir != Vector2.zero && topSpeed > 0)
        {
            Vector3 calc = new Vector3(debugMoveDir.x, debugMoveDir.y, 0) * currentSpeed * Time.deltaTime;
            this.rigidBody.transform.position += calc;
        }
#endif

        else if (Mathf.Abs(moveDir.magnitude) <= .15f && currentSpeed > 0)
            DecelToStop();
    }

    protected void ApplyMovement()
    {
        currentSpeed = AccelCurrentSpeed();

        Vector3 calc = new Vector3(moveDir.x, moveDir.y, 0) * currentSpeed * Time.deltaTime;
        cachedMoveDir = moveDir;
        this.rigidBody.transform.position += calc;
    }

    protected void DecelToStop()
    {
        currentSpeed = DecelCurrentSpeed();

        Vector3 calc = new Vector3(cachedMoveDir.x, cachedMoveDir.y, 0) * currentSpeed * Time.deltaTime;
        this.rigidBody.transform.position += calc;
    }

    protected float AccelCurrentSpeed()
    {
        if (currentSpeed < topSpeed)  
            return currentSpeed += accelRate;

        else if (currentSpeed > topSpeed)
            currentSpeed = topSpeed;

        return currentSpeed;
    }

    protected float DecelCurrentSpeed()
    {
        if (currentSpeed > 0)
            return currentSpeed -= decelRate;

        else if (currentSpeed < 0)
            currentSpeed = 0;

        return currentSpeed;
    }
}
