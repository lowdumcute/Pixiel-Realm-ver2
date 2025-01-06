Shader "Unlit/GlowShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _EmissionTex ("Emission Texture", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
        _EmissionIntensity ("Emission Intensity", Float) = 1.0
        _FlipX ("FlipX", Float) = 0.0 // Biến điều khiển flipX
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // Sử dụng alpha blending
        ZWrite Off                      // Không ghi vào Z-buffer (để trong suốt hoạt động)

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _EmissionTex;
            float4 _MainTex_ST;
            float4 _EmissionTex_ST;
            fixed4 _EmissionColor;
            float _EmissionIntensity;
            float _FlipX; // Biến điều khiển flipX

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Nếu flipX == 1, đảo ngược UV theo trục X
                float2 flippedUV = v.uv;
                if (_FlipX > 0.5) // Kiểm tra giá trị _FlipX (0 hoặc 1)
                {
                    flippedUV.x = 1.0 - v.uv.x; // Đảo ngược UV theo trục X
                }

                o.uv = TRANSFORM_TEX(flippedUV, _MainTex);

                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample base texture
                fixed4 baseColor = tex2D(_MainTex, i.uv);

                // Kiểm tra alpha cho sự trong suốt
                if (baseColor.a < 0.01) discard;

                // Sample emission texture
                fixed4 emission = tex2D(_EmissionTex, i.uv);

                // Multiply emission texture by color and intensity
                fixed4 emissionColor = emission * _EmissionColor * _EmissionIntensity;

                // Combine base color and emission
                fixed4 finalColor = baseColor + emissionColor;

                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalColor);

                return finalColor;
            }
            
            ENDCG
        }
    }
}
