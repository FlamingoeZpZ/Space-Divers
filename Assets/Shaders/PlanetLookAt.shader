Shader "Unlit/PlanetLookAt"
{
   Properties {
      _MainTex ("Texture Image", 2D) = "white" {}
      _ScaleX ("Scale X", Float) = 1.0
      [HDR]_Color ("Color", Color) = (1,1,1,1)
   }
   //https://stackoverflow.com/questions/57204343/can-a-shader-rotate-shapes-to-face-camera
   SubShader {
      Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      Pass {   
         HLSLPROGRAM

         #pragma vertex vert  
         #pragma fragment frag

         #include "UnityCG.cginc"

         // User-specified uniforms            
         uniform sampler2D _MainTex;        
         uniform float _ScaleX;
         fixed4 _Color;
         float4 _PlayerPos;

         struct vertexInput {
            float4 vertex : POSITION;
            float4 tex : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
         };
         
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;

            //Look at player
            //_PlayerPos /= scalef;

            output.pos = mul(UNITY_MATRIX_P,
              mul(UNITY_MATRIX_MV, float4(_PlayerPos.x, _PlayerPos.y, _PlayerPos.z, 1)) // Modified matrix to move object
              + float4(input.vertex.x, input.vertex.y, 0.0, 0.0)
              * float4(_ScaleX, _ScaleX, 1.0, 1.0));
            
            output.tex = input.tex;

            return output;
         }

         float4 frag(vertexOutput input) : COLOR
         {
            return tex2D(_MainTex, float2(input.tex.xy)) * _Color;   
         }

         ENDHLSL
      }
   }
}
