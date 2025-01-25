Shader "Custom/S_Dissolve"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _AlphaMask("Alpha Mask", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
    }

        SubShader
        {
            Tags {"Queue" = "AlphaTest" "RenderType" = "TransparentCutout"}

            CGPROGRAM
            #pragma surface surf Standard alpha:fade

            sampler2D _MainTex;
            sampler2D _AlphaMask;
            half _Cutoff;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_AlphaMask;
            };

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo texture
                half4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = c.rgb;

                // Alpha mask for cutout
                half alphaMask = tex2D(_AlphaMask, IN.uv_AlphaMask).r;
                o.Alpha = step(_Cutoff, alphaMask);
            }
            ENDCG
        }

            FallBack "Diffuse"
}
