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

                child.gameObject.SetActive(child.tag == "SpriteRepresentation"
                                           ^ (int)representation == (int)Representation.Sprite);
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

                    represent.gameObject.SetActive(represent.tag == "SpriteRepresentation"
                                                   ^ (int)representation == (int)Representation.Sprite);
                }
            }
        }
    }
}
