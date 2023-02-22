Shader "CustomSkybox"
{
	Properties
	{
		_Seed("Seed", Float) = 1
		_NumStars("NumStars", Range( -0.9 , -0.6)) = -0.7
		_StarSize("Star Size", Range( 0 , 200)) = 0
		_StarColor1("StarColor1", Color) = (1,0.7294118,0.7294118,1)
		_StarColor2("StarColor2", Color) = (0.7411765,0.764706,1,1)
		_DustAmount("DustAmount", Range( 0 , 1)) = 0.5
		_Nebular1Strength("Nebular 1 Strength", Range( 0 , 1)) = 0.7411765
		_Nebular1Stretch("Nebular 1 Stetch", Range( 0 , 20)) = 1
		_Nebula1Waves("Nebular 1 Waves", Range( 0 , 20)) = 0.1
		_Nebular1ColorMain("Nebular1ColorMain", Color) = (0.245283,0.08214667,0.08214667,0)
		_Nebular1ColorMid("Nebular1ColorMid", Color) = (0.6839622,0.7408348,1,0)
		_Nebular2Strength("Nebular2Strength", Range( 0 , 1)) = 0
		_Nebular2Color1("Nebular2Color1", Color) = (0.08884287,1,0,0)
		_Nebular2Color2("Nebular2Color2", Color) = (0.928674,1,0,0)
		_SunSize("Sun Size", Range( 0 , 1)) = 0
		_SunShineStrength("Sun Shine Strength", Range(1, 10)) = 0
	}
	
	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Background" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#include "AutoLight.cginc"
			#include "UnityStandardBRDF.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			uniform float _Seed;
			uniform float _DustAmount;
			uniform float _NumStars;
			uniform float _StarSize;
			uniform float4 _StarColor1;
			uniform float4 _StarColor2;
			uniform float _Nebular1Strength;
			uniform float _Nebula1Waves;
			uniform float _Nebular1Stretch;
			uniform float4 _Nebular1ColorMain;
			uniform float4 _Nebular1ColorMid;
			uniform float4 _Nebular2Color1;
			uniform float4 _Nebular2Color2;
			uniform float _Nebular2Strength;
			uniform float _SunSize;
			uniform float _SunShineStrength;

			//289 seems like an arb num...
			float4 mod3D289( float4 x ) { return x - floor( x / 800.0 ) * 800.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289(float4(i,0));
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				
				o.ase_texcoord = v.vertex;
				o.ase_texcoord2.xyz = v.ase_texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				float3 Map = float3(_Seed ,0,0) + i.ase_texcoord.xyz;
				
				float perlinDustA = snoise( ( Map * 1.0) );
				float perlinDustB = snoise( ( Map * 2.0) );
				
				float starMap = snoise( ( Map * _StarSize ) );
				float smoothstepResult25 = smoothstep( _NumStars , -1.0 , starMap);
				
				float starPerlin = snoise( ( Map * 2.0 ).xyz );
				float4 starColor = lerp( _StarColor1 , _StarColor2 , (starPerlin*0.5 + 0.5));
				
				float Neb1Noise = abs(snoise( Map * _Nebula1Waves ) );
				float Neb1Brightness = _Nebular1Strength*0.0001;
				float simplePerlin3D105 = snoise( ( Map * 2.0 ) );
				float lerpResult231 = lerp( 0.5 , 1.0 , _Nebular1Strength);
				float simplePerlin3D188 = snoise( ( Map * 5.0 ) );
				float simplePerlin3D192 = snoise( ( Map * 10.0 ) );
				float simplePerlin3D204 = snoise( ( Map * 150.0 ) );
				float temp_output_201_0 = ( (simplePerlin3D105*0.5 + lerpResult231) + simplePerlin3D188 + ( simplePerlin3D192 * 0.5 ) + ( simplePerlin3D204 * 0.05 ) );
				float lerpResult232 = lerp( 30.0 , 15.0 , _Nebular1Strength);
				float clampResult218 = clamp( ( pow( ( 1.0 - Neb1Noise ) , lerpResult232 ) * temp_output_201_0 ) , 0.0 , 1.0 );
				float4 lerpResult215 = lerp( ( pow( ( 1.0 - ( Neb1Noise - Neb1Brightness ) ) , _Nebular1Stretch ) * temp_output_201_0 * _Nebular1ColorMain ) , _Nebular1ColorMid , clampResult218);
				float clampResult251 = clamp( ( _Nebular1Strength * 10.0 ) , 0.0 , 1.0 );
				float4 lerpResult248 = lerp( float4(0,0,0,0) , lerpResult215 , clampResult251);
				float4 color259 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
				
				float perlinDustC = snoise(  Map * 4 );
				float perlinDustD = snoise(  Map * 8 ) ;
				float4 lerpResult258 = lerp( color259 , ( ( (perlinDustC*1.0 + 0.5) * _Nebular2Color1 * 0.2 ) + ( (perlinDustD*1.0 + 0.5) * _Nebular2Color2 * 0.2 ) ) , _Nebular2Strength);

				#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
				float4 ase_lightColor = 0;
				#else //aselc
				float4 ase_lightColor = _LightColor0;
				#endif //aselc
				
				float3 ase_worldPos = i.ase_texcoord1.xyz;
				float3 worldSpaceLightDir = Unity_SafeNormalize(UnityWorldSpaceLightDir(ase_worldPos));
				float3 ase_sunPos = i.ase_texcoord2.xyz; // I think it's un pos?
				float sun = (1 - length( worldSpaceLightDir - ase_sunPos));
				float InvSunSize = 1 - _SunSize;
				float sunBlur = smoothstep( (1 - _SunSize*_SunShineStrength-0.01) , InvSunSize , sun);
				float sunColor = smoothstep( InvSunSize , 1.5 , sun);
				float4 lerpResult301 = lerp( ( perlinDustA + perlinDustB ) * _DustAmount * 0.1 + smoothstepResult25 * starColor + lerpResult248 + lerpResult258  , ase_lightColor ,  sunBlur + sunColor );
				float sunGlow = smoothstep(  (1 - _SunSize*_SunShineStrength*2) , InvSunSize , sun);
				
				return lerpResult301 + sunGlow * InvSunSize * ( ase_lightColor / 3.0 );
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}