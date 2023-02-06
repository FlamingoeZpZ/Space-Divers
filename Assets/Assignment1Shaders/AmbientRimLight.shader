// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/AmbientRimLight"
{
    Properties {
      _MainTex ("Main Texture", 2D) = "white" {} 
      _Color ("Ambient Color Mult", Color) = (1,1,1,1)
      _RimColor ("Rim Color", Color) = (1,1,1,1)
      _RimPower ("Rim Power", Range(0.5, 16)) = 3
    }
  
   SubShader{
      Tags { "LightMode" = "ForwardBase"  "RenderType"="Opaque"} 

      Pass {    
CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // just pass the texture coordinate
                o.uv = v.uv;
                return o;
            }
            
            // texture we will sample
            sampler2D _MainTex;
            fixed4 _Color;
            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG    
           }
      CGPROGRAM
         
        #pragma surface surf Lambert alpha:fade
        struct Input
        {
            float3 viewDir;
        };

        float4 _RimColor;
        float _RimPower;

        void surf(Input IN, inout SurfaceOutput o)
        {
            const half rim = 1- saturate(dot(normalize(IN.viewDir), o.Normal));
            const float p = pow( rim, _RimPower);
            o.Emission = _RimColor.rgb * p * 10;
            o.Alpha = p;
        }
        ENDCG
      
   }
    
   Fallback "Diffuse"

}
