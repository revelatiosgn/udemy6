using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), 0.5f);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextWaypointIndex(i)));
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextWaypointIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }
    }
}
