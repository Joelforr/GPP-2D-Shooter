using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextJuice : MonoBehaviour {
    TextMesh text;
	// Use this for initialization
	void Start () {
        text = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        text.color = new Color(Random.value, Random.value,Random.value);
	}
}
