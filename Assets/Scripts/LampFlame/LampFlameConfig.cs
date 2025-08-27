﻿using UnityEngine;

namespace LampFlame
{
    [CreateAssetMenu(menuName = "Config/LampFlame")]
    public class LampFlameConfig : ScriptableObject
    {
        [Range(0, 20)] public float maxValue = 10f;
        [Range(0, 20)] public float minValue = 0f;
    }
}