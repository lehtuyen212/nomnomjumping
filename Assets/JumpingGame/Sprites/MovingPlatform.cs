using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : PlatForm
{
    public float moveSpeed;
    private bool m_canMoveLeft;
    private bool m_canMoveRight;

    protected override void Start()
    {
        base.Start();
        float randCheck = Random.Range(0f, 1f);
        if(randCheck <= 0.5f)
        {
            m_canMoveLeft = true;
            m_canMoveRight = false;
        }else
        {
            m_canMoveLeft = false;
            m_canMoveRight = true;
        }
    }

    private void FixedUpdate()
    {
        float curSpeed = 0;
        if (!m_rb) return;
        if (m_canMoveLeft)
        {
            curSpeed = -moveSpeed;
        } else if (m_canMoveRight) 
        {
            curSpeed = moveSpeed;
        }
        m_rb.velocity = new Vector2(curSpeed, 0f);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(GameTag.LeftConner.ToString()))
        {
            m_canMoveLeft = false;
            m_canMoveRight = true;
        }
        if (col.CompareTag(GameTag.RightConner.ToString()))
        {
            m_canMoveLeft = true;
            m_canMoveRight = false;
        }
    }
}
