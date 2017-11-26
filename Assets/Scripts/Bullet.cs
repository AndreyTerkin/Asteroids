using UnityEngine;

using AsteroidsLibrary.SpaceObjects;

public class Bullet : WeaponObject
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Space object")
        {
            ISpaceObject spaceObj = collider.gameObject.GetComponent<ISpaceObject>();
            if (spaceObj != null)
                Explode(spaceObj.SpaceObject);
        }
    }
}
