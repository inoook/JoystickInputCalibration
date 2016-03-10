using UnityEngine;
using System.Collections;

public class JoystickSetting : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		LoadSetting();
	}
	
	float throttleInput = 0;
	float yawInput = 0;
	
	float throttleInputRemap = 0;
	float yawInputRemap = 0;
	
	public float guiX;
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.J)){
			debug = !debug;
		}
		
		if(debug){
			throttleInput = Input.GetAxisRaw ("Throttle");
			yawInput = Input.GetAxisRaw ("Yaw");

			throttleInputRemap = InputRemap.GetAxisRaw ("Throttle");
			yawInputRemap = InputRemap.GetAxisRaw ("Yaw");
		}
	}
	
	public InputRemapCalibration calibrationBehav;
	public bool debug = true;
	
	public int depth = 100;
	public Color color = Color.white;
	
	void OnGUI()
	{
		if(!debug){ return; }
		
		GUI.depth = depth;
		GUI.color = color;
		
		float w = 500;
		float x = Screen.width / 2 - w/2;
		guiX = x;

		GUILayout.BeginArea (new Rect (x, 50, w, 800));
		GUILayout.BeginVertical ("box");

		GUILayout.Label ("-Raw");
		GUILayout_HorizontalSlider (throttleInput, "Throttle", -1.0f, 1.0f);
		GUILayout_HorizontalSlider (yawInput, "Yaw", -1.0f, 1.0f);

		GUILayout.Label ("-Remap");
		GUILayout_HorizontalSlider (throttleInputRemap, "Throttle", -1.0f, 1.0f);
		GUILayout_HorizontalSlider (yawInputRemap, "Yaw", -1.0f, 1.0f);
		
		//if (calibrationBehav.debug) {
		GUILayout.Space (10);
		GUILayout.Label("####### Select Axis ########");
		if (GUILayout.Button ("Throttle")) {
			calibrationBehav.axis_name = "Throttle";
		}
		if (GUILayout.Button ("Yaw")) {
			calibrationBehav.axis_name = "Yaw";
		}
			
		GUILayout.Space(10);
		calibrationBehav.DrawGUI();
		//}
		GUILayout.Space (10);
		if (GUILayout.Button ("Sava")) {
				SaveSetting ();
		}
		if (GUILayout.Button ("Clear")) {
				ClearSetting ();
		}

		GUILayout.EndVertical ();
		GUILayout.EndArea();
	}
	
	void GUILayout_HorizontalSlider(float v, string label, float min, float max)
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label (label + ": "+v.ToString("0.0000"), GUILayout.Width(150));
		GUILayout.HorizontalSlider (v, min, max);
		GUILayout.EndHorizontal ();
	}
	
	//
	void LoadSetting()
	{
		InputRemap.LoadMap("Throttle");
		InputRemap.LoadMap("Yaw");
	}
	void SaveSetting()
	{
		InputRemap.SaveMap("Throttle");
		InputRemap.SaveMap("Yaw");
	}
	void ClearSetting()
	{
		InputRemap.DeleteMap("Throttle");
		InputRemap.DeleteMap("Yaw");
	}
}
