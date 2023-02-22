Shader "Custom/SkyboxCubemap" {
    Properties {
        _Tint ("Tint", Color) = (1,1,1,1)
        _Exposure ("Exposure", Range(0.1, 10.0)) = 1.0
        _SunSize ("Sun Size", Range(0, 0.1)) = 1.0
        _SunStrength ("Sun Strength", Range(0, 100.0)) = 1.0
        _MainTex ("Cubemap", Cube) = "" {}
    }
 
    SubShader {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include <UnityLightingCommon.cginc>
            #include <UnityStandardBRDF.cginc>

            #include "UnityCG.cginc"
 
            samplerCUBE _MainTex;
            float _Exposure;
            float _SunSize;
            float _SunStrength;
            fixed4 _Tint;
 
            struct appdata {
                float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 base_texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
				float4 base_texcoord1 : TEXCOORD1;
				float4 base_texcoord2 : TEXCOORD2;

            };
 
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.base_texcoord1.xyz = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				o.base_texcoord2.xyz = v.base_texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.base_texcoord1.w = 0;
				o.base_texcoord2.w = 0;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target {
                float3 worldNormal = normalize(i.worldPos - _WorldSpaceCameraPos.xyz);
                float3 worldView = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
                float3 worldReflection = reflect(worldView, worldNormal);
                float4 color = texCUBE(_MainTex, worldReflection);
                color.rgb *= _Tint.rgb;
                color.rgb *= _Exposure;

                #if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
				float4 base_lightColor = 0;
				#else //aselc
				float4 base_lightColor = _LightColor0;
				#endif //aselc
                
                float3 ase_worldPos = i.base_texcoord1.xyz;
				float3 worldSpaceLightDir = Unity_SafeNormalize(UnityWorldSpaceLightDir(ase_worldPos));
				float3 ase_sunPos = i.base_texcoord2.xyz; // I think it's un pos?
								float sun = (1 - length( worldSpaceLightDir - ase_sunPos));
				float InvSunSize = 1 - _SunSize;
				float sunBlur = smoothstep( (1 - _SunSize*_SunStrength-0.01) , InvSunSize , sun);
				float sunGlow = smoothstep(  (1 - _SunSize*_SunStrength*2) , InvSunSize , sun);
				
				return color + sunBlur  + sunGlow * InvSunSize * ( base_lightColor / 3.0 );
                
            }
            ENDCG
        }
    }
    FallBack "Skybox/Cubemap"
}