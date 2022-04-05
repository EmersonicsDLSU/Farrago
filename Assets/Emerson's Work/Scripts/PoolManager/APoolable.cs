using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APoolable : MonoBehaviour
{
	//initializes the property of this object.
	public abstract void initialize();

	//throws this event when this object has been released back to the pool.
	public abstract void onRelease();

	//throws this event when this object has been activated from the pool.
	public abstract void onActivate();

	//provides a copy of the poolable instance. must be implemented per specific class
	public ObjPools poolRef;
}
