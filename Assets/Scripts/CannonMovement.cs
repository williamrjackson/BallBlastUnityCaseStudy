using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMovement : MonoBehaviour
{
	[SerializeField]
	private float smoothTime = 1f;
	[SerializeField]
	private float wheelRotationFactor = 90f;
	[SerializeField]
	private Transform wheel1;
	[SerializeField]
	private Transform wheel2;
	private Camera mainCam;
	
	private float targetX;
	private float vel = 0f;
	
    // Start is called before the first frame update
    void Start()
    {
	    mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
	    if (Input.GetMouseButton(0))
	    {
	    	Vector2 touchPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
	    	targetX = touchPosition.x;
	    }
	    float newXPosition = Mathf.SmoothDamp(transform.position.x, targetX, ref vel, smoothTime * Time.deltaTime);
	    Vector3 wheelRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.position.x * wheelRotationFactor);
	    wheel1.localEulerAngles = wheelRotation;
	    wheel2.localEulerAngles = wheelRotation;
	    transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }
}
