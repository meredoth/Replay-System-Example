using UnityEngine;
using UnityEngine.InputSystem;

namespace Replay_Input_System.Replay_System
{
public class BallRefactored : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float range = 10f;
    [SerializeField] private float speed = 5f;

    private float _movementTimer;

    public float Range
    {
        get => range;
        set => range = value;
    }

    private void Update()
    {
        if(_movementTimer > 0f)
            _movementTimer -= Time.deltaTime;
    }

    public void ChangeColor() => sr.color = sr.color == Color.white ? Color.blue : Color.white;

    private void FixedUpdate()
    {
        if (_movementTimer <= 0f)
        {
            _movementTimer = 0f;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Gizmos.DrawLine(transform.position, worldMousePos);
        
        Gizmos.DrawWireSphere(transform.position, Range);
        
        var ballPos = rb.position;
        var vectorOfEndPoint = worldMousePos - ballPos;
        var centerEndPoint = vectorOfEndPoint.normalized * Range + ballPos;
      
        var xAngle = Mathf.Atan2(vectorOfEndPoint.y, vectorOfEndPoint.x);
        var xCos = Mathf.Cos(xAngle);
        var ySin = Mathf.Sin(xAngle);
        var edgeEndPoint = centerEndPoint - new Vector2(transform.localScale.x/2f * xCos, transform.localScale.x/2f * ySin);
        
        Gizmos.DrawWireSphere(edgeEndPoint, transform.localScale.x/2f);
    }
    
    public void Move(Vector2 pos)
    {
        var mass = rb.mass;
        var ballPos = rb.position;
        var vectorOfEndPoint = pos - ballPos;
        var centerEndPoint = vectorOfEndPoint.normalized * Range + ballPos;
      
        var xAngle = Mathf.Atan2(vectorOfEndPoint.y, vectorOfEndPoint.x);
        var xCos = Mathf.Cos(xAngle);
        var ySin = Mathf.Sin(xAngle);
        var edgeEndPoint = centerEndPoint - new Vector2(transform.localScale.x/2f * xCos, transform.localScale.x/2f * ySin);
        var distance = edgeEndPoint - ballPos;

        var travelTime = distance.magnitude / speed;
      
        var force = mass * distance / travelTime;
        
        rb.AddForce(force,ForceMode2D.Impulse);
        _movementTimer = travelTime;
    }
}
}
