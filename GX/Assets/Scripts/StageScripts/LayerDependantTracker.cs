using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerDependantTracker : MonoBehaviour
{
    //unity uses a maximum of 32 layers, so we use this number to account for all
    private const int layerCount = 32;
    //series of lists storing characters which must move with each layer
    public static List<GameObject>[] objectsPerLayer = new List<GameObject>[layerCount];

    // Start is called before the first frame update
    void Awake()
    {
       //initialize objectsPerLayer
        for (int i = 0; i < layerCount; ++i)
        {
            objectsPerLayer[i] = new List<GameObject>();
        }
    }

    // Add object to list of the layer it is on
    public static void AddObjectToLayer(GameObject newObject, int targetLayer)
    {
        Debug.Log("target layer = " + targetLayer);//test
        objectsPerLayer[targetLayer].Add(newObject);
    }

    // Remove an object from the list of the layer is was previously on
    public static void RemoveObjectFromLayer(GameObject oldObject, int targetLayer)
    {
        objectsPerLayer[targetLayer].Remove(oldObject);
    }
}
