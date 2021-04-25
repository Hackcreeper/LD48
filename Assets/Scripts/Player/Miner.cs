using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Miner : MonoBehaviour
    {
        public int points = 10;
        public float startY = -.6f;
        public float width = 3f;

        public GameObject checkParent;
        public LevelGenerator level;
        public Control player;

        private void Update()
        {
            for (var i = 0; i < checkParent.transform.childCount; i++)
            {
                var child = checkParent.transform.GetChild(i);

                if (child.localPosition.x < 0)
                {
                    player.score += level.DestroyTile(
                        Mathf.FloorToInt(child.transform.position.x),
                        Mathf.FloorToInt(child.transform.position.y)
                    );

                    continue;
                }
                
                player.score += level.DestroyTile(
                    Mathf.RoundToInt(child.transform.position.x),
                    Mathf.RoundToInt(child.transform.position.y)
                );
            }
        }

        public void GenerateChecks()
        {
            Debug.Log("Generating check points");

            foreach (var position in GetPoints())
            {
                var sphereRight = new GameObject("Check");
                sphereRight.transform.parent = checkParent.transform;
                sphereRight.transform.position = position;
            }

            Debug.Log("Done!");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            foreach (var position in GetPoints())
            {
                Gizmos.DrawSphere(
                    position,
                    .1f
                );
            }
        }

        private List<Vector3> GetPoints()
        {
            var pointList = new List<Vector3>();

            for (var i = 0; i < points; i++)
            {
                pointList.Add(transform.position + new Vector3(
                    -width / 2f + (width / (points - 1) * i),
                    startY,
                    0
                ));
            }

            return pointList;
        }
    }
}