Shader "Custom/FusionShaderHLSL"
{
    Properties
    {
        _Tex0 ("Texture0", 2D) = "white" {}
        _Tex1 ("Texture1", 2D) = "white" {}
        _Tex2 ("Texture2", 2D) = "white" {}
        _TexEmissive0 ("Emisive", 2D) = "white" {}
        _TexNorm ("Normals", 2D) = "white" {}
        
        _Color0 ("Color0", Color) = (1,1,1,1)
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        [HDR] _ColorEmissive0 ("EmissiveColor", Color) = (1,1,1,1)
        
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _NormalHeight ("NormalScalar", Range(0,10)) = 1.0
    }
    
     
        
    SubShader
    {
            
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "Lighting.cginc"
                #include "AutoLight.cginc"

                struct VertexInput {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 normal : NORMAL;
                    float3 tangent : TANGENT;

                };

                struct VertexOutput {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float2 uv1 : TEXCOORD1;
                    float4 normals : NORMAL;

                    float3 tangentSpaceLight: TANGENT;
                   
                };

                sampler2D _Tex0;
                sampler2D _Tex1;
                sampler2D _Tex2;
                sampler2D _TexEmissive0;
                sampler2D _TexNorm;
                
                half _Glossiness;
                half _Metallic;
                half _NormalHeight;
                fixed4 _Color0;
                fixed4 _Color1;
                fixed4 _Color2;
                fixed4 _ColorEmissive0;

                VertexOutput vert(VertexInput v) {

                    VertexOutput o;
                    o.normals = v.normal;
                    o.uv1 = v.uv;
                    o.vertex = UnityObjectToClipPos( v.vertex );
                   // o.uv = TRANSFORM_TEX( v.uv, _MainTex ); // used for texture

                    return o;
                }



                float4 frag(VertexOutput i) : COLOR{

                    float4 col2 = tex2D(_Tex0, i.uv1) * _Color0 + tex2D(_Tex1, i.uv1) * _Color1 + tex2D(_Tex2, i.uv1) * _Color2 + tex2D(_TexEmissive0, i.uv1) * _ColorEmissive0; 
                    return col2 * i.normals * 5;
                }
                ENDCG
            }
        }
    FallBack "Diffuse"
}
