fixed4 LightingUnlit(SurfaceOutput s, half3 lightDir, fixed atten)
{
    return half4(s.Albedo, s.Alpha);
}
