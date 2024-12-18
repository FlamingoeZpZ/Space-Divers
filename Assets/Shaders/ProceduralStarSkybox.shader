Shader "Custom/ProceduralStarSkybox" {
   
   //TODO:
   //1) Perlin noise? based stars Multiplied by random colors...? 
   //2) Color grading to gcreate "Milky" effect... Nebula dust?
   //3) 
   
   Properties {
      _Cube ("Environment Map", Cube) = "" {}
   }
   SubShader {
      Tags { "Queue" = "Background" }
      
      Pass {   
         ZWrite Off
         Cull Front

         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc" // What does this do?

         // User-specified uniforms
         uniform samplerCUBE _Cube;   
 
         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float3 viewDir : TEXCOORD1;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = unity_ObjectToWorld;
            output.viewDir = mul(modelMatrix, input.vertex).xyz - _WorldSpaceCameraPos;
            output.pos = UnityObjectToClipPos(input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            return texCUBE(_Cube, input.viewDir);
         }
 
         ENDCG
      }
   }
}