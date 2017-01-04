#include "UnityCG.cginc"

sampler2D _MainTex;
float2 _MainTex_TexelSize;

half4 _LineColor;
half4 _BGColor;

half2 _Threshold; // (low, high)

half4 frag(v2f_img i) : SV_Target
{
    const float4 disp = _MainTex_TexelSize.xyxy * float4(1, 1, -1, 0);

    fixed3 s0 = tex2D(_MainTex, i.uv).rgb;           // TL
    fixed3 s1 = tex2D(_MainTex, i.uv + disp.xy).rgb; // BR
    fixed3 s2 = tex2D(_MainTex, i.uv + disp.xw).rgb; // TR
    fixed3 s3 = tex2D(_MainTex, i.uv + disp.wy).rgb; // BL

    // Roberts cross operator
    fixed3 g1 = s1 - s0;
    fixed3 g2 = s3 - s2;
    fixed g = sqrt(dot(g1, g1) + dot(g2, g2));

    // Thresholding
    g = saturate((g - _Threshold.x) / (_Threshold.y - _Threshold.x));

    half4 cs = tex2D(_MainTex, i.uv);
    half3 c0 = lerp(cs.rgb, _BGColor.rgb, _BGColor.a);
    half3 co = lerp(c0, _LineColor.rgb, g * _LineColor.a);
    return half4(co, cs.a);
}
