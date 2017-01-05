// Custom surface shader for Skinner Particle
Shader "Skinner/Particle/Cel-Like"
{
    Properties
    {
        [Header(Self Illumination)]
        _BaseHue("Base Hue", Range(0, 1)) = 0
        _HueRandomness("Hue Randomness", Range(0, 1)) = 0.2
        _Saturation("Saturation", Range(0, 1)) = 1
        _Brightness("Brightness", Range(0, 6)) = 0.8

        [Header(Color Modifier (By Speed))]
        _CutoffSpeed("Cutoff Speed", Float) = 0.5
        _SpeedToIntensity("Sensitivity", Float) = 1
        _BrightnessOffs("Brightness Offset", Range(0, 6)) = 1.0
        _HueShift("Hue Shift", Range(-1, 1)) = 0.2

        [HideInInspector] _PositionBuffer("", 2D) = ""{}
        [HideInInspector] _VelocityBuffer("", 2D) = ""{}
        [HideInInspector] _RotationBuffer("", 2D) = ""{}

        [HideInInspector] _PreviousPositionBuffer("", 2D) = ""{}
        [HideInInspector] _PreviousRotationBuffer("", 2D) = ""{}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off
        CGPROGRAM
        #pragma surface surf Unlit vertex:vert nolightmap addshadow
        #pragma target 3.0
        #include "ParticleSurfaceCelLike.cginc"
        #include "Unlit.cginc"
        ENDCG
    }
}
