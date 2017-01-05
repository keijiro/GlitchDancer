// Custom surface shader for Skinner Particle
#include "Common.cginc"

// Buffers from the animation kernels
sampler2D _PositionBuffer;
sampler2D _VelocityBuffer;
sampler2D _RotationBuffer;

// Scale modifier
float2 _Scale; // (min, max)

// Color modifier
half _CutoffSpeed;
half _SpeedToIntensity;

struct Input
{
    fixed4 color : COLOR;
    fixed facing : VFACE;
};

void vert(inout appdata_full data)
{
    // Particle ID
    float id = data.texcoord1.x;

    // Fetch samples from the animation kernel.
    float4 texcoord = float4(id, 0.5, 0, 0);
    float4 P = tex2Dlod(_PositionBuffer, texcoord);
    float4 V = tex2Dlod(_VelocityBuffer, texcoord);
    float4 R = tex2Dlod(_RotationBuffer, texcoord);

    // Attribute modifiers
    half speed = length(V.xyz);
    half scale = ParticleScale(id, P.w + 0.5, V.w, _Scale);
    half intensity = saturate((speed - _CutoffSpeed) * _SpeedToIntensity);

    // Modify the vertex attributes.
    data.vertex.xyz = RotateVector(data.vertex.xyz, R) * scale + P.xyz;
    data.normal = RotateVector(data.normal, R);
    data.color.rgb = ColorAnimationSimple(id, intensity);
}

void surf(Input IN, inout SurfaceOutput o)
{
    o.Albedo = IN.color.rgb;
    o.Normal = half3(0, 0, IN.facing > 0 ? 1 : -1);
}
