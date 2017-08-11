using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Compass : MonoBehaviour
{
	[SerializeField] private Image pointForCompass;

	private Camera mainCamera;

	private Image panel;

	private float width = 0;

	private Vector3 center = Vector3.zero;
	private Vector3 left = Vector3.zero;
	private Vector3 right = Vector3.zero;

	[SerializeField] private RectTransform north;
	[SerializeField] private RectTransform south;
	[SerializeField] private RectTransform west;
	[SerializeField] private RectTransform east;

	private Dictionary<GameObject, RectTransform> points = new Dictionary<GameObject, RectTransform>();

	public bool Initialized { get { return init; } }
	private bool init = false;

	public event System.Action compassInitialized;

	public void InitCompass() // transform.position init only after two fixedframe in proceduralgeneration scene
	{
		if (transform.position != Vector3.zero)
		{
			mainCamera = Camera.main;
			panel = GetComponent<Image>();

			width = panel.rectTransform.rect.width;

			center = panel.transform.position;
			left = center - new Vector3(width / 2, 0, 0);
			right = center + new Vector3(width / 2, 0, 0);

			init = true;
			if (compassInitialized != null)
				compassInitialized();
		}
	}

	void FixedUpdate ()
	{
		if (!init)
		{
			InitCompass();
			return;
		}

		UpdatePointPosition(north, Vector3.forward);
		UpdatePointPosition(south, -Vector3.forward);
		UpdatePointPosition(west, Vector3.left);
		UpdatePointPosition(east, Vector3.right);

		foreach (GameObject point in points.Keys)
		{
			Vector3 dir = (point.transform.position - mainCamera.transform.position).normalized;
			dir.y = 0;
			UpdatePointPosition(points[point], dir);
		}
	}

	public void UpdatePointPosition(RectTransform point, Vector3 dir)
	{
		Vector3 camDir = mainCamera.transform.forward;
		camDir.y = 0;
		float angle = Vector3.Angle(dir, camDir);
		angle = Vector3.Cross(dir, camDir).y < 0 ? -angle : angle;

		float x = (width / 100) * (angle / 180f) * 100;
		point.transform.position = new Vector3(center.x - x, center.y, center.z);

		point.gameObject.SetActive(!(point.transform.position.x > right.x || point.transform.position.x < left.x));
	}

	public void AddPointOnCompass(GameObject entity, Color colorOfPoint)
	{
		if (!points.ContainsKey(entity))
		{
			Image newPoint = Instantiate(pointForCompass, center, Quaternion.identity) as Image;
			newPoint.transform.SetParent(transform);
			newPoint.color = colorOfPoint;

			points.Add(entity, newPoint.rectTransform);
		}
	}

	public void RemovePoint(GameObject entity)
	{
		if (points.ContainsKey(entity))
		{
			Destroy(points[entity].gameObject);
			points.Remove(entity);
		}
	}
}
