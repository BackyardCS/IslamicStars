using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicessing;
public class Stars : UGraphics 
{

	List <Poly> polyss;

	bool isInteractive = false; // turn off when you've found a good ratio for this tiling...

	static float minx=10000, maxx=-10000, miny=10000, maxy=-10000;
	static float dxs = 1;
	static float dys = 1;
	static float lm = 10;
	static float tm = 10;
	static float ratio = 1;
	static float angStar = (2* Mathf.PI)/18;  // 30 degrees
	static float edgeDist = .1f;
	static float starEdge = 1000;

	public Color outside = new Color (0.5f, 0.5f, 0.5f);
	public Color inside = new Color (1, 1, 1);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
