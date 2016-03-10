using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputRemap {

	public class InputMap
	{
		public float inMin = -1;
		public float inMax = 1;
		public float outMin = -1;
		public float outMax = 1;

		public float center = 0;

		public bool invert = false;
		public float dead = 0.0f;

		public float damping = 0.0f;
		public float newOutput = 0.0f;
	}

	public static Dictionary<string, InputMap> dic = new Dictionary<string, InputMap>();

	public static void SetMap(string name, float inMin, float inMax, float outMin = -1.0f, float outMax = 1.0f, bool invert = false, float dead = 0.0f, float damping = 0.0f, float center = 0.0f)
	{
		InputMap map = new InputMap ();
		map.inMin = inMin;
		map.inMax = inMax;
		map.outMin = outMin;
		map.outMax = outMax;
		map.invert = invert;
		map.dead = dead;
		map.damping = damping;
		map.center = center;

		dic [name] = map;
	}

	public static void SetMap(InputMap map, float inMin, float inMax)
	{
		map.inMin = inMin;
		map.inMax = inMax;
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

//			float rawInput = Input.GetAxis(name);
			float rawInput = Input.GetAxisRaw(name);
			float inMin = map.inMin;
			float inMax = map.inMax;
			float outMin = map.outMin;
			float outMax = map.outMax;

			//float v = Remap(rawInput, inMin, inMax, outMin, outMax);

			float dead = map.dead;
			float v = 0;
			float center = map.center;
			if (rawInput < center - dead) {
				v = Remap (rawInput, inMin, center - dead, outMin, 0);
			}else if(rawInput > center + dead){
				v = Remap (rawInput, center + dead, inMax , 0, outMax);
			}

			if(float.IsNaN(v)){
				v = 0;
			}

			v = Mathf.Clamp(v, outMin, outMax);

			float damping = map.damping;
			if (damping != 0) {
				map.newOutput = Mathf.Lerp (map.newOutput, v, Time.deltaTime * damping);
			}else{
				map.newOutput = v;
			}

			float output = map.newOutput;
			if ( Mathf.Abs (output) < dead * 0.15f) {
				output = 0;
			}
			return output * (map.invert ? -1 : 1);
//
//			float dead = map.dead;
//			if (Mathf.Abs (v) < dead) {
//				v = 0;
//			}
//
//			float damping = map.damping;
//			if (damping != 0) {
//				map.newOutput = Mathf.Lerp (map.newOutput, v, Time.deltaTime * damping);
//				if ( Mathf.Abs (map.newOutput) < dead * 0.001f) {
//					map.newOutput = 0;
//				}
//			}else{
//				map.newOutput = v;
//			}
//			return map.newOutput * (map.invert ? -1 : 1);
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

		bool invert = false;

		float dead = 0;
		float damping = 0;
		float center = 0;

		if (PlayerPrefs.HasKey (name + "_ramap_inMin")) {
			inMin = PlayerPrefs.GetFloat (name + "_ramap_inMin");
			inMax = PlayerPrefs.GetFloat (name + "_ramap_inMax");

			outMin = PlayerPrefs.GetFloat (name + "_ramap_outMin");
			outMax = PlayerPrefs.GetFloat (name + "_ramap_outMax");

			invert = (PlayerPrefs.GetInt (name + "_ramap_invert") == 1);

			dead = PlayerPrefs.GetFloat (name + "_ramap_dead");

			damping = PlayerPrefs.GetFloat (name + "_ramap_damping");

			center = PlayerPrefs.GetFloat (name + "_ramap_center");
		}
		SetMap (name, inMin, inMax, outMin, outMax, invert, dead, damping, center);
	}
	public static void SaveMap(string name)
	{
		InputMap map = GetMap (name);

		PlayerPrefs.SetFloat (name + "_ramap_inMin", map.inMin);
		PlayerPrefs.SetFloat (name + "_ramap_inMax", map.inMax);

		PlayerPrefs.SetFloat (name + "_ramap_outMin", map.outMin);
		PlayerPrefs.SetFloat (name + "_ramap_outMax", map.outMax);

		PlayerPrefs.SetInt (name + "_ramap_invert", (map.invert ? 1 : 0));

		PlayerPrefs.SetFloat (name + "_ramap_dead", map.dead);

		PlayerPrefs.SetFloat (name + "_ramap_damping", map.damping);

		PlayerPrefs.SetFloat (name + "_ramap_center", map.center);
	}

	public static void DeleteMap(string name)
	{
		PlayerPrefs.DeleteKey (name + "_ramap_inMin");
		PlayerPrefs.DeleteKey (name + "_ramap_inMax");

		PlayerPrefs.DeleteKey (name + "_ramap_outMin");
		PlayerPrefs.DeleteKey (name + "_ramap_outMax");

		PlayerPrefs.DeleteKey (name + "_ramap_invert");

		PlayerPrefs.DeleteKey (name + "_ramap_dead");

		PlayerPrefs.DeleteKey (name + "_ramap_damping");

		PlayerPrefs.DeleteKey (name + "_ramap_center");

	}
}
