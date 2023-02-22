// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/AmbientDiffuseRimLight"
{
    Properties {
      _MainTex ("Main Texture", 2D) = "white" {} 
      _Color ("Ambient Color Mult", Color) = (1,1,1,1)
      [HDR] _RimColor ("Rim Color", Color) = (1,1,1,1)
      _RimPower ("Rim Power", Range(0.5, 16)) = 3
    }
  
   SubShader{
       Tags {
          "LightMode" = "UniversalForward"
          } 
        LOD 300
      Pass {  
           
HLSLPROGRAM
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
                float3 worldPos : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                // get vertex normal in world space
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = normalize(worldNormal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                // factor in the light color
                o.diff = nl * _LightColor0;
                return o;
            }
            
            sampler2D _MainTex;
            fixed4 _Color;

            float4 _RimColor;
            float _RimPower;

            fixed4 frag (v2f i) : SV_Target
            {
                const half rim = 1- saturate(dot(i.worldPos, i.viewDir));
                const float p = pow( rim, _RimPower);
                const fixed4 rimCol = _RimColor.rgba * p;
                const fixed4 mainCol =tex2D(_MainTex, i.uv) * _Color * i.diff;
                return rimCol + mainCol;
            }
            ENDHLSL    
           }
   }
    
   Fallback "Diffuse"

}
