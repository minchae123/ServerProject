using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CameraControl : MonoBehaviour
{
	private Camera cam;

	[SerializeField] private float rotateSpeed = 500f;
	[SerializeField] private float zoomScale = 10f;
	[SerializeField] private float minZoom = 0.1f;
	[SerializeField] private float maxZoom = 10f;

	private Vector3 mouseWorldPosStart;

	private void Awake()
	{
		cam = Camera.main;
	}

	private void Update()
	{
		if (Input.GetMouseButton(1))
		{
			CameraOrbit();
		}

		CameraZoom(Input.GetAxis("Mouse ScrollWheel"));
	}

	public void CameraOrbit()
	{
		if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
		{
			float vertical = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
			float horizontal = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
			transform.Rotate(Vector3.left, vertical);
			transform.Rotate(Vector3.down, horizontal, Space.World);
		}
	}

	public void CameraZoom(float scale)
	{
		if (scale != 0)
		{
			mouseWorldPosStart = cam.ScreenToWorldPoint(Input.mousePosition);
			cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scale * zoomScale, minZoom, maxZoom);
			Vector3 mouseDiff = mouseWorldPosStart - cam.ScreenToWorldPoint(Input.mousePosition);
			transform.position += mouseDiff;
		}

	}
}
