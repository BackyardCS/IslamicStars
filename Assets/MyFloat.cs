using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFloat: MonoBehaviour 
{
	float v;
	public MyFloat(float v)
	{
		this.v = v;
	}
	public float floatValue() 
	{
		return v;
	}
}
