using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unicessing;

public class IslamicStars : UGraphics 
{

	List <Poly> polys;

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

	public string filename;

	public Color outside = new Color (0.5f, 0.5f, 0.5f);
	public Color inside = new Color (1, 1, 1);

//	public string[] lines;

	public class Point
	{
		public float x,y;
		public Point(float x, float y)
		{
			this.x = x;
			this.y = y;
			if (x < minx)  minx = x;
			if (x > maxx)  maxx = x;
			if (y < miny)  miny = y;
			if (y > maxy)  maxy = y;
		} 
	}

	public class MyFloat {
		float v;
		public MyFloat(float v) {
			this.v = v;
		}
		public float floatValue() {
			return v;
		}
	}

	public class Poly: UGraphics
	{
		public List<Point>  pts;
		public int nbrSides;


		public Poly(int nbrSides)
		{
			this.nbrSides = nbrSides;
			this.pts = new List<Point>();
		}

		public void AddDot(float x, float y)
		{
			pts.Add( new Point(x,y) );
		}

		public bool outsideBorder()
		{
			float cx = 0;
			float cy = 0;
			for (int i = 0; i < nbrSides; ++i) 
			{
				Point pt = (Point) pts[i % nbrSides];
				cx += tx(pt.x);
				cy += ty(pt.y);
			}
			cx /= nbrSides;
			cy /= nbrSides;
			float dx = cx - width/2;
			float dy = cy - height/2;
			Debug.Log (" dot position: " + sqrt (dx * dx + dy * dy) + " should be less than " + ratio*Screen.width/2);
			return sqrt(dx*dx + dy*dy) > ratio*Screen.width/2;
		}
	}


	protected override void Setup()
	{
		size (120, 120);
		colorMode(RGB, 1);
//		smooth();
		stroke(2);
		blendMode(UMaterials.BlendMode.Add);
		// noLoop();
		polys = new List<Poly>();
		ReadFile(filename + ".txt");
	}

	protected override void Draw()
	{
		Debug.Log ("in draw function");
		angStar = .001f + (float)-mouseX/width*PI;
		edgeDist = (float) mouseY/height;
		// println("angstar = " + angStar  + " lm = " + lm);
		//    println("ratio = " + ratio);
		//  }
		background(0.5f);
		for (int i = 0; i < polys.Count; ++i) {
			Poly poly = polys[i];
			doDraw(poly);
		}


	}

	public void doDraw(Poly mPoly)
	{
		Debug.Log ("in do draw function");
		if (mPoly.outsideBorder ()) 
		{
			fill (0.5f);
			Debug.LogError ("outsideborder");
			return;
		} else 
		{
			fill (1);
			Debug.LogError ("insideborder");
		}

		//Debug.LogError ("setup");
		float r = sin(mPoly.nbrSides*PI*2/8.0f);
		float g = sin(mPoly.nbrSides*PI*2/8.0f+2);
		float b = sin(mPoly.nbrSides*PI*2/8.0f+4);
		fill  (.9f+r*.1f, .9f+g*.1f, .9f+b*.1f);
		stroke(.8f+r*.1f, .8f+g*.1f, .8f+b*.1f);

		string outStr = mPoly.nbrSides +",";
		beginShape();

		Debug.Log ("begin shape");
		for (int i = 0; i <= mPoly.nbrSides; ++i) {
			Point pt = (Point) mPoly.pts[i % mPoly.nbrSides];
			vertex( tx(pt.x), ty(pt.y) );
			if (i < mPoly.nbrSides)
				outStr += pt.x + "," + pt.y + ",";
		}
		endShape();
//		Debug.Log(outStr);

		stroke(0);

//		float cx = 0;
//		float cy = 0;
//		for (int i = 0; i < mPoly.nbrSides; ++i) {
//			Point pt = (Point) mPoly.pts[i % mPoly.nbrSides];
//			cx += tx(pt.x);
//			cy += ty(pt.y);
//		}
//		cx /= mPoly.nbrSides;
//		cy /= mPoly.nbrSides;
//		for (int i = 0; i < mPoly.nbrSides; ++i) {
//			Point p1 = (Point) mPoly.pts[i % mPoly.nbrSides];
//			Point p2 = (Point) mPoly.pts[(i+1) % mPoly.nbrSides];
//			Point p3 = (Point) mPoly.pts[(i+2) % mPoly.nbrSides];
//			// Draw segment that starts at midpoint of p2,p1 and goes at angle p2,p1 + (PI-angStar)/2
//			// to the point where it intersects segment that starts at midpoint of p2,p3 and goes at angle p2,p3-(PI-angStar)/2
//			float mx1 = (p1.x + p2.x)/2;
//			float my1 = (p1.y + p2.y)/2;
//			mx1 += (p1.x - p2.x)*edgeDist;
//			my1 += (p1.y - p2.y)*edgeDist;
//			float mx2 = (p3.x + p2.x)/2;
//			float my2 = (p3.y + p2.y)/2;
//			mx2 += (p3.x - p2.x)*edgeDist;
//			my2 += (p3.y - p2.y)*edgeDist;
//			float ang1 = atan2(p2.y-p1.y,p2.x-p1.x) + (PI-angStar)/2;
//			float ang2 = atan2(p2.y-p3.y,p2.x-p3.x) - (PI-angStar)/2;
//			float ex1 = mx1+cos(ang1)*starEdge;
//			float ey1 = my1+sin(ang1)*starEdge;
//			float ex2 = mx2+cos(ang2)*starEdge;
//			float ey2 = my2+sin(ang2)*starEdge;
//			Point ip = intersection(mx1,my1,ex1,ey1,mx2,my2,ex2,ey2);
//			if (ip == null)
//				continue;
//			line(tx(mx1),ty(my1), tx(ip.x), ty(ip.y));
//			line(tx(mx2),ty(my2), tx(ip.x), ty(ip.y));
			// Find point where these lines intersect, and draw line from mx1,my1 ix,iy   and mx2,my2,ix,iy
//		}
	}

