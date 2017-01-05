Shader "Hidden/Glitch Dancer/PostEffects"
{
    Properties
    {
        _MainTex("", 2D) = ""{}
        _LineColor("", Color) = (0, 0, 0, 1)
        _BGColor("", Color) = (1, 1, 1, 0)
    }
    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0
            #include "PostEffects.cginc"
            ENDCG
        }
    }
}
