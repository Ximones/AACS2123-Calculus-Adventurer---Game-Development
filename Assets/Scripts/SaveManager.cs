using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // List of saved GameObject states
    public List<GameObjectState> savedObjects = new List<GameObjectState>();

    // Save the state of all GameObjects in the scene
    public void SaveSceneState(GameObject[] gameObjectsToSave)
    {
        savedObjects.Clear();  // Clear any previous saves

        foreach (var obj in gameObjectsToSave)
        {
            savedObjects.Add(new GameObjectState(obj));  // Save each object's state
        }
    }

    // Restore the state of the GameObjects in the scene
    public void LoadSceneState()
    {
        foreach (var state in savedObjects)
        {
            // Find the GameObject in the scene by name
            GameObject obj = GameObject.Find(state.objectName);

            if (obj != null)
            {
                // Restore position, rotation, and active state
                obj.transform.position = state.position;
                obj.transform.rotation = state.rotation;
                obj.SetActive(state.isActive);
            }
        }
    }
}

[System.Serializable]
public class GameObjectState
{
    public string objectName;      // Name of the GameObject
    public Vector3 position;       // Object's position
    public Quaternion rotation;    // Object's rotation
    public bool isActive;          // Whether the object is active or destroyed

    // Constructor to capture the current state of a GameObject
    public GameObjectState(GameObject obj)
    {
        objectName = obj.name;
        position = obj.transform.position;
        rotation = obj.transform.rotation;
        isActive = obj.activeSelf;
    }
}

