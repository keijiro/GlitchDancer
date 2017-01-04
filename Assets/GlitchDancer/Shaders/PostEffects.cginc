#include "UnityCG.cginc"

sampler2D _MainTex;
float2 _MainTex_TexelSize;

fixed4 _Color1;
fixed3 _Color2;
fixed4 _LineColor;

fixed _Glitch;

fixed4 frag(v2f_img i) : SV_Target
{
    float2 uv = i.uv;
    float4 disp = _MainTex_TexelSize.xyxy * float4(1, 1, -1, 0);

    // Glitch noise
    uv.x += (frac(uv.y * 366.747893 + _Time.y * 7.989238) - 0.5) * _Glitch;

    fixed3 s0 = tex2D(_MainTex, uv).rgb;           // TL
    fixed3 s1 = tex2D(_MainTex, uv + disp.xy).rgb; // BR
    fixed3 s2 = tex2D(_MainTex, uv + disp.xw).rgb; // TR
    fixed3 s3 = tex2D(_MainTex, uv + disp.wy).rgb; // BL

    // Roberts cross operator
    fixed3 g1 = s1 - s0;
    fixed3 g2 = s3 - s2;
    fixed edge = saturate(sqrt(dot(g1, g1) + dot(g2, g2)));

    // Color mixing
    fixed4 c0 = tex2D(_MainTex, uv);

    fixed3 rgb = lerp(_Color1, _Color2, dot(c0.rgb, 0.333));
    rgb = lerp(c0.rgb, rgb, _Color1.a);
    rgb = lerp(rgb, _LineColor.rgb, edge * _LineColor.a);

    return fixed4(rgb, c0.a);
}
