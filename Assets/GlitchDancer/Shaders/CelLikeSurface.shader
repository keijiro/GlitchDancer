Shader "GlitchDancer/CellLike"
{
    Properties
    {
        _MainTex("Albedo", 2D) = "white"{}
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        fixed4 LightingCelLike(SurfaceOutput s, half3 lightDir, fixed atten)
        {
            const fixed kCuts = 4;
            const fixed kRcpCuts = 1.0 / 4;
            const fixed kLightBias = -0.2;

            // Stepping gradient parameter
            fixed nl = (dot(s.Normal, lightDir) + 1) * 0.5;
            nl = round(nl * kCuts + kLightBias) * kRcpCuts;

            // For [0.0, 0.5] -- from black to the albedo color
            fixed3 c1 = s.Albedo * min(nl * 2, 1);

            // For [0.5, 1.0] -- from the albedo color to the light color
            fixed3 c2 = lerp(c1, _LightColor0.rgb, max(nl * 2 - 1, 0));

            return half4(c2 * atten, s.Alpha);
        }

        #pragma surface surf CelLike nolightmap fullforwardshadows
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
    FallBack "Diffuse"
}
