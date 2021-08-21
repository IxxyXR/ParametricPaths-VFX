using UnityEngine;
using UnityEngine.Serialization;

public class RotateGameobject : MonoBehaviour
{
    public Vector3 angles = Vector3.zero;
    
    void Update()
    {
        var rot = transform.localRotation;
        rot *= Quaternion.Euler(angles);
        transform.localRotation = rot;
    }
}