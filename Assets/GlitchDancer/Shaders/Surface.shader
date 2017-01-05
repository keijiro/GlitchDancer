Shader "Glitch Dancer/Cel-Like Surface"
{
    Properties
    {
        _MainTex("Albedo", 2D) = "white"{}
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #include "Common.cginc"

        #pragma surface surf CelLike nolightmap addshadow
        #pragma target 3.0

        sampler2D _MainTex;
        fixed3 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
        }

        ENDCG
    }
}
