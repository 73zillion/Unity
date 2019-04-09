using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlicker_01 : MonoBehaviour {
    public Renderer lamp;
    private Color originColor;

	// Use this for initialization
	void Start () {
        //램프 오브젝트의 색상정보를 저장한다.
        originColor = lamp.material.color;
		
	}
	
	// Update is called once per frame
	void Update () {
        //사인파에 의한 시간변화를 곱하여 색 점멸.
        float flicker = Mathf.Abs(Mathf.Sin(Time.time * 10));
        lamp.material.color = originColor * flicker;
	}
}
