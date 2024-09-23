using System;
using Sandbox;

namespace Frostrial;

public sealed class LightFlicker : Component
{
    [Property] public PointLight Light { get; set; }
    [Property] public float BaseAttenuation { get; set; } = 300;
    [Property] public float CosAmplitude { get; set; } = 50;
    [Property] public float CosFrequency { get; set; } = 2;

    protected override void OnUpdate()
    {
        Light.Attenuation =
            BaseAttenuation + (float)Math.Cos(Time.Now * CosFrequency) * CosAmplitude * (1 + Time.Now % 1); // Acceptable flickering
    }
}