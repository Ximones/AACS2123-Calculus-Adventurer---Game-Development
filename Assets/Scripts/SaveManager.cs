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
        // Clear the existing GameManager's array since we will update it
        GameManager.Instance.gameObjectsInLevel1 = new GameObject[savedObjects.Count];

        for (int i = 0; i < savedObjects.Count; i++)
        {
            var state = savedObjects[i];

            // Find the GameObject in the scene by name
            GameObject obj = GameObject.Find(state.objectName);

            if (obj != null)
            {
                // Restore position, rotation, and active state
                obj.transform.position = state.position;
                obj.transform.rotation = state.rotation;
                obj.SetActive(state.isActive);

                // Update GameManager's array with the restored objects
                GameManager.Instance.gameObjectsInLevel1[i] = obj;
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

