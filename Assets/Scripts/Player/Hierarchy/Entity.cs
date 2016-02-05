using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
    public virtual Vector2 moveDir { get; set; }
    protected Rigidbody2D rigidBody;

    private Vector2 cachedMoveDir;

    public float topSpeed;
    public float currentSpeed;

    public float accelRate, decelRate;

#region Keyboard Input Related Variables (for Debugging)
#if UNITY_EDITOR
    public Vector2 debugMoveDir;
    private float debugCurrentSpeed;
#endif
#endregion

    public virtual void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        currentSpeed = 0;
    }
	
	public virtual void Update () 
    {
        if (moveDir != Vector2.zero && topSpeed > 0)
            ApplyMovement();

        else if (Mathf.Abs(moveDir.magnitude) <= .15f && currentSpeed > 0)
            DecelToStop();

#region Keyboard Input Related Code (for Debugging)
#if UNITY_EDITOR
        else if (debugMoveDir != Vector2.zero && topSpeed > 0)
        {
            // AccelCurrentSpeed
            if (debugCurrentSpeed < topSpeed)
            {
                debugCurrentSpeed += accelRate;
            }
            else if (debugCurrentSpeed > topSpeed)
            {
                debugCurrentSpeed = topSpeed;
            }

            // ApplyMovement
            Vector3 calc = new Vector3(debugMoveDir.x, debugMoveDir.y, 0) * debugCurrentSpeed * Time.deltaTime;
            cachedMoveDir = debugMoveDir;
            this.rigidBody.transform.position += calc;
        }
        else if (Mathf.Abs(debugMoveDir.magnitude) <= .15f && currentSpeed > 0)
        {
            // DecelCurrentSpeed
            if (debugCurrentSpeed > 0)
            {
                debugCurrentSpeed -= decelRate;
            }
            else if (debugCurrentSpeed < 0)
            {
                debugCurrentSpeed = 0;
            }

            // DecelToStop
            Vector3 calc = new Vector3(cachedMoveDir.x, cachedMoveDir.y, 0) * debugCurrentSpeed * Time.deltaTime;
            this.rigidBody.transform.position += calc;
        }
#endif
#endregion
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
