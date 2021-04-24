using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(2 * Time.deltaTime, -2 * Time.deltaTime, 0);
    }
}