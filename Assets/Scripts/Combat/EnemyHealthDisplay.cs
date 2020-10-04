using System;
using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health target = fighter.GetTarget();
            if (target == null)
            {
                GetComponent<Text>().text = "N/A";
            }
            else
            {
                GetComponent<Text>().text = String.Format("{0:0}%", target.GetPercentage());
            }
        }
    }
}
