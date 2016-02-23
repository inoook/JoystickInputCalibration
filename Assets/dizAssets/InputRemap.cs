using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputRemap {

	public class InputMap{

		public float inMin = -1;
		public float inMax = 1;
		public float outMin = -1;
		public float outMax = 1;
//
//				public InputMap(float inMin_, float inMax_, float outMin_, float outMax_)
//				{
//						inMin = inMin_;
//						inMax = inMax_;
//						outMin = outMin_;
//						outMax = outMax_;	
//				}
	}

	public static Dictionary<string, InputMap> dic = new Dictionary<string, InputMap>();

	public static void SetMap(string name, float inMin, float inMax, float outMin = -1.0f, float outMax = 1.0f)
	{
//				dic [name] = new InputMap (inMin, inMax, outMin, outMax);
		InputMap map = new InputMap ();
		map.inMin = inMin;
		map.inMax = inMax;
		map.outMin = outMin;
		map.outMax = outMax;
		dic [name] = map;
	}
	public static InputMap GetMap(string name)
	{
		if (dic == null) {
			return new InputMap();
		}
		if (dic.ContainsKey (name)) {
			return dic [name];
		}
		return new InputMap();
	}

	public static float GetAxisRaw(string name)
	{
		if (dic == null) {
				return 0;
		}

		if (dic.ContainsKey (name)) {
			InputMap map = dic [name];

			float rawInput = Input.GetAxis(name);
			float inMin = map.inMin;
			float inMax = map.inMax;
			float outMin = map.outMin;
			float outMax = map.outMax;
			float v = Remap(rawInput, inMin, inMax, outMin, outMax);

			if(float.IsNaN(v)){
				v = 0;
			}
					
//						if(useClamp){
			v = Mathf.Clamp(v, outMin, outMax);
//						}

			return v;
		}
		return 0;
	}

	public static float GetAxis(string name)
	{
		return GetAxisRaw (name);
	}

	// remap
	public static float Remap (float value, float inputMin, float inputMax, float outputMin, float outputMax) {
		return (value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
	}

	// load / save
	public static void LoadMap(string name)
	{
		float inMin = -1;
		float inMax = 1;

		float outMin = -1;
		float outMax = 1;

		if (PlayerPrefs.HasKey (name + "_ramap_inMin")) {
			inMin = PlayerPrefs.GetFloat (name + "_ramap_inMin");
			inMax = PlayerPrefs.GetFloat (name + "_ramap_inMax");

			outMin = PlayerPrefs.GetFloat (name + "_ramap_outMin");
			outMax = PlayerPrefs.GetFloat (name + "_ramap_outMax");
		}
		SetMap (name, inMin, inMax, outMin, outMax);
	}
	public static void SaveMap(string name)
	{
		InputMap map = GetMap (name);

		PlayerPrefs.SetFloat (name + "_ramap_inMin", map.inMin);
		PlayerPrefs.SetFloat (name + "_ramap_inMax", map.inMax);

		PlayerPrefs.SetFloat (name + "_ramap_outMin", map.outMin);
		PlayerPrefs.SetFloat (name + "_ramap_outMax", map.outMax);
	}

	public static void DeleteMap(string name)
	{
		PlayerPrefs.DeleteKey (name + "_ramap_inMin");
		PlayerPrefs.DeleteKey (name + "_ramap_inMax");

		PlayerPrefs.DeleteKey (name + "_ramap_outMin");
		PlayerPrefs.DeleteKey (name + "_ramap_outMax");
	}
}
