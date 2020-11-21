using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RampDirection
{
    NONE,
    UP,
    DOWN
}


public class OpossumBehaviour : MonoBehaviour
{

    public float runForce;
    public Transform lookAheadPoint;
    public Transform lookInFrontPoint;
    public LayerMask collisionGroundLayer;
    public bool isGroundAhead;
    public Rigidbody2D rigidbody2d;
    public LayerMask collisionWallLayer;
    public bool onRamp;
    public RampDirection rampDirection;

    public LineOfSight opossumLOS;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rampDirection = RampDirection.NONE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (_hasLOS())
        {
            Debug.Log("Opossum can see the player");
        }
        _LookInFront();
        _LookAhead();
        _Move();
    }

    private bool _hasLOS()
    {
        if (opossumLOS.colliders.Count > 0)
        {

            if (opossumLOS.collidesWith.gameObject.name == "Player" && opossumLOS.colliders[0].gameObject.name == "Player")
            {
                return true;
            }
        }


        return false;
    }

    private void _LookInFront()
    {
        var wallHit = Physics2D.Linecast(transform.position, lookInFrontPoint.position, collisionWallLayer);
        if (wallHit){
            if (!wallHit.collider.CompareTag("Ramps"))
            {
                _FlipX();
                rampDirection = RampDirection.DOWN;
            }
            else
            {
                Debug.Log("Ramp up");
                rampDirection = RampDirection.UP;
            }

        }



        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.red);

    }

    private void _LookAhead()
    {
        var groundHit = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionGroundLayer);
        if (groundHit)
        {
            if (groundHit.collider.CompareTag("Ramps"))
            {


                onRamp = true;
            }

            if (groundHit.collider.CompareTag("Platforms"))
            {

                onRamp = false;
            }

            isGroundAhead = true;
        }
        else
        {
            isGroundAhead = false;
        }

        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.green);
    }

    private void _Move()
    {

        if (isGroundAhead)
        {
            rigidbody2d.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);

            if (onRamp)
            {
                if (rampDirection == RampDirection.UP)
                {
                    rigidbody2d.AddForce(Vector2.up * runForce * 0.5f * Time.deltaTime);
                }
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, -26.0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }


            rigidbody2d.velocity *= 0.90f;
        }
      
        else
        {
            _FlipX();
        }
    }
    


    private void _FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);

    }

}
