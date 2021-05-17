using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public bool isMovingRight = true;
    [SerializeField]
    private float horizontalSpeed = 5f;
    [SerializeField]
    private float peekY;
    [SerializeField]
    private float bounceGroundOffset = 0f;
    [SerializeField]
    private float bounceAirTime = 3f;
    [SerializeField]
    Wrj.Utils.MapToCurve bounceCurve;
    [SerializeField]
    BoxCollider2D floorCollider;
    [SerializeField]
    private CircleCollider2D targetCollider;
    private Coroutine bounceRoutine;
    private float rotationSpeed = 0f;

    private float BottomYPosition => floorCollider.transform.position.y + (floorCollider.transform.lossyScale.y * .5f) + targetCollider.radius + bounceGroundOffset;
    
    private void Awake() 
    {
        rotationSpeed = Random.Range(-15f, 15f);
    }

    public bool TargetMoving
    {
        get{
            return bounceRoutine != null;
        }
        set{
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
    
    private void OnEnable()
	{
        TargetMoving = true;
    }
    private void OnDisable()
    {
       TargetMoving = false;
    }

    void Update()
    {
        if (isMovingRight)
        {
            transform.position += Vector3.right * (horizontalSpeed * Time.deltaTime);
        }
        else
        {
            transform.position -= Vector3.right * (horizontalSpeed * Time.deltaTime);
        }
        Vector3 rotation = transform.localEulerAngles;
        rotation.z += rotationSpeed * Time.deltaTime;
        transform.localEulerAngles = rotation;

        transform.position = transform.position.With(y:bounceControlledY);
    }

    private void BounceProcedure()
    {
        // Bounce particles, sound effect, camerashake...
        Debug.Log("Boing!!!");
    }

    private float bounceControlledY;
    private IEnumerator BounceControlledYManager()
    {
        bounceControlledY = transform.position.y;
        while (true)
        {
            // Fall down..
            yield return bounceCurve.ManipulateFloat(x => bounceControlledY = x, transform.position.y, BottomYPosition, bounceAirTime * .5f, mirrorCurve:true, onDone:
            () =>
            {
                BounceProcedure();
            }).coroutine;
            // Bounce up...
            yield return bounceCurve.ManipulateFloat(x => bounceControlledY = x, BottomYPosition, peekY, bounceAirTime * .5f).coroutine;  
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Wall wall = other.GetComponent<Wall>();
        if (wall != null)
        {
            // Debug.Log("Hit " + wall.name);
            if (wall.wallType == Wall.WallType.Left)
            {
                isMovingRight = true;
            }
            else if (wall.wallType == Wall.WallType.Right)
            {
                isMovingRight = false;
            }
        }
    }
}
