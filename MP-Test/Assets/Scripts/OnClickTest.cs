using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Host_OnClick : MonoBehaviour {

	public Button btn;

	void Start()
	{
		Button myBtn = btn.GetComponent<Button>();
		myBtn.onClick.AddListener(Host);
	}

	void Host()
	{
    	Destroy(this.gameObject);
		Debug.Log("test");
  	}  
}
