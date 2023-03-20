// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/UISimpleGradient"
{
    Properties
    {
        _Texture ("Texture", 2D) = "black" {}
        _BelowColor ("BelowColor", Color) = (0,0,0)
        _AboveColor("AboveColor", Color) = (1,1,1)
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always
        ZTest Always
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
                o.uv = v.uv;
                return o;
            }

            sampler2D _Texture;
            fixed4 _BelowColor;
            fixed4 _AboveColor;

            fixed4 frag (v2f i) : SV_Target
            {
               // i.pos = UnityObjectToClipPos (v.vertex);
                const fixed4 col = lerp(_BelowColor,_AboveColor, i.uv.y) + tex2D(_Texture, i.uv);
                return col;
            }
            ENDHLSL
        }
    }
}
