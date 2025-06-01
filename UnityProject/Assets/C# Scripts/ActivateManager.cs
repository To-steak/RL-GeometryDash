using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class ActivateManager : MonoBehaviour
{
    public Form form = Form.Cube;

    public Vector2 flightForce;
    public float jumpForce;
    
    [Header("Ground Settings")]
    public LayerMask groundLayer;
    public float groundCheckRadius;

    public bool isGrounded;
    public bool isGravityInverted;
    
    private Rigidbody2D _rigidbody2D;
    private Dictionary<Form, Action> _formActions = new Dictionary<Form, Action>();
    private float _ufoCooldown = 0.0f;
    public enum Form
    {
        Cube, 
        Ball, 
        Ufo, 
        Ship // _rigidbody2D의 gravity를 3에서 2로 조정
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _formActions.Add(Form.Cube, Jump);
        _formActions.Add(Form.Ball, Roll);
        _formActions.Add(Form.Ufo, Bounce);
        // _formActions.Add(Form.Ship, Flight);
    }

    public void Activate()
    {
        if (_formActions.ContainsKey(form))
        {
            _formActions[form]?.Invoke();
        }
    }
    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Roll()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            isGravityInverted = !isGravityInverted;
            var value = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = isGravityInverted ? -value : value;
        }
    }

    private void FixedUpdate()
    {
        if (form == Form.Ufo)
        {
            _ufoCooldown += Time.deltaTime;
        }
    }

    private void Bounce()
    {
        if (_ufoCooldown >= 0.3f)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _ufoCooldown = 0.0f;
        }
    }

    // private void Flight()
    // {
    //     _rigidbody2D.AddForce(flightForce, ForceMode2D.Impulse);
    // }

    public void SetGravity(float x)
    {
        _rigidbody2D.gravityScale = x;
    }
}