	void ReadFile(string vFileName)
	{
		List<MyFloat> vipts = new List<MyFloat>();

		string path = ("Assets/Resources/" + vFileName);
		FileStream fs = new FileStream(path, FileMode.Open);

		string content = "";
		using (StreamReader reader = new StreamReader (fs, true))
		{
			content = reader.ReadToEnd ();		
		}
		string[] lines= content.Split('\n');
		for (int i = 0; i < lines.Length; ++i) {
			if (lines[i].Length < 3)
				continue;
			string[] nums = lines[i].Split(',');
			for (int j = 0; j < nums.Length; ++j) {
				//Debug.Log ("iteration number: " + j + " and num: " + nums[j] + " length of nums: " + nums[j].Length);
				if(nums[j].Length == 0)
				{
					break;
				}
				if (nums[j].Substring(0,1).Equals(" "))
					nums[j] = nums[j].Substring(1);
				if (nums[j].Length > 0)
					vipts.Add(new MyFloat(float.Parse(nums[j])));
			}
		}
		polys = new List<Poly>();
		float[] ipts;
		ipts = new float[vipts.Count];
		println("loaded " + vipts.Count + " points");
		for (int i = 0; i < vipts.Count; ++i) {
			ipts[i] = vipts[i].floatValue();
		}

		for (int i = 0; i < vipts.Count; )
		{
			int nbrSides = (int) ipts[i++];
			Poly poly = new Poly(nbrSides);
			for (int j = 0; j < nbrSides; ++j) {
				poly.AddDot(ipts[i+j*2],ipts[i+j*2+1]);
			}
			i += nbrSides*2;
			polys.Add(poly);
		}
		println("loaded " + polys.Count + " polys");
		dxs = (width-lm*2)/(float)(maxx-minx);
		dys = (height-tm*2)/(float)(maxy-miny);   
	}



	// this crashes in certain situations...
	Point intersection(float x1,float y1,float x2,float y2,
		float x3, float y3, float x4,float y4 )
	{
		float d = (x1-x2)*(y3-y4) - (y1-y2)*(x3-x4);
		try {
			//      float xi = ((x3-x4)*(x1*y2-y1*x2)-(x1-x2)*(x3*y4-y3*x4))/d;
			//      float yi = ((y3-y4)*(x1*y2-y1*x2)-(y1-y2)*(x3*y4-y3*x4))/d;
			float denom = (y4-y3)*(x2-x1) - (x4-x3)*(y2-y1);
			float numea = (x4-x3)*(y1-y3) - (y4-y3)*(x1-x3);
			float numeb = (x2-x1)*(y1-y3) - (y2-y1)*(x1-x3);
			if (abs(denom) < 0.01) {
				if (numea == 0.0 && numeb == 0.0)
				{
					// coincident
					println("c");
					return new Point ( x1, y1 );
				}
				else {
					// parallel
					println("p");
					return null;
				}
			}
			float ua = numea / denom;
			float ub = numeb / denom;
			if (ua >= 0.0f && ua <= 1.0f && ub >= 0.0f && ub <= 1.0f)
			{
				float xi = x1 + ua*(x2-x1);
				float yi = y1 + ua*(y2-y1);
				if (xi > 0 && xi < width && yi > 0 && yi < width)
					return new Point(xi,yi);
				else
					return null;
			}
			else {
				// not intersecting - this works well...
				return new Point (x1, y1 );
			}
		}
		catch (System.Exception e)
		{
			println("e");
			return null;
		} 
	}
		

	static float tx(float x)
	{
		return (x - minx)*dxs + lm;
	}

	static float ty(float y)
	{
		return (y - miny)*dys + tm;
	}

	void myLine(float x1, float y1, float x2, float y2)
	{
		line(tx(x1),ty(y1),tx(x2),ty(y2));
	}

}
