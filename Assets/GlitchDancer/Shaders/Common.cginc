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
