using UnityEngine;

public class DrillHead : MonoBehaviour
{
    public float rotationSpeed = 10;
    
    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
}