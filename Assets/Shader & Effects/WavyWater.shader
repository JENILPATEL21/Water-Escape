Shader "Custom/WavyWater"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _WaveStrength ("Wave Strength", Float) = 0.02
        _WaveFrequency ("Wave Frequency", Float) = 10.0
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _Tint ("Tint", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        Lighting Off
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            float _WaveSpeed;
            float4 _Tint;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float time = _Time.y * _WaveSpeed;

                // Sine wave distortion for UVs
                float waveX = sin(i.uv.y * _WaveFrequency + time) * _WaveStrength;
                float waveY = cos(i.uv.x * _WaveFrequency + time) * _WaveStrength;

                float2 distortedUV = i.uv + float2(waveX, waveY);
                fixed4 col = tex2D(_MainTex, distortedUV) * _Tint;

                return col;
            }
            ENDCG
        }
    }
}
