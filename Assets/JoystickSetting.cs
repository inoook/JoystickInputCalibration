using UnityEngine;
using System.Collections;

public class JoystickSetting : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		LoadSetting();
	}
	
	float horizontalRaw = 0;
	float verticalRaw = 0;
	float trottleRaw = 0;
	
	float horizontalRemap = 0;
	float verticalRemap = 0;
	float trottleRemap = 0;
	
	public float guiX;
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.J)){
			debug = !debug;
		}
		
		if(debug){
			horizontalRaw = Input.GetAxisRaw ("Horizontal");
			verticalRaw = Input.GetAxisRaw ("Vertical");
			trottleRaw = Input.GetAxisRaw ("Trottle");

			horizontalRemap = InputRemap.GetAxisRaw ("Horizontal");
			verticalRemap = InputRemap.GetAxisRaw ("Vertical");
			trottleRemap = InputRemap.GetAxisRaw ("Trottle");
		}
	}
	
	public InputRemapCalibration calibrationBehav;
	public bool debug = true;
	
	public int depth = 100;
	
	void OnGUI()
	{
		if(!debug){ return; }
		
		GUI.depth = depth;
		
		float w = 500;
		float x = Screen.width / 2 - w/2;
		guiX = x;
		GUILayout.BeginArea (new Rect (x, 50, w, 800));
		
		GUILayout.Label ("-Raw");
		GUILayout_HorizontalSlider (horizontalRaw, "Horizontal", -1.0f, 1.0f);
		GUILayout_HorizontalSlider (verticalRaw, "Vertical", -1.0f, 1.0f);
		GUILayout_HorizontalSlider (trottleRaw, "Trottle", -1.0f, 1.0f);

		GUILayout.Label ("-Remap");
		GUILayout_HorizontalSlider (horizontalRemap, "Horizontal", -1.0f, 1.0f);
		GUILayout_HorizontalSlider (verticalRemap, "Vertical", -1.0f, 1.0f);
		GUILayout_HorizontalSlider (trottleRemap, "Trottle", -1.0f, 1.0f);
		
		//if (calibrationBehav.debug) {
				GUILayout.Space (20);
				GUILayout.Label("####### Select Axis ########");
				if (GUILayout.Button ("Horizontal")) {
						calibrationBehav.name = "Horizontal";
				}
				if (GUILayout.Button ("Vertical")) {
						calibrationBehav.name = "Vertical";
				}
				if (GUILayout.Button ("Trottle")) {
						calibrationBehav.name = "Trottle";
				}
			
				GUILayout.Space(20);
				calibrationBehav.DrawGUI();
		//}
		GUILayout.Space (20);
		if (GUILayout.Button ("Sava")) {
				SaveSetting ();
		}
		if (GUILayout.Button ("Clear")) {
				ClearSetting ();
		}
		
		GUILayout.EndArea();
	}
	
	void GUILayout_HorizontalSlider(float v, string label, float min, float max)
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label (label + ": "+v.ToString("0.00"), GUILayout.Width(150));
		GUILayout.HorizontalSlider (v, min, max);
		GUILayout.EndHorizontal ();
	}
	
	//
	void LoadSetting()
	{
		InputRemap.LoadMap("Horizontal");
		InputRemap.LoadMap("Vertical");
		InputRemap.LoadMap("Trottle");
	}
	void SaveSetting()
	{
		InputRemap.SaveMap("Horizontal");
		InputRemap.SaveMap("Vertical");
		InputRemap.SaveMap("Trottle");
	}
	void ClearSetting()
	{
		InputRemap.DeleteMap("Horizontal");
		InputRemap.DeleteMap("Vertical");
		InputRemap.DeleteMap("Trottle");
	}
}
