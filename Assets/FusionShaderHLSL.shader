Shader "Custom/FusionShaderHLSL"
{
    Properties
    {
        _Tex0 ("Texture0", 2D) = "white" {}
        _Tex1 ("Texture1", 2D) = "white" {}
        _Tex2 ("Texture2", 2D) = "white" {}
        _TexEmissive0 ("Emisive", 2D) = "white" {}
        _TexNorm ("Normals", 2D) = "white" {}
        
        _ColorA ("ColorA", Color) = (1,1,1,1)
        _ColorB ("ColorB", Color) = (1,1,1,1)
        _ColorC ("ColorC", Color) = (1,1,1,1)
        _EmissiveColor ("EmissiveColor", Color) = (1,1,1,1)
        _Intensity("Intensity", Range(-10,10)) = 0
        
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
                Stencil {
                  Ref 1
                  Comp Equal
                }
                
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

                };

                struct VertexOutput {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float4 normals : NORMAL;
                   
                };

                sampler2D _Tex0;
                float4 _Tex0_ST;
                sampler2D _Tex1;
                sampler2D _Tex2;
                sampler2D _TexEmissive0;
                sampler2D _TexNorm;
                
                half _Glossiness;
                half _Metallic;
                half _NormalHeight;
                fixed4 _ColorA;
                fixed4 _ColorB;
                fixed4 _ColorC;
                fixed4 _EmissiveColor;
                half _Intensity;

                

                VertexOutput vert(VertexInput v) {

                    VertexOutput o;
                    o.normals = v.normal;
                    o.vertex = UnityObjectToClipPos( v.vertex );
                    o.uv = v.uv;
                     //.uv = TRANSFORM_TEX( v.uv, _Tex0 ); // used for texture

                    return o;
                }

                float4 frag(VertexOutput i) : COLOR{

                    float4 col2 = tex2D(_Tex0, i.uv) * _ColorA + tex2D(_Tex1, i.uv) * _ColorB + tex2D(_Tex2, i.uv) * _ColorC + tex2D(_TexEmissive0, i.uv) * _EmissiveColor; 

                    col2 *= pow(2.2, _Intensity);
                    return col2;
                }
                ENDCG
            }
        }
    FallBack "Diffuse"
}
