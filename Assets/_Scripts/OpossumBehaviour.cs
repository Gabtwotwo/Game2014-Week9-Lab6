using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumBehaviour : MonoBehaviour
{

    public float runForce;
    public Transform lookAheadPoint;
    public Transform lookInFrontPoint;
    public LayerMask collisionGroundLayer;
    public bool isGroundAhead;
    public Rigidbody2D rigidbody2d;
    public LayerMask collisionWallLayer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _LookInFront();
        _LookAhead();
        _Move();
    }

    private void _LookAhead()
    {
        isGroundAhead = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionGroundLayer);

        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.green);
    }

    private void _LookInFront()
    {

        if (Physics2D.Linecast(transform.position, lookInFrontPoint.position, collisionWallLayer)){
            _FlipX();
        }



        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.red);

    }

    private void _Move()
    {

        if (isGroundAhead)
        {
            rigidbody2d.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);

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
