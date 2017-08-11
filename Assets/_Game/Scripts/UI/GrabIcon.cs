using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GrabIcon : MonoBehaviour
{
    private Ray actionRay;
    public GameObject grabIconUI;
    bool isNearUsableObject = false;
    GameObject usableObject = null;

	public GameObject Player { get { return playerChara; } set { playerChara = value; } }
    private GameObject playerChara;

    private InputSystem inputSystem = null;
    GameCamera cameraPerso;
    Transform center;

    void Start ()
    {
        inputSystem = playerChara.GetComponent<InputSystem>();
        cameraPerso = Camera.main.GetComponent<GameCamera>();
        center = cameraPerso.center;
        if (inputSystem != null)
            inputSystem.onUse.AddListener(new UnityAction(() => { activateAction(); }));
    }
	
	void Update ()
    {
        ShowGrabIcon();
    }

    private void ShowGrabIcon()
    {
        if (center == null || cameraPerso == null)
        {
            cameraPerso = Camera.main.GetComponent<GameCamera>();
            center = cameraPerso.center;
        }

        Vector3 dir = (cameraPerso.transform.position + cameraPerso.transform.forward) - center.position;
        if (cameraPerso.zoom > 0.21f)
            dir = -dir;

        actionRay = new Ray(center.position, dir);
        RaycastHit hit;
        LayerMask mask = 1 << LayerMask.NameToLayer("Player");
        mask = ~mask;
        if (Physics.Raycast(actionRay, out hit, Mathf.Infinity,mask, QueryTriggerInteraction.Ignore))
        {
            Usable usable = hit.transform.GetComponent<Usable>();

            if (hit.distance < 2f && usable != null && usable.canUse)
            {
                isNearUsableObject = true;
                grabIconUI.SetActive(true);
                usableObject = hit.transform.gameObject;
            }
            else
            {
                isNearUsableObject = false;
                grabIconUI.SetActive(false);
                usableObject = null;
            }
        }
    }

    public void activateAction()
    {
        if (isNearUsableObject && usableObject != null)
            usableObject.transform.GetComponent<Usable>().Use();
    }
}
