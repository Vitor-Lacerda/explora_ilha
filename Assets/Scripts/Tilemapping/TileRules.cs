using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class TileRules : ScriptableObject {

	[System.Serializable]
	public struct Rule
	{
		public Sprite sprite;
		public bool top;
		public bool bottom;
		public bool left;
		public bool right;																																						

	}

	public Sprite[] defaults;

	public List<Rule> rules;

	public Sprite SpriteVizinhos(bool[] v){
		Sprite s = rules.FirstOrDefault (r => r.top == v [0] && r.bottom == v [1] && r.left == v [2] && r.right == v [3]).sprite;
		if (s == null) {
			return RandomDefault ();
		}
		return s;
	}

	public Sprite RandomDefault(){
		return defaults[Random.Range(0, defaults.Length)];
	}

}
