using UnityEngine;
using System.Collections;
using Assets.Source;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SelectLevelButton : MonoBehaviour
{

    private Button button;

	// Use this for initialization
	void Start ()
	{
	    var main = FindObjectOfType<MainMenuController>();
	    button = GetComponent<Button>();
	    button.GetComponentInChildren<Text>().text = name;
	    if (Application.isPlaying)
	        button.onClick.AddListener(() => main.OpenLevel(name));

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetCompleted(bool b)
    {
        if (b)
        GetComponentInChildren<Text>().text = "done";
    }
}
