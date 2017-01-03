Shader "DrawNormal"
{
    SubShader
    {
        Tags { "NormalDrawer" = "Simple" }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 color : COLOR;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.normal * 0.5 + 0.5;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(i.color, 1);
            }

            ENDCG
        }
    }

    SubShader
    {
        Tags { "NormalDrawer" = "Trail" }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "../../Skinner/Shader/Common.cginc"

            sampler2D _PositionBuffer;
            sampler2D _VelocityBuffer;
            sampler2D _OrthnormBuffer;

            half3 _LineWidth; // (max width, cutoff, speed-to-width / max width)

            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 color : COLOR;
            };

            v2f vert(appdata_t data)
            {
                // Line ID
                float id = data.vertex.x;

                // Fetch samples from the animation kernel.
                float4 texcoord = float4(data.vertex.xy, 0, 0);
                float3 P = tex2Dlod(_PositionBuffer, texcoord).xyz;
                float3 V = tex2Dlod(_VelocityBuffer, texcoord).xyz;
                float4 B = tex2Dlod(_OrthnormBuffer, texcoord);

                // Extract normal/binormal vector from the orthnormal sample.
                half3 normal = StereoInverseProjection(B.xy);
                half3 binormal = StereoInverseProjection(B.zw);

                // Attribute modifiers
                half speed = length(V);

                half width = _LineWidth.x * data.vertex.z * (1 - data.vertex.y);
                width *= saturate((speed - _LineWidth.y) * _LineWidth.z);

                float4 vp = float4(P + binormal * width, data.vertex.w);

                v2f o;
                o.vertex = UnityObjectToClipPos(vp);
                o.color = normal * 0.5 + 0.5;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(i.color, 1);
            }

            ENDCG
        }
    }
}
