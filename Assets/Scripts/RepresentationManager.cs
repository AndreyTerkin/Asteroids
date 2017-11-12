using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Representation : int
{
    Sprite,
    Vector
}

public class RepresentationManager : MonoBehaviour
{
    public Transform backgound;
    public List<Transform> multiRepresentableObjects;

    private Representation representation;
    public Representation Representation { get { return representation; } }

    void Start()
    {
        representation = PlayerPrefs.GetInt("Representation", -1) == -1
            ? Representation.Sprite
            : (Representation)PlayerPrefs.GetInt("Representation");

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
        // TODO: place fot improvement
        foreach (var obj in multiRepresentableObjects)
        {
            if (obj == null)
                continue;

            foreach (Transform child in obj)
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
}
