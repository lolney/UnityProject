using UnityEngine;
using System.Collections;

public class ZoomPanCameraBehavior : MonoBehaviour {

	Vector3 lastPos = Vector3.zero;
	public float sensitivity = -2f;
	public float maxCameraSize = 75;
	
	private float limitX;
	private float limitY;
	
	void Start(){
		Vector3 size = new Vector3(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
		limitX = size.x;
		limitY = size.y;
	}
	
	// Update is called once per frame
	void Update () {
		float scroll = Input.GetAxis("Mouse ScrollWheel");

		// Zooming
		if(scroll != 0){
			float newSize;
			
			if(scroll < 0)
				newSize = Mathf.Max (Camera.main.orthographicSize + sensitivity * scroll, 6);
			else
				newSize = Mathf.Min (Camera.main.orthographicSize + sensitivity * scroll, maxCameraSize);
			
			Camera.main.orthographicSize = newSize;
			Camera.main.transform.position = clampToCameraLimits(Camera.main.transform.position);
		}
		
		// Panning
		if(Input.GetMouseButton(0)){
			Vector3 currentPos = Input.mousePosition;
			if(lastPos != Vector3.zero) {
				float scale = Camera.main.orthographicSize / maxCameraSize;
				Vector3 newPos = Camera.main.transform.position + (scale * (currentPos - lastPos));
				
				Camera.main.transform.position = clampToCameraLimits(newPos);
			}
			lastPos = currentPos;
		} else {
			lastPos = Vector3.zero;
		}
	}
	
	bool exceedCameraLimits(Vector3 newPos) {
		Vector3 size = new Vector3(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
		
		Vector3 upper = newPos + size;
		Vector3 lower = newPos - size;
					
		if (upper.x > limitX || lower.x < -limitX ||
			upper.y > limitY || lower.y < -limitY) 
				return true;
		else return false;
	}
	
	Vector3 clampToCameraLimits(Vector3 newPos){
		Vector3 size = new Vector3(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
		
		newPos.x = Mathf.Min(newPos.x, limitX - size.x);
		newPos.x = Mathf.Max(newPos.x, -limitX + size.x);
		newPos.y = Mathf.Min(newPos.y, limitY - size.y);
		newPos.y = Mathf.Max(newPos.y, -limitY + size.y);
		
		return newPos;
	}
}
