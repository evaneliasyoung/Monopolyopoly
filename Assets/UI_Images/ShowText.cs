using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Reflection;
using TMPro;

/**
License: do whatever you want.

Questions etc: @t_machine_org
*/
public class ShowText : MonoBehaviour
{
	public GameObject target;
	public string variableName;
	public TMPro.TextMeshProUGUI myText;
	public int textSize = 36;
	public string preText = "";
	public string textAllignment = "Center";

	/** Don't refresh at 60FPS; wasteful! */
	private float updateNSeconds = 0.25f;
	private float lastUpdateTime = 0f;

	// Update is called once per frame
	void Update ()
	{
		lastUpdateTime += Time.deltaTime;
		if (lastUpdateTime > updateNSeconds) {
			lastUpdateTime = 0;
			if (myText == null) {
				Debug.LogError ("Missing Text object, please disable this DisplayVariable component");
			} else if (variableName == null || target == null) {
				myText.text = "[Unattached]";
			} else {
				bool foundIt = false;
				string valueAsString = "";
				foreach (Component subComponent in target.GetComponents ( typeof(Component))) {
					FieldInfo[] fields = subComponent.GetType ().GetFields ();
					foreach (FieldInfo fieldInfo in fields) {
						if (fieldInfo.Name.Equals (variableName)) {
							valueAsString = fieldInfo.GetValue (subComponent).ToString ();
							foundIt = true;
							break;
						}
						if (foundIt) {
							break;
						}
					}
					if (! foundIt) {
						PropertyInfo[] properties = subComponent.GetType ().GetProperties ();
						foreach (PropertyInfo propertyInfo in properties) {
							if (propertyInfo.Name.Equals (variableName)) {
								valueAsString = propertyInfo .GetValue (subComponent, null).ToString ();
								foundIt = true;
								break;
							}
							if (foundIt){
								break;
							}	
						}
					}
				}

				if(textAllignment == "Center"){
					myText.alignment = TextAlignmentOptions.Center; 
				}
				if(textAllignment == "Right"){
					myText.alignment = TextAlignmentOptions.Right; 
				}
				if(textAllignment == "Left"){
					myText.alignment = TextAlignmentOptions.Left; 
				}
				
				myText.fontSize = textSize;
				myText.text = preText + valueAsString;
			}
		}
	}
}