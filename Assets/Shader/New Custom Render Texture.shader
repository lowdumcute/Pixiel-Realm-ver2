Shader "Custom/GlowOutline"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.02
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 offset1 = float2(_OutlineThickness, 0);
                float2 offset2 = float2(0, _OutlineThickness);

                float alpha = tex2D(_MainTex, i.uv).a;
                alpha += tex2D(_MainTex, i.uv + offset1).a;
                alpha += tex2D(_MainTex, i.uv - offset1).a;
                alpha += tex2D(_MainTex, i.uv + offset2).a;
                alpha += tex2D(_MainTex, i.uv - offset2).a;

                fixed4 col = tex2D(_MainTex, i.uv);
                if (alpha > 0.0 && col.a == 0.0)
                {
                    return _OutlineColor;
                }

                return col;
            }
            ENDCG
        }
    }
}
