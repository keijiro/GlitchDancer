Shader "GlitchDancer/GradientSkybox"
{
    Properties
    {
        _Color1("Color 1", Color) = (0, 0, 0)
        _Color2("Color 2", Color) = (1, 1, 1)
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            fixed3 _Color1;
            fixed3 _Color2;

            struct appdata_t {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                float2 up = normalize(mul(UNITY_MATRIX_V, float4(0, 1, 0, 0)).xy);

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = dot(o.vertex.xy / o.vertex.z, up);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed p = saturate(i.texcoord.y * 0.5 + 0.5);
                return half4(lerp(_Color1, _Color2, p), 1);
            }

            ENDCG
        }
    }
}
