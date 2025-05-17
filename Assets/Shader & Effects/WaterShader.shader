Shader "Custom/2DWater"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _WaveStrength ("Wave Strength", Float) = 0.05
        _WaveFrequency ("Wave Frequency", Float) = 10.0
        _ScrollSpeed ("Scroll Speed", Float) = 0.2
        _Transparency ("Alpha", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Lighting Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _WaveStrength;
            float _WaveFrequency;
            float _ScrollSpeed;
            float _Transparency;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Scroll and distort the UV
                uv.x += _Time.y * _ScrollSpeed;
                uv.y += sin((uv.x + _Time.y) * _WaveFrequency) * _WaveStrength;

                fixed4 col = tex2D(_MainTex, uv);
                col.a *= _Transparency;
                return col;
            }
            ENDCG
        }
    }
}
