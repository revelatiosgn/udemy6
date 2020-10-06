using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        float healthPoints = -1f;

        bool isDead = false;

        void Start()
        {
            if (healthPoints < 0f)
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);

            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage " + damage);

            healthPoints = Mathf.Max(healthPoints - damage, 0f);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience != null)
            {
                experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
            }
        }

        public float GetPoints()
        {
            return healthPoints;
        }

        public float GetMaxPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void Die()
        {
            if (isDead)
                return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float) state;

            if (healthPoints == 0)
            {
                Die();
            }
        }

        public void RegenerateHealth()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }
    }
}


