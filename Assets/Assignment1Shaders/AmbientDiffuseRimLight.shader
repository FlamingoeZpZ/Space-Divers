// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/AmbientDiffuseRimLight"
{
    Properties {
      _MainTex ("Main Texture", 2D) = "white" {} 
      _Color ("Ambient Color Mult", Color) = (1,1,1,1)
      _RimColor ("Rim Color", Color) = (1,1,1,1)
      _RimPower ("Rim Power", Range(0.5, 16)) = 3
    }
  
   SubShader{
      Tags {"LightMode"="ForwardBase"}

      Pass {    
CGPROGRAM
            // indicate that our pass is the "base" pass in forward
            // rendering pipeline. It gets ambient and main directional
            // light data set up; light direction in _WorldSpaceLightPos0
            // and color in _LightColor0
            
        
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // for UnityObjectToWorldNormal
            #include "UnityLightingCommon.cginc" // for _LightColor0

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 diff : COLOR0; // diffuse lighting color
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                // get vertex normal in world space
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                // factor in the light color
                o.diff = nl * _LightColor0;
                return o;
            }
            
            sampler2D _MainTex;
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                // multiply by lighting
                col *= i.diff;
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
