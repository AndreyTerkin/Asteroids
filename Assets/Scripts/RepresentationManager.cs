using System.Collections.Generic;
using UnityEngine;

public class RepresentationManager : MonoBehaviour
{
    public Transform backgound;
    public List<GameObject> multiRepresentableObjects;

    public enum Representation : int
    {
        Sprite,
        Vector
    }

    private Representation representation;

    void Awake()
    {
        representation = Representation.Sprite;
        UpdateRepresentaion();
    }

    public void ChangeRepresentation()
    {
        representation = (int)representation == (int)Representation.Sprite
            ? Representation.Vector
            : Representation.Sprite;

        UpdateRepresentaion();

        var representations = GameObject.FindGameObjectsWithTag("Representation");
        foreach (var obj in representations)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                var child = obj.transform.GetChild(i);
                if (child.tag != "SpriteRepresentation" && child.tag != "VectorRepresentation")
                    continue;

                child.gameObject.SetActive(IsActive(child.tag));
            }
        }
    }

    private void UpdateRepresentaion()
    {
        // TODO: place for improvement
        foreach (var obj in multiRepresentableObjects)
        {
            if (obj == null)
                continue;

            foreach (Transform child in obj.transform)
            {
                if (child.tag != "Representation")
                    continue;

                foreach (Transform represent in child)
                {
                    if (represent.tag != "SpriteRepresentation" && represent.tag != "VectorRepresentation")
                        continue;

                    represent.gameObject.SetActive(IsActive(represent.tag));
                }
            }
        }
    }

    private bool IsActive(string tag)
    {
        /* tag  represent | active
         * -----------------------
         * vec     vec    |   1
         * vec     spr    |   0
         * spr     vec    |   0
         * spr     spr    |   1
         */
        return (tag == "SpriteRepresentation")
                == ((int)representation == (int)Representation.Sprite);
    }

    /// <summary>
    /// Receive the first game object which has component T
    /// </summary>
    /// <typeparam name="T">Wanted component (attached script recommended)</typeparam>
    /// <returns>Game object</returns>
    public GameObject GetGameObjectOfType<T>() where T : Component
    {
        foreach (var obj in multiRepresentableObjects)
        {
            if (obj.GetComponent<T>() != null)
                return obj;
        }

        return null;
    }
}
