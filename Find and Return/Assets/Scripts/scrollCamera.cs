using UnityEngine;
using System.Collections;

public class scrollCamera : MonoBehaviour {

	public float cameraSpeed = 9.0f;

	public float distanceMin = 20;
	public float distanceMax = 80;
	public float zoomSpeed = 15.0f;
	
	private float[] distances = new float[32];
	private Vector3 moveDirection = Vector3.zero;


	// Use this for initialization
	void Start () {
	
	}
	


	/*
	public void function Zoom()
	{
		//targetView -= Input.GetAxis("Mouse ScrollWheel") * zoomStep;
		//targetView = Mathf.Clamp(targetView, minZoom, maxZoom);
		//camera.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, targetView, zoomStep, damp);
	}
*/
	// Update is called once per frame
	void Update () {

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		if(Input.GetKey(KeyCode.RightArrow))
		{
			transform.position = new Vector3(x+cameraSpeed * Time.deltaTime,y,z);
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position = new Vector3(x-cameraSpeed * Time.deltaTime,y,z);
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.position = new Vector3(x,y-cameraSpeed * Time.deltaTime,z);
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.position = new Vector3(x,y+cameraSpeed * Time.deltaTime,z);
		}


		transform.Translate(moveDirection, Space.Self);

		moveDirection = new Vector3(0,0,Input.GetAxis("Mouse ScrollWheel"));
		moveDirection *= zoomSpeed;
		
		//Checking the upper and lower bounds is a matter of determining if 
		//    the direction we're moving is positive (zooming in) or negative (zooming out). 
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			//We shouldn't be allowed to zoom in more than distanceMin
			if (Mathf.Floor(transform.position.y + moveDirection.y) > distanceMin)
			{
				transform.Translate(moveDirection, Space.Self);
				
			}
		}else if (Input.GetAxis("Mouse ScrollWheel") < 0){
			//We shouldn't be allowed to zoom out more than distanceMax
			if (Mathf.Floor(transform.position.y + moveDirection.y) < distanceMax)
			{
				transform.Translate(moveDirection, Space.Self);    
			}
		}
	}
}


