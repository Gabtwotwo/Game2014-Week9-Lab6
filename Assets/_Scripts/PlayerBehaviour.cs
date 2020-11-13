using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    public Transform spawnPoint;

    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        _Move();

    }




    void _Move()
    {

        if (isGrounded)
        {

            if(!isJumping && !isCrouching)
            {
                if (joystick.Horizontal > joystickHorizontalSensitivity)
                {
                    //right
                    _rigidBody.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                    _spriteRenderer.flipX = false;
                    _animator.SetInteger("AnimState", (int)PlayerAnimationType.RUN);
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    //left
                    _rigidBody.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    _spriteRenderer.flipX = true;
                    _animator.SetInteger("AnimState", (int)PlayerAnimationType.RUN);

                }
                else
                {
                    _animator.SetInteger("AnimState", (int)PlayerAnimationType.IDLE);

                }
            }



            if (joystick.Vertical > joystickVerticalSensitivity && (!isJumping))
            {
                //jump
                _rigidBody.AddForce(Vector2.up * verticalForce);
                _animator.SetInteger("AnimState", (int)PlayerAnimationType.JUMP);
                isJumping = true;
            }
            else
            {
                //idle
                isJumping = false;

            }

            if (joystick.Vertical < -joystickVerticalSensitivity && (!isCrouching))
            {
                //jump
                _animator.SetInteger("AnimState", (int)PlayerAnimationType.CROUCH);
                isCrouching = true;
            }
            else
            {
                //idle
                isCrouching = false;

            }
        }

       

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //respawn
        if (other.gameObject.CompareTag("deathPlane"))
        {
            transform.position = spawnPoint.position;
        }
    }
}
