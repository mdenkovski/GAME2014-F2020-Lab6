using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBehaviour : MonoBehaviour
{
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;
    public bool IsGrounded;
    public bool IsJumping;
    public Transform SpawnPoint;

    private Rigidbody2D m_rigidbody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move(); 
    }

    void _Move()
    {
        if(joystick.Horizontal > joystickHorizontalSensitivity)
        {
            //move right
            m_rigidbody2D.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
            m_spriteRenderer.flipX = false;
            m_animator.SetInteger("AnimState", 1);
        }
        else if(joystick.Horizontal < -joystickHorizontalSensitivity)
        {
            //move left
            m_rigidbody2D.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
            m_spriteRenderer.flipX = true;
            m_animator.SetInteger("AnimState", 1);

        }
        else if(!IsJumping)
        {
           
                //idle
                m_animator.SetInteger("AnimState", 0);
           
        }

        if (joystick.Vertical > joystickVerticalSensitivity)
        {
            if (IsGrounded)
            {
                //jump
                m_rigidbody2D.AddForce(Vector2.up * verticalForce * Time.deltaTime);
                m_animator.SetInteger("AnimState", 2);
                IsJumping = true;
            }
        }
        else
        {
            IsJumping = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<TilemapCollider2D>() != null)
        {
            IsGrounded = true;
            m_animator.SetInteger("AnimState", 0);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<TilemapCollider2D>() != null)
        {
            IsGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("DeathPlane"))
        {
            //respawn
            _Respawn();
        }
    }

    private void _Respawn()
    {
        transform.position = SpawnPoint.position;
    }

}
