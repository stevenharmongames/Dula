Shader "Custom/S_Dissolve"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _AlphaMask("Alpha Mask", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
        _TintColor("Tint Color", Color) = (1,1,1,1)
    }

        SubShader
        {
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 200

            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha
                ZWrite On

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                sampler2D _MainTex;
                sampler2D _AlphaMask;
                half _Cutoff;
                fixed4 _TintColor;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    UNITY_INITIALIZE_OUTPUT(v2f, o);
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    UNITY_TRANSFER_FOG(o,o.pos);
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    // Albedo texture
                    half4 c = tex2D(_MainTex, i.uv);

                    // Apply tint color
                    c.rgb *= _TintColor.rgb;

                    // Alpha mask for cutout
                    half alphaMask = tex2D(_AlphaMask, i.uv).r;
                    c.a *= step(_Cutoff, alphaMask);

                    UNITY_APPLY_FOG(i.fogCoord, c);

                    return c;
                }
                ENDCG
            }
        }

            Fallback "Diffuse"
}
