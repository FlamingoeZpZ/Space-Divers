Shader "Custom/FusionShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
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
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0 // Unlucky, this will also probably prevent it from working on mobile, but I need to to get the proper look. I keep hearing URP is just the way to go.

        sampler2D _MainTex;
        sampler2D _Tex0;
        sampler2D _Tex1;
        sampler2D _Tex2;
        sampler2D _TexEmissive0;
        sampler2D _TexEmissive1;
        
        sampler2D _TexNorm;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Tex0;
            float2 uv_Tex1;
            float2 uv_Tex2;
            float2 uv_TexNorm;
            float2 uv_TexEmissive0;
            float2 uv_TexEmissive1;
        };

        half _Glossiness;
        half _Metallic;
        half _NormalHeight;
        
        fixed4 _Color0;
        fixed4 _Color1;
        fixed4 _Color2;
        fixed4 _ColorEmissive0;
        fixed4 _ColorEmissive1;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) + tex2D(_Tex0, IN.uv_Tex0)* _Color0 + tex2D(_Tex1, IN.uv_Tex1) * _Color1 + tex2D(_Tex2, IN.uv_Tex2) * _Color2;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Emission = tex2D(_TexEmissive0, IN.uv_TexEmissive0) * _ColorEmissive0;
            o.Normal = UnpackNormal(tex2D(_TexNorm, IN.uv_TexNorm)) * float3(_NormalHeight, _NormalHeight, 1);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
