﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 99)] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;

        int currentLevel = 0;

        private void Start() 
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, CalculateLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
                currentLevel = CalculateLevel();

            return currentLevel;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null)
                return startingLevel;

            float currentXP = experience.GetPoints();
            float[] levels = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int i = 0; i < levels.Length; i++)
            {
                if (currentXP < levels[i])
                    return i + 1;
            }

            return levels.Length + 1;
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            if (!shouldUseModifiers)
                return 0f;

            float total = 0f;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
        
        private float GetPercentageModifiers(Stat stat)
        {
            if (!shouldUseModifiers)
                return 0f;
                
            float total = 0.0f;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1.0f + GetPercentageModifiers(stat) / 100.0f);
        }
    }
}