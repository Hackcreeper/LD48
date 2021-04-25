using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 4;
    public LevelGenerator level;

    private int minAngle;
    private int maxAngle;
    
    private void Update()
    {
        // How to handle rotation
        // We only allow rotation from min and max angles
    
        transform.Translate(0, -speed * Time.deltaTime, 0);

        Drill(2, 0, 3);
        Drill(1, 3, 5);
        level.DestroyTile(
            Mathf.FloorToInt(transform.position.x),
            Mathf.FloorToInt(transform.position.y - 5)
        );   
    }

    private void Drill(int xRange, int yStart, int yEnd)
    {
        for (var x = -xRange; x <= xRange; x++)
        {
            for (var y = yStart; y < yEnd; y++)
            {
                level.DestroyTile(
                    Mathf.FloorToInt(transform.position.x + x),
                    Mathf.FloorToInt(transform.position.y - y)
                );
            }
        }
    }
}