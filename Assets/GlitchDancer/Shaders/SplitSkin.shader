Shader "Glitch Dancer/Cel-Like Split Skin"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _Scale("Triangle Scale", Range(0, 5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #include "Common.cginc"

        #pragma surface surf CelLike vertex:vert nolightmap addshadow
        #pragma target 3.0

        fixed3 _Color;
        half _Scale;

        struct Input
        {
            fixed facing : VFACE;
        };

        void vert(inout appdata_full v)
        {
            float3 P = v.vertex.xyz;
            float3 N = v.normal;
            float3 T = v.tangent.xyz;
            float3 B = cross(N, T) * v.tangent.w;

            float3 C = P + T * v.texcoord.x + B * v.texcoord.y;
            float S = _Scale + abs(_Scale - 1) * sin(_Time.y * 6 + P.x * 2) * 0.5;

            v.vertex.xyz = lerp(C, P, S);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color;
            o.Normal = float3(0, 0, IN.facing > 0 ? 1 : -1);
        }

        ENDCG
    }
}
