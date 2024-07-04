using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce;
    public float moveSpeed;
    private PlatForm m_platformLanded;
    private float m_movingLimitX;

    private Rigidbody2D m_rb;
    public PlatForm PlatformLanded { get => m_platformLanded; set => m_platformLanded=value; }
    public float MovingLimitX { get => m_movingLimitX;}
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MovingHandle();
    }

    public void Jump()
    {
        if (!GameManager.Ins || GameManager.Ins.state != GameState.Playing) return;
        if (!m_rb || m_rb.velocity.y > 0 || !m_platformLanded) return;
        if (m_platformLanded is BreakablePlatform)
        {
            m_platformLanded.PlatformAction();
        }
        m_rb.velocity = new Vector2(m_rb.velocity.x, jumpForce);
        if (AudioController.Ins)
        {
            AudioController.Ins.PlaySound(AudioController.Ins.jump);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
    }
    
    private void MovingHandle()
    {
        /* if (!GamepadController.Ins || !m_rb || !GameManager.Ins || GameManager.Ins.state != GameState.Playing) return;

         float horizontalInput = Input.GetAxis("Horizontal");

         // Nếu Player có thể di chuyển về bên trái
         if (horizontalInput < 0 || GamepadController.Ins.CanMoveLeft)
         {
             m_rb.velocity = new Vector2(-moveSpeed, m_rb.velocity.y);
         }
         // Nếu Player có thể di chuyển về bên phải
         else if (horizontalInput > 0 || GamepadController.Ins.CanMoveRight)
         {
             m_rb.velocity = new Vector2(moveSpeed, m_rb.velocity.y);
         }
         // Nếu không có phím nào được nhấn, dừng di chuyển
         else
         {
             m_rb.velocity = new Vector2(0, m_rb.velocity.y);
         }

         m_movingLimitX = Helper.Get2DCamSize().x / 2;*/
        if (!GamepadController.Ins || !m_rb || !GameManager.Ins || GameManager.Ins.state != GameState.Playing) return;
        if (GamepadController.Ins.CanMoveLeft)
        {
            m_rb.velocity = new Vector2(-moveSpeed, m_rb.velocity.y);
        }
        else if (GamepadController.Ins.CanMoveRight)
        {
            m_rb.velocity = new Vector2(moveSpeed, m_rb.velocity.y);
        }
        else
        {
            m_rb.velocity = new Vector2(0, m_rb.velocity.y);
        }
        m_movingLimitX = Helper.Get2DCamSize().x / 2;

        // Giữ Player không cho phép di chuyển vượt quá ranh giới màn hình
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -m_movingLimitX, m_movingLimitX),
            transform.position.y,
            transform.position.z);

        // Chặn xoay tròn bằng cách đặt constraints trong Rigidbody2D
        m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (GameManager.Ins && GameManager.Ins.Score > 200)
        {
            jumpForce = 14f;
        }
        if (GameManager.Ins && GameManager.Ins.Score > 300)
        {
            jumpForce = 16f;
        }
        if (GameManager.Ins && GameManager.Ins.Score > 400)
        {
            jumpForce = 18f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(GameTag.Collectable.ToString()))
        {
            var collectable = col.GetComponent<Collectable>();
            if (collectable)
            {
                collectable.Trigger();
            }
        }
        
    }
}
