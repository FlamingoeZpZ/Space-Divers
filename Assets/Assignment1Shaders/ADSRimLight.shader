// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/AmbientDiffuseSpecularRimLight"
{
    Properties {
      _MainTex ("Main Texture", 2D) = "white" {} 
      _Color ("Ambient Color Mult", Color) = (1,1,1,1)
      _RimColor ("Rim Color", Color) = (1,1,1,1)
      _RimPower ("Rim Power", Range(0.5, 16)) = 3
      _SpecPow("Specular Power", range(0,100)) = 50

   }
  
   SubShader{
      Tags {"LightMode"="ForwardBase"}
        CGPROGRAM
      // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf BasicBlinn
        half _SpecPow;
        fixed4 _Color;
        sampler2D _MainTex;

        
        //NOTE: BasicLambert, Basic Blinn too must be in both function and name

        //For flowCharting --> Inputs --> Process --> Output
        
       half4 LightingBasicBlinn(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        {
            const half3 h = normalize(lightDir  + viewDir);
            const half diff = max(0, dot(s.Normal, lightDir));
            const float nh = max(0, dot(s.Normal, h)); // Uses halfway vector
            const float spec = pow(nh, _SpecPow);

           
            half4 c;
            c.rgb = (s.Albedo * _LightColor0.rgb  * diff + _LightColor0.rgb * spec) * atten;
            c.a = s.Alpha;
            return c;
        }

       
        
        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex,  IN.uv_MainTex).rgb * _Color.rgb;
        }
        ENDCG
   
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
