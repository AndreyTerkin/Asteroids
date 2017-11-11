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
        //representation = PlayerPrefs.GetInt("Representation", -1) == -1
        //    ? Representation.Sprite
        //    : (Representation)PlayerPrefs.GetInt("Representation");
        representation = Representation.Vector;

        UpdateRepresentaion();
    }

    public void ChangeRepresentation()
    {
        representation = (int)representation == (int)Representation.Sprite
            ? Representation.Vector
            : Representation.Sprite;

        UpdateRepresentaion();
    }

    private void UpdateRepresentaion()
    {
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
                    represent.gameObject.SetActive(represent.tag == "SpriteRepresentation"
                                                   ^ (int)representation == (int)Representation.Sprite);
                }
            }
        }
    }
}
