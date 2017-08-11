using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{
    public float xMouseSensibility = 100.0f;
    public float yMouseSensibility = 100.0f;

    public Transform center = null;
    public Vector3 offset = new Vector3(0.0f, 0.0f, -2.0f);

    /// <summary>
    /// 1.0f : TP, 0.0f : FP
    /// </summary>
    [Range(0.0f, 1.0f)]
    public float zoom = 1.0f;
    public float zoomStep = 0.2f;
    public float zoomFPStartRange = 0.1f;
    public float zoomSmoothing = 15.0f;

    public Renderer modelRenderer = null;

    public LayerMask ignoredLayers = 0;
    public float wallCollisionOffset = 0.01f;

    private float yRotation = 0.0f;
    private float xRotation = 0.0f;
    private float smoothedZoom = 0.0f;

    private void Start()
    {
        smoothedZoom = zoom;
    }

    private void Update()
    {
        xRotation += Input.GetAxis("Mouse X") * xMouseSensibility * Time.deltaTime;
        xRotation = Mathf.Repeat(xRotation, 360.0f);
        yRotation += -Input.GetAxis("Mouse Y") * yMouseSensibility * Time.deltaTime;
        yRotation = Mathf.Clamp(yRotation, -40.0f, 80.0f);

        float scrollValue = 0.0f;
        if ((scrollValue = Input.GetAxis("Mouse ScrollWheel")) != 0.0f)
            zoom = Mathf.Clamp01(zoom + (-Mathf.Sign(scrollValue) * zoomStep));

        smoothedZoom = Mathf.SmoothStep(smoothedZoom, zoom, Time.deltaTime * zoomSmoothing);
    }

    private void FixedUpdate()
    {
		if (smoothedZoom > zoomFPStartRange)
        {
            transform.position = center.position + Vector3.Scale(offset, new Vector3(1.0f, 1.0f, smoothedZoom));

            transform.RotateAround(center.position, Vector3.up, xRotation);

            transform.LookAt(center, Vector3.up);

            transform.RotateAround(center.position, transform.right, yRotation);

            transform.LookAt(center, Vector3.up);

            Ray ray = new Ray(center.position, transform.position - center.position);
            float dist = Vector3.Distance(center.position, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, dist, ~ignoredLayers))
                transform.position = hit.point + hit.normal * wallCollisionOffset;

            modelRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        else if (smoothedZoom < zoomFPStartRange)
        {
            transform.position = center.position;
            transform.rotation = Quaternion.identity;

            transform.Rotate(Vector3.up, xRotation);

            transform.Rotate(Vector3.right, yRotation);

            transform.LookAt(transform.position + transform.forward);

            modelRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

        Debug.DrawLine(transform.position, transform.position + transform.forward * 2.0f, Color.blue);
    }
}
