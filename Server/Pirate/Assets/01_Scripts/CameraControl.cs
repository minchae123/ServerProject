using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
	[SerializeField] private float speed;

	private void Update()
	{
		float x = Input.GetAxis("Horizontal");
		if(x != 0)
		{
			transform.Rotate(new Vector3(30, -x * speed, 0));
			transform.rotation = Quaternion.Euler(30, transform.rotation.eulerAngles.y, 0);
		}
	}
}
