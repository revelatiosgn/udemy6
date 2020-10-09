using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<float> healthPoints;

        bool isDead = false;

        private void Awake() {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        public float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage " + damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0f);

            if (healthPoints.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            {
                takeDamage.Invoke(damage);
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
            return healthPoints.value;
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
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float) state;

            if (healthPoints.value == 0)
            {
                Die();
            }
        }

        public void RegenerateHealth()
        {
            healthPoints.value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void Heal(float points)
        {
            healthPoints.value = Mathf.Clamp(healthPoints.value + points, 0f, GetMaxPoints());
        }
    }
}


