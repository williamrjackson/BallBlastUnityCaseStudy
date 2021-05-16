using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    [SerializeField]
    bool isMovingRight;
    [SerializeField]
    float horizontalSpeed = 2.5f;
    [SerializeField]
    float peekY;
    [SerializeField]
    float airTime = 3f;
    [SerializeField]
    Wrj.Utils.MapToCurve bounceCurve;
    [SerializeField]
    BoxCollider2D floorCollider;
    [SerializeField]
    CircleCollider2D targetCollider;

    private Coroutine bounceRoutine;

    float bottomYPosition => floorCollider.transform.position.y + (floorCollider.transform.lossyScale.y * .5f) + targetCollider.radius;
    
    private void OnEnable()
    {
        TargetMoving = true;
    }
    private void OnDisable()
    {
        TargetMoving = false;
    }
    public bool TargetMoving
    {
        get
        {
            return bounceRoutine != null;
        }
        set
        {
            if (value)
            {
                bounceRoutine = StartCoroutine(BounceControlledYManager());
            }
            else if (bounceRoutine != null)
            {
                StopCoroutine(bounceRoutine);
                bounceRoutine = null;
            }
        }
    }

    void Update()
    {
        if(isMovingRight)
        {
            transform.position += Vector3.right * (horizontalSpeed * Time.deltaTime);
        }
        else
        {
            transform.position -= Vector3.right * (horizontalSpeed * Time.deltaTime);
        }
        transform.position = transform.position.With(y: bounceControlledY);
    }

    private void BounceProducedure()
    {
        // Cam shake, particle play, sound fx, etc.
        Debug.Log("Boing!!!");
    }
    private float bounceControlledY;

    private IEnumerator BounceControlledYManager()
    {
        while (true)
        {
            // Fall down
            yield return bounceCurve.ManipulateFloat(x => bounceControlledY = x, transform.position.y, bottomYPosition, airTime * .5f, mirrorCurve: true, onDone: () => 
            {
                // BounceProducedure();
            }).coroutine;
            // Bounce up...
            yield return bounceCurve.ManipulateFloat(x => bounceControlledY = x, bottomYPosition, peekY, airTime * .5f, mirrorCurve: false).coroutine;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Wall wall = other.GetComponent<Wall>();
        if (wall != null)
        {
            if (wall.wallType == Wall.WallType.Left)
            {
                Debug.Log("LeftWall");
                isMovingRight = true;
            }
            else if (wall.wallType == Wall.WallType.Right)
            {
                Debug.Log("RightWall");
                isMovingRight = false;
            }
        }
    }
}
