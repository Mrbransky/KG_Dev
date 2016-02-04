using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
    public virtual Vector2 moveDir { get; set; }
    protected Rigidbody2D rigidBody;

    public Vector2 debugMoveDir;

    public float baseSpeed;
    public float currentSpeed;

    public virtual void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
    }
	
	public virtual void Update () 
    {
        if (moveDir != Vector2.zero && baseSpeed > 0)
            ApplyMovement();
#if UNITY_EDITOR
        else if (debugMoveDir != Vector2.zero && baseSpeed > 0)
        {
            Vector3 calc = new Vector3(debugMoveDir.x, debugMoveDir.y, 0) * currentSpeed * Time.deltaTime;
            this.rigidBody.transform.position += calc;
        }
#endif
    }

    protected void ApplyMovement()
    {
        Vector3 calc = new Vector3(moveDir.x, moveDir.y, 0) * currentSpeed * Time.deltaTime;
        this.rigidBody.transform.position += calc;
    }
}
