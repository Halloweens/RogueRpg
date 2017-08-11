using UnityEngine;
using System.Collections;

public class RadialMenuHide : MonoBehaviour {

    public GameObject menuFont;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnRadialButtonPressed()
    {
        menuFont.SetActive(false);
    }
}
