using UnityEngine;
using System.Collections;

public class SpaceObject : MonoBehaviour
{
	protected virtual void Explode()
	{
		// TODO: show explode
		Destroy(gameObject);
	}
}
