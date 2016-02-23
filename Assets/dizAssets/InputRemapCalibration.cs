using UnityEngine;
using System.Collections;

public class InputRemapCalibration : MonoBehaviour
{
	
	public float inputMin = 0.0f;
	public float inputMax = 1.0f;

	public float outputMin = 0.0f;
	public float outputMax = 1.0f;

	public float rawInput = 0.0f;

	public string name = "Horizontal";
	
	bool isCalibrate = false;

	void Update ()
	{
		rawInput = Input.GetAxisRaw (name);

		if (isCalibrate) {
			if (rawInput < inputMin) {
				inputMin = rawInput;
			}
			if (rawInput > inputMax) {
				inputMax = rawInput;
			}
			InputRemap.SetMap (name, inputMin, inputMax);
		}
	}

	void BeginCalibrate ()
	{
		inputMin = Mathf.Infinity;
		inputMax = Mathf.NegativeInfinity;
		isCalibrate = true;
	}

	void EndCalibrate ()
	{
		isCalibrate = false;
	}

		
	public void DrawGUI ()
	{
		GUILayout.Label ("####### calibration ########");
		GUILayout.Label ("name: " + name);
		if (GUILayout.Button (isCalibrate ? "EndCalibrate" : "BeginCalibrate")) {
			if (isCalibrate) {
				EndCalibrate ();
			} else {
				BeginCalibrate ();
			}
		}
		InputRemap.InputMap map = InputRemap.GetMap (name);

		GUILayout.Label ("input:" + rawInput);
		GUILayout.Label ("output: " + InputRemap.GetAxisRaw (name));

		float inputMin = map.inMin;
		float inputMax = map.inMax;

//		GUILayout.Label ("inputMin: " + inputMin);
//		GUILayout.Label ("inputMax: " + inputMax);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("inputMin:", GUILayout.Width(100));
		inputMin = float.Parse( GUILayout.TextField (inputMin.ToString ("0.00")) );
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("inputMax:", GUILayout.Width(100));
		inputMax = float.Parse( GUILayout.TextField (inputMax.ToString ("0.00")) );
		GUILayout.EndHorizontal ();


	}
}
