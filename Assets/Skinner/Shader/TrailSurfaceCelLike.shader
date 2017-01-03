// Custom surface shader for Skinner Trail
Shader "Skinner/Trail (CelLike)"
{
    Properties
    {
        _Albedo("Albedo", Color) = (0.5, 0.5, 0.5)

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
        [HideInInspector] _OrthnormBuffer("", 2D) = ""{}

        [HideInInspector] _PreviousPositionBuffer("", 2D) = ""{}
        [HideInInspector] _PreviousOrthnormBuffer("", 2D) = ""{}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "NormalDrawer"="Trail" }
        Cull Off
        CGPROGRAM
        #pragma surface surf CelLike2 vertex:vert nolightmap fullforwardshadows
        #pragma target 3.0
        #include "TrailSurfaceCelLike.cginc"
        ENDCG
    }
}
