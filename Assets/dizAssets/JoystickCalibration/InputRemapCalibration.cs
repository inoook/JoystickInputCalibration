using UnityEngine;
using System.Collections;

public class InputRemapCalibration : MonoBehaviour
{
	
	public float inputMin = 0.0f;
	public float inputMax = 1.0f;

	public float outputMin = 0.0f;
	public float outputMax = 1.0f;

	public float rawInput = 0.0f;

	public string axis_name = "Horizontal";
	
	bool isCalibrate = false;

	void Update ()
	{
		rawInput = Input.GetAxisRaw (axis_name);

		if (isCalibrate) {
			if (rawInput < inputMin) {
				inputMin = rawInput;
			}
			if (rawInput > inputMax) {
				inputMax = rawInput;
			}

			InputRemap.InputMap map = InputRemap.GetMap (axis_name);
			InputRemap.SetMap (map, inputMin, inputMax);
			//InputRemap.SetMap (name, inputMin, inputMax, map.outMin, map.outMax, map.invert);
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
		GUILayout.Label ("name: " + axis_name);
		if (GUILayout.Button (isCalibrate ? "EndCalibrate" : "BeginCalibrate")) {
			if (isCalibrate) {
				EndCalibrate ();
			} else {
				BeginCalibrate ();
			}
		}
		InputRemap.InputMap map = InputRemap.GetMap (axis_name);

		GUIStyle style = new GUIStyle ();
		style.fontSize = 20;
		style.normal.textColor = Color.white;
		GUILayout.Label ("input:\t" + rawInput.ToString("0.0000"), style);
		GUILayout.Label ("output:\t" + InputRemap.GetAxisRaw (axis_name).ToString("0.0000"), style);
		map.invert = GUILayout.Toggle (map.invert, "invert");

		//---------------------
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("center:", GUILayout.Width(100));
		float center = map.center;
		center = float.Parse( GUILayout.TextField (center.ToString ("0.00")) );
		map.center = center;
		if (GUILayout.Button ("GetCenter")) {
			map.center = rawInput;
		}
		GUILayout.EndHorizontal ();

		//---------------------
		GUILayout.BeginHorizontal ();
		// min / max
		float inputMin = map.inMin;
		float inputMax = map.inMax;

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("inputMin:", GUILayout.Width(100));
		inputMin = float.Parse( GUILayout.TextField (inputMin.ToString ("0.00")) );
		map.inMin = inputMin;
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("inputMax:", GUILayout.Width(100));
		inputMax = float.Parse( GUILayout.TextField (inputMax.ToString ("0.00")) );
		map.inMax = inputMax;
		GUILayout.EndHorizontal ();

		GUILayout.EndHorizontal ();

		//---------------------
		GUILayout.BeginHorizontal ();
		// dead
		float dead = map.dead;

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("dead:", GUILayout.Width(100));
		dead = float.Parse( GUILayout.TextField (dead.ToString ("0.00")) );
		map.dead = dead;
		GUILayout.EndHorizontal ();

		// damping
		float damping = map.damping;

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("damping:", GUILayout.Width(100));
		damping = float.Parse( GUILayout.TextField (damping.ToString ("0.00")) );
		map.damping = damping;
		GUILayout.EndHorizontal ();

		GUILayout.EndHorizontal ();
	}
}
