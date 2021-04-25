using System;
using UnityEngine;

namespace Player
{
    public class Miner : MonoBehaviour
    {
        public int rayLines = 10;
        public float totalHeight = 2.5f;
        public float decraseEachStepBy = .18f;
        public float startLength = 1.8f;
        public float startY = -.6f;

        public GameObject checkParent;
        public LevelGenerator level;

        private void Update()
        {
            for (var i = 0; i < checkParent.transform.childCount; i++)
            {
                var child = checkParent.transform.GetChild(i);

                level.DestroyTile(
                    Mathf.FloorToInt(child.transform.position.x),
                    Mathf.FloorToInt(child.transform.position.y)
                );
            }


            // for (var i = 0; i < rayLines; i++)
            // {
            //     // now shoot one left and one right
            //     var angle = transform.rotation.eulerAngles.z * Mathf.PI / 180;
            //     var position = transform.position + new Vector3(0, startY - totalHeight / (rayLines + 2) * i, 0);
            //
            //     
            //     Gizmos.DrawRay(position, transform.right * length);
            //     Gizmos.DrawRay(position, -transform.right * length);
            //
            //     length -= decraseEachStepBy;
            // }
        }

        public void GenerateChecks()
        {
            Debug.Log("Generating check points");
            
            var length = startLength;

            for (var i = 0; i < rayLines; i++)
            {
                // now shoot one left and one right
                var angle = transform.rotation.eulerAngles.z * Mathf.PI / 180;
                var position = transform.position + new Vector3(0, startY - totalHeight / (rayLines + 2) * i, 0);

                var sphereRight = new GameObject("Check_Right_" + i);
                sphereRight.transform.parent = checkParent.transform;
                sphereRight.transform.position = position + transform.right * length; 
                
                var sphereLeft = new GameObject("Check_Left_" + i);
                sphereLeft.transform.parent = checkParent.transform;
                sphereLeft.transform.position = position - transform.right * length; 

                length -= decraseEachStepBy;
            }

            Debug.Log("Done!");
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            var length = startLength;

            for (var i = 0; i < rayLines; i++)
            {
                // now shoot one left and one right
                var angle = transform.rotation.eulerAngles.z * Mathf.PI / 180;
                var position = transform.position + new Vector3(0, startY - totalHeight / (rayLines + 2) * i, 0);
                
                Gizmos.DrawRay(position, transform.right * length);
                Gizmos.DrawRay(position, -transform.right * length);
                
                Gizmos.DrawSphere(
                    position + transform.right * length,
                    .1f
                );
                
                Gizmos.DrawSphere(
                    position - transform.right * length,
                    .1f
                ); 

                length -= decraseEachStepBy;
            }
        }
    }
}