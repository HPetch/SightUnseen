// Made with Amplify Shader Editor v1.9.3.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "INab Studio/Post Processing Scan FX"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		[HideInInspector][NoScaleOffset]_MainTex("_MainTex", 2D) = "white" {}
		_Origin("Origin", Vector) = (0,0,0,0)
		_Forward("Forward", Vector) = (0,0,0,0)
		_CurrentScanValue("_CurrentScanValue", Range( 0 , 2)) = 1
		_Size("Size", Float) = 0
		_SizeAdjust("SizeAdjust", Float) = 1
		[HDR]_EdgeColor("Edge Color", Color) = (0.8490566,0.4626606,0.1321645,1)
		_EdgeHardness("Edge Hardness", Range( 0 , 1)) = 1
		_OriginOffset("_OriginOffset", Float) = 0
		[Header(Edge)]_EdgePower("Edge Power", Float) = 10
		_EdgeMultiplier("Edge Multiplier", Float) = 1
		[HDR]_HighlightColor("Highlight Color", Color) = (0.1333333,0.5768225,0.8509804,1)
		_HighlightPower("Highlight Power", Float) = 10
		_HighlightMultiplier("Highlight Multiplier", Float) = 0
		[Header(Shere Mask)]_MaskRadius("Mask Radius", Float) = 5
		_MaskHardness("Mask Hardness", Range( 0 , 1)) = 1
		_MaskPower("Mask Power", Float) = 1
		_FovMask("Fov Mask", Range( 0 , 1)) = 1
		_FovMaskSmoothness("Fov Mask Smoothness", Range( 0 , 1)) = 0
		_Frequency("Frequency", Float) = 0
		_Ratio("Ratio", Float) = 0
		[HDR]_OverlayColor("Overlay Color", Color) = (0.8490566,0.4626606,0.1321645,1)
		_OverlayMultiplier("Overlay Multiplier", Range( 0 , 1)) = 1
		_OverlayPower("Overlay Power", Float) = 1
		_NormalsOffset("NormalsOffset", Float) = 0
		_NormalsPower("NormalsPower", Float) = 0
		_Thickness("Thickness", Float) = 0
		_NormalsHardness("NormalsHardness", Float) = 0
		_DepthPower("DepthPower", Float) = 0
		_DepthThreshold("DepthThreshold", Float) = 0
		_DepthHardness("DepthHardness", Float) = 0
		[NoScaleOffset]_ScreenTexture("Screen Texture", 2D) = "white" {}
		_ScreenTextureTiling("Screen Texture Tiling", Float) = 0
		_EdgeDetectionMultiplier("EdgeDetectionMultiplier", Float) = 0
		_GridMultiplier("GridMultiplier", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#pragma shader_feature_local _OVERLAY_NONE _OVERLAY_SCREENTEXTURE _OVERLAY_EDGEDETECTION _OVERLAY_GRID _OVERLAY_GRIDANDEDGEDETECTION
			#pragma shader_feature _FOVMASKENABLED


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				float4 ase_texcoord4 : TEXCOORD4;
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float4 _HighlightColor;
			uniform float4x4 _InverseView;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float3 _Origin;
			uniform float _CurrentScanValue;
			uniform float _SizeAdjust;
			uniform float _MaskRadius;
			uniform float _Size;
			uniform float _HighlightPower;
			uniform float _HighlightMultiplier;
			uniform float4 _OverlayColor;
			uniform float _OverlayPower;
			uniform float _OverlayMultiplier;
			uniform sampler2D _ScreenTexture;
			uniform float _ScreenTextureTiling;
			uniform sampler2D _CameraDepthNormalsTexture;
			uniform float _Thickness;
			uniform float _NormalsOffset;
			uniform float _NormalsPower;
			uniform float _NormalsHardness;
			uniform float _DepthThreshold;
			uniform float _DepthPower;
			uniform float _DepthHardness;
			uniform float _Frequency;
			uniform float _Ratio;
			uniform float _GridMultiplier;
			uniform float _EdgeDetectionMultiplier;
			uniform float4 _EdgeColor;
			uniform float _EdgePower;
			uniform float _EdgeMultiplier;
			uniform float _OriginOffset;
			uniform float _EdgeHardness;
			uniform float _MaskHardness;
			uniform float _MaskPower;
			uniform float _FovMask;
			uniform float _FovMaskSmoothness;
			uniform float3 _Forward;


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 uv_MainTex10 = i.uv.xy;
				float4 MainTex19 = tex2D( _MainTex, uv_MainTex10 );
				float4 screenPos = i.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float4 break423 = (ase_screenPosNorm*2.0 + -1.0);
				float4 appendResult424 = (float4(break423.x , break423.y , 1.0 , 1.0));
				float eyeDepth419 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float4 break428 = ( mul( unity_CameraInvProjection, appendResult424 ) * eyeDepth419 );
				float4 appendResult427 = (float4(break428.x , break428.y , break428.z , 1.0));
				float4 WorldPositionFromDepth420 = mul( _InverseView, appendResult427 );
				float3 OriginVector227 = _Origin;
				float SphereSDF214 = distance( WorldPositionFromDepth420 , float4( OriginVector227 , 0.0 ) );
				float temp_output_347_0 = ( _SizeAdjust + 1.0 );
				float mask_radius279 = _MaskRadius;
				float temp_output_270_0 = ( SphereSDF214 - ( fmod( ( _CurrentScanValue * temp_output_347_0 ) , temp_output_347_0 ) * mask_radius279 ) );
				float temp_output_269_0 = saturate( (0.0 + (temp_output_270_0 - ( _Size * -1.0 )) * (1.0 - 0.0) / (1.0 - ( _Size * -1.0 ))) );
				float Highlight_Value_Out446 = saturate( ( pow( temp_output_269_0 , _HighlightPower ) * _HighlightMultiplier ) );
				float4 lerpResult451 = lerp( MainTex19 , _HighlightColor , Highlight_Value_Out446);
				float4 break603 = ase_screenPosNorm;
				float2 appendResult608 = (float2(( break603.x * ( _ScreenParams.x / _ScreenParams.y ) ) , break603.y));
				float2 appendResult520 = (float2(( 1.0 / _ScreenParams.x ) , ( 1.0 / _ScreenParams.y )));
				float2 temp_output_521_0 = ( appendResult520 * _Thickness );
				float4 temp_output_532_0 = ( float4( ( float2( 1,1 ) * temp_output_521_0 ), 0.0 , 0.0 ) + ase_screenPosNorm );
				float depthDecodedVal504 = 0;
				float3 normalDecodedVal504 = float3(0,0,0);
				DecodeDepthNormal( tex2D( _CameraDepthNormalsTexture, temp_output_532_0.xy ), depthDecodedVal504, normalDecodedVal504 );
				float4 temp_output_557_0 = ( ase_screenPosNorm + float4( ( float2( -1,-1 ) * temp_output_521_0 ), 0.0 , 0.0 ) );
				float depthDecodedVal561 = 0;
				float3 normalDecodedVal561 = float3(0,0,0);
				DecodeDepthNormal( tex2D( _CameraDepthNormalsTexture, temp_output_557_0.xy ), depthDecodedVal561, normalDecodedVal561 );
				float3 temp_output_507_0 = ( normalDecodedVal504 - normalDecodedVal561 );
				float dotResult509 = dot( temp_output_507_0 , temp_output_507_0 );
				float4 temp_output_534_0 = ( float4( ( float2( -1,1 ) * temp_output_521_0 ), 0.0 , 0.0 ) + ase_screenPosNorm );
				float depthDecodedVal506 = 0;
				float3 normalDecodedVal506 = float3(0,0,0);
				DecodeDepthNormal( tex2D( _CameraDepthNormalsTexture, temp_output_534_0.xy ), depthDecodedVal506, normalDecodedVal506 );
				float4 temp_output_533_0 = ( float4( ( float2( 1,-1 ) * temp_output_521_0 ), 0.0 , 0.0 ) + ase_screenPosNorm );
				float depthDecodedVal505 = 0;
				float3 normalDecodedVal505 = float3(0,0,0);
				DecodeDepthNormal( tex2D( _CameraDepthNormalsTexture, temp_output_533_0.xy ), depthDecodedVal505, normalDecodedVal505 );
				float3 temp_output_508_0 = ( normalDecodedVal506 - normalDecodedVal505 );
				float dotResult510 = dot( temp_output_508_0 , temp_output_508_0 );
				float smoothstepResult512 = smoothstep( ( dotResult509 + dotResult510 ) , 0.0 , _NormalsOffset);
				float temp_output_1_0_g7 = _NormalsHardness;
				float clampDepth554 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, temp_output_532_0.xy );
				float clampDepth553 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, temp_output_557_0.xy );
				float clampDepth552 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, temp_output_534_0.xy );
				float clampDepth551 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, temp_output_533_0.xy );
				float smoothstepResult539 = smoothstep( ( sqrt( ( pow( ( clampDepth554 - clampDepth553 ) , 2.0 ) + pow( ( clampDepth552 - clampDepth551 ) , 2.0 ) ) ) * 100.0 ) , 0.0 , ( clampDepth553 * _DepthThreshold ));
				float temp_output_1_0_g8 = _DepthHardness;
				float temp_output_567_0 = saturate( max( ( ( pow( smoothstepResult512 , _NormalsPower ) - temp_output_1_0_g7 ) / ( 1.0 - temp_output_1_0_g7 ) ) , ( ( pow( smoothstepResult539 , _DepthPower ) - temp_output_1_0_g8 ) / ( 1.0 - temp_output_1_0_g8 ) ) ) );
				float4 temp_output_382_0 = ( WorldPositionFromDepth420 * _Frequency );
				float4 temp_output_392_0 = ( max( abs( ddx( temp_output_382_0 ) ) , abs( ddy( temp_output_382_0 ) ) ) * 0.5 );
				float4 temp_output_395_0 = ( temp_output_382_0 + temp_output_392_0 );
				float4 temp_output_394_0 = ( temp_output_382_0 - temp_output_392_0 );
				float4 temp_output_641_0 = saturate( ( ( floor( temp_output_395_0 ) + min( ( frac( temp_output_395_0 ) * _Ratio ) , float4( 1,1,1,1 ) ) ) - ( floor( temp_output_394_0 ) - min( ( frac( temp_output_394_0 ) * _Ratio ) , float4( 1,1,1,1 ) ) ) ) );
				float4 break645 = ( 1.0 - temp_output_641_0 );
				float4 break399 = temp_output_641_0;
				float temp_output_646_0 = ( step( ( break645.x * break645.y * break645.z ) , 1E-08 ) - step( ( 1.0 - ( break399.x * break399.y * break399.z ) ) , 1E-08 ) );
				#if defined(_OVERLAY_NONE)
				float staticSwitch465 = 0.0;
				#elif defined(_OVERLAY_SCREENTEXTURE)
				float staticSwitch465 = tex2D( _ScreenTexture, ( appendResult608 * _ScreenTextureTiling ) ).r;
				#elif defined(_OVERLAY_EDGEDETECTION)
				float staticSwitch465 = temp_output_567_0;
				#elif defined(_OVERLAY_GRID)
				float staticSwitch465 = temp_output_646_0;
				#elif defined(_OVERLAY_GRIDANDEDGEDETECTION)
				float staticSwitch465 = saturate( ( ( temp_output_646_0 * _GridMultiplier ) + ( temp_output_567_0 * _EdgeDetectionMultiplier ) ) );
				#else
				float staticSwitch465 = 0.0;
				#endif
				float Overlay_Value_Out458 = saturate( ( pow( temp_output_269_0 , _OverlayPower ) * ( _OverlayMultiplier * staticSwitch465 ) ) );
				float4 lerpResult460 = lerp( lerpResult451 , _OverlayColor , Overlay_Value_Out458);
				float Edge_Value_Out445 = saturate( ( pow( temp_output_269_0 , _EdgePower ) * _EdgeMultiplier ) );
				float4 lerpResult449 = lerp( lerpResult460 , _EdgeColor , Edge_Value_Out445);
				float4 Out_Color88 = lerpResult449;
				float clampResult378 = clamp( _EdgeHardness , 0.0 , 0.9999 );
				float smoothstepResult376 = smoothstep( 1.0 , clampResult378 , temp_output_270_0);
				float temp_output_141_0 = ( _MaskRadius + 1.0 );
				float lerpResult137 = lerp( 0.0 , ( temp_output_141_0 - 0.001 ) , _MaskHardness);
				float smoothstepResult134 = smoothstep( temp_output_141_0 , lerpResult137 , SphereSDF214);
				float SphereMask107 = pow( smoothstepResult134 , _MaskPower );
				float temp_output_208_0 = ( 1.0 - _FovMask );
				float2 appendResult193 = (float2(_Forward.x , _Forward.z));
				float4 break180 = WorldPositionFromDepth420;
				float2 appendResult182 = (float2(break180.x , break180.z));
				float2 appendResult183 = (float2(_Origin.x , _Origin.z));
				float2 normalizeResult170 = normalize( ( appendResult182 - appendResult183 ) );
				float dotResult171 = dot( appendResult193 , normalizeResult170 );
				float smoothstepResult200 = smoothstep( temp_output_208_0 , ( temp_output_208_0 + _FovMaskSmoothness ) , saturate( dotResult171 ));
				#ifdef _FOVMASKENABLED
				float staticSwitch622 = smoothstepResult200;
				#else
				float staticSwitch622 = 1.0;
				#endif
				float FovMask177 = staticSwitch622;
				float4 lerpResult364 = lerp( MainTex19 , Out_Color88 , ( ( ( saturate( ( ( _OriginOffset * -1.0 ) + SphereSDF214 ) ) * smoothstepResult376 ) * max( max( Edge_Value_Out445 , Highlight_Value_Out446 ) , Overlay_Value_Out458 ) ) * ( SphereMask107 * FovMask177 ) ));
				

				finalColor = lerpResult364;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19301
Node;AmplifyShaderEditor.CommentaryNode;416;-7131.516,501.7732;Inherit;False;2387.422;605.0495;;13;432;430;429;428;427;426;425;424;423;422;421;419;420;World Position From Depth;0.7490196,0,0,1;0;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;429;-7062.148,817.7674;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;422;-6823.066,827.9954;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;423;-6578.761,832.9412;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;424;-6409.761,831.9412;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CameraProjectionNode;430;-6558.645,663.6887;Inherit;False;unity_CameraInvProjection;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;425;-6238.346,789.2584;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenDepthNode;419;-6234.481,1015.915;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;426;-6006.482,800.4932;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;428;-5714.203,841.8953;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;427;-5512.25,822.5323;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Matrix4X4Node;432;-5593.233,600.8616;Inherit;False;Global;_InverseView;_InverseView;13;0;Create;True;0;0;0;False;0;False;0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;421;-5258.859,818.1014;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;420;-5006.502,844.5645;Inherit;False;WorldPositionFromDepth;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;414;-4270.675,583.2374;Inherit;False;3421.406;1053.29;;34;463;412;411;399;410;398;406;397;396;403;408;404;407;401;405;400;402;395;394;392;393;391;390;389;387;388;382;383;641;642;643;644;645;646;Grid Overlay;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;569;-5542.546,1836.21;Inherit;False;4607.79;1910.699;;60;498;500;518;519;520;521;527;525;526;528;556;529;530;531;557;532;533;534;551;552;553;554;560;501;502;503;535;536;546;504;505;506;548;549;561;507;508;537;509;510;538;545;511;540;555;514;512;515;539;544;513;516;541;543;517;550;563;559;562;567;Edge Detection Overlay;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;463;-4216.734,734.3468;Inherit;False;420;WorldPositionFromDepth;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;383;-4121.675,879.5788;Inherit;False;Property;_Frequency;Frequency;20;0;Create;True;1;Grid;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;382;-3909.675,739.5788;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenParams;498;-5186.546,2808.276;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DdyOpNode;388;-3668.82,986.2193;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DdxOpNode;387;-3661.877,881.4293;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;518;-4954.545,2794.276;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;519;-4940.188,2901.346;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;389;-3517.82,867.2193;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;390;-3523.82,968.2193;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;520;-4744.925,2872.926;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;500;-4742.486,3074.779;Inherit;False;Property;_Thickness;Thickness;27;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;391;-3328.82,925.2193;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;393;-3394.82,1105.219;Inherit;False;Constant;_Float0;Float 0;20;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;521;-4527.925,2960.926;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;527;-4315.477,2142.104;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;0;False;0;False;-1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;525;-4317.506,2263.929;Inherit;False;Constant;_Vector1;Vector 0;3;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;526;-4322.506,2380.929;Inherit;False;Constant;_Vector2;Vector 0;3;0;Create;True;0;0;0;False;0;False;1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;528;-4329.506,2512.929;Inherit;False;Constant;_Vector3;Vector 0;3;0;Create;True;0;0;0;False;0;False;-1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;392;-3199.82,1027.219;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;556;-4077.879,2171.969;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;563;-4215.557,1886.21;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;529;-4069.267,2591.556;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;530;-4073.097,2450.466;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;531;-4060.879,2327.969;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;395;-2973.729,633.2372;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;394;-2985.877,1090.889;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;557;-3846.243,2161.195;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;532;-3808.243,2320.195;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;533;-3811.243,2494.195;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;534;-3812.243,2672.195;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;402;-2851.183,958.0623;Inherit;False;Property;_Ratio;Ratio;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;400;-2769.183,790.0623;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FractNode;405;-2792.599,1406.122;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenDepthNode;551;-3008.982,3458.07;Inherit;False;1;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;552;-3021.955,3619.853;Inherit;False;1;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;553;-3010.432,3217.651;Inherit;False;1;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;554;-3016.219,3347.87;Inherit;False;1;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;559;-3846.818,1896.606;Inherit;True;Global;_CameraDepthNormalsTexture;_CameraDepthNormalsTexture ;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;401;-2659.183,842.0623;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;407;-2654.843,1480.895;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;560;-3488.938,2156.727;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;501;-3459.327,2378.314;Inherit;True;Property;_TextureSample1;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;502;-3452.505,2578.438;Inherit;True;Property;_TextureSample2;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;503;-3452.505,2780.837;Inherit;True;Property;_TextureSample3;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;535;-2732.595,3266.145;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;536;-2734.24,3414.258;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;546;-2720.422,3633.909;Inherit;False;Constant;_Float1;Float 0;13;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;403;-2508.183,938.0623;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FloorOpNode;396;-2767.126,638.7978;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMinOpNode;408;-2445.842,1514.895;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FloorOpNode;404;-2645.284,1252.15;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DecodeDepthNormalNode;561;-3102.636,2166.356;Inherit;False;1;0;FLOAT4;0,0,0,0;False;2;FLOAT;0;FLOAT3;1
Node;AmplifyShaderEditor.DecodeDepthNormalNode;504;-3081.072,2350.284;Inherit;False;1;0;FLOAT4;0,0,0,0;False;2;FLOAT;0;FLOAT3;1
Node;AmplifyShaderEditor.DecodeDepthNormalNode;505;-3085.787,2603.391;Inherit;False;1;0;FLOAT4;0,0,0,0;False;2;FLOAT;0;FLOAT3;1
Node;AmplifyShaderEditor.DecodeDepthNormalNode;506;-3083.246,2798.051;Inherit;False;1;0;FLOAT4;0,0,0,0;False;2;FLOAT;0;FLOAT3;1
Node;AmplifyShaderEditor.PowerNode;548;-2554.339,3419.951;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;549;-2555.245,3276.234;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;397;-2243.182,757.0623;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;406;-2292.842,1279.895;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;507;-2740.503,2295.655;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;508;-2763.176,2684.507;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;537;-2396.139,3333.323;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;216;-2771.562,-2908.239;Inherit;False;2702.303;917.7312;;20;227;171;170;193;168;192;183;182;180;56;177;200;203;206;207;208;204;437;622;623;FOV Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;398;-2102.831,1037.285;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DotProductOpNode;509;-2531.344,2351.116;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;510;-2557.434,2667.759;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SqrtOpNode;538;-2260.14,3359.323;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;545;-2268.453,3218.566;Inherit;False;Property;_DepthThreshold;DepthThreshold;30;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;56;-2495.179,-2312.169;Inherit;False;Property;_Origin;Origin;2;0;Create;True;1;Fov Mask;0;0;False;0;False;0,0,0;-2,0,1.75;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;296;-4126.805,-827.6511;Inherit;False;3504.344;767.662;;21;268;267;269;275;270;278;221;273;346;280;347;276;358;359;360;377;376;378;438;440;696;Scan Logic;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;641;-1974.392,1299.318;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;511;-2371.243,2533.75;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;540;-1974.088,3141.84;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;555;-2074.635,3390.115;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;514;-2338.72,2695.802;Inherit;False;Property;_NormalsOffset;NormalsOffset;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;215;-2906.797,-3919.217;Inherit;False;2067.888;523.7026;;4;214;212;228;436;Sphere SDF;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;227;-2208.266,-2143.202;Inherit;False;OriginVector;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;226;-1655.047,-1827.486;Inherit;False;1605.563;528.4161;;11;141;105;137;219;134;147;146;140;107;102;279;Sphere Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;643;-1824.659,1014.108;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;399;-1721.431,1201.069;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SmoothstepOpNode;512;-2113.758,2566.211;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;515;-2043.946,2746.09;Inherit;False;Property;_NormalsPower;NormalsPower;26;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;539;-1717.276,3282.616;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;544;-1709.832,3460.607;Inherit;False;Property;_DepthPower;DepthPower;29;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;358;-3799.411,-414.9531;Inherit;False;Property;_SizeAdjust;SizeAdjust;6;0;Create;True;0;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;228;-2557.415,-3737.68;Inherit;False;227;OriginVector;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;436;-2668.964,-3842.8;Inherit;False;420;WorldPositionFromDepth;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;618;-2888.225,4128.528;Inherit;False;1606.026;582.6699;;9;570;605;606;603;607;608;610;609;573;Screen Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;347;-3496.938,-386.3092;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-1605.047,-1665.715;Inherit;False;Property;_MaskRadius;Mask Radius;15;1;[Header];Create;True;1;Shere Mask;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;645;-1634.329,973.1914;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;411;-1537.233,1189.104;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;513;-1818.833,2630.901;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;516;-1804.946,2482.09;Inherit;False;Property;_NormalsHardness;NormalsHardness;28;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;541;-1493.163,3328.799;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;543;-1517.267,3190.492;Inherit;False;Property;_DepthHardness;DepthHardness;31;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;359;-3866.411,-655.9528;Inherit;False;Property;_CurrentScanValue;_CurrentScanValue;4;0;Create;True;0;0;0;False;0;False;1;0.2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;212;-2293.814,-3811;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;279;-1459.277,-1525.36;Inherit;False;mask radius;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;360;-3405.411,-595.9528;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;570;-2838.225,4178.528;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenParams;605;-2648.707,4502.198;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;644;-1430.99,992.0244;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;410;-1345.163,1236.865;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;517;-1537.18,2621.933;Inherit;False;Inverse Lerp;-1;;7;09cbe79402f023141a4dc1fddd4c9511;0;3;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;550;-1314.61,3230.596;Inherit;False;Inverse Lerp;-1;;8;09cbe79402f023141a4dc1fddd4c9511;0;3;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;214;-2064.747,-3834.745;Inherit;False;SphereSDF;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;280;-3207.186,-407.0245;Inherit;False;279;mask radius;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;606;-2450.305,4541.62;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;603;-2584.707,4266.198;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.StepOpNode;412;-1191.457,1289.349;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1E-08;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;642;-1265.361,1067.074;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1E-08;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;562;-1269.339,2772.337;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FmodOpNode;346;-3148.33,-557.8967;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;273;-2950.149,-473.6015;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;607;-2307.305,4466.62;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;646;-1054.873,1219.755;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;567;-1112.757,2821.96;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;692;-992.0721,4304.542;Inherit;False;Property;_EdgeDetectionMultiplier;EdgeDetectionMultiplier;34;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;695;-977.0898,4089.864;Inherit;False;Property;_GridMultiplier;GridMultiplier;35;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;276;-2373.314,-486.9872;Inherit;False;Property;_Size;Size;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;221;-2723.12,-782.6511;Inherit;False;214;SphereSDF;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;437;-2671.036,-2523.215;Inherit;False;420;WorldPositionFromDepth;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;278;-2063.447,-477.6671;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;270;-2416.265,-737.2488;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;608;-2124.305,4372.62;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;691;-722.0646,4035.974;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;690;-712.7158,4238.23;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;610;-2058.305,4576.62;Inherit;False;Property;_ScreenTextureTiling;Screen Texture Tiling;33;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;180;-2358.884,-2501.498;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TFHCRemapNode;275;-1851.448,-532.667;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;609;-1815.305,4412.62;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;693;-563.0212,4154.544;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;182;-2165.658,-2529.194;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;183;-2161.658,-2351.194;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;269;-1360.866,-506.5537;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;362;-1051.921,59.99027;Inherit;False;Property;_HighlightPower;Highlight Power;13;0;Create;True;0;0;0;False;0;False;10;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;647;-769.209,1601.672;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;624;-754.4734,3904.161;Inherit;False;Constant;_Float3;Float 3;38;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;694;-433.0212,4206.544;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;573;-1594.199,4246.731;Inherit;True;Property;_ScreenTexture;Screen Texture;32;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;192;-2084.89,-2843.316;Inherit;False;Property;_Forward;Forward;3;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;168;-1916.943,-2440.049;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;442;-874.058,112.8492;Inherit;False;Property;_HighlightMultiplier;Highlight Multiplier;14;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;363;-855.1043,-12.85057;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;455;-787.0769,787.2487;Inherit;False;Property;_OverlayMultiplier;Overlay Multiplier;23;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;626;-1195.013,364.6138;Inherit;False;Property;_OverlayPower;Overlay Power;24;0;Create;True;0;0;0;False;0;False;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;465;-203.5123,3906.043;Inherit;False;Property;_OVERLAY_;OVERLAY_;26;0;Create;True;0;0;0;False;0;False;0;0;0;False;;KeywordEnum;5;NONE;SCREENTEXTURE;EDGEDETECTION;GRID;GRIDANDEDGEDETECTION;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;217;-1307.345,-3314.229;Inherit;False;630.512;277.2678;;2;10;19;Main Tex;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;141;-1405.531,-1648.531;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;193;-1838.889,-2797.316;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;170;-1756.893,-2376.863;Inherit;False;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;204;-1160.034,-2312.368;Inherit;False;Property;_FovMask;Fov Mask;18;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;267;-1126.657,-397.6747;Inherit;False;Property;_EdgePower;Edge Power;10;1;[Header];Create;True;1;Edge;0;0;False;0;False;10;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;441;-667.541,32.34758;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;627;-939.0126,312.6139;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;459;-478.6124,923.2882;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-1356.348,-1412.07;Inherit;False;Property;_MaskHardness;Mask Hardness;16;0;Create;True;0;0;0;False;0;False;1;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;140;-1230.935,-1559.646;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.001;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;208;-779.8411,-2316.015;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;171;-1574.892,-2507.863;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;207;-1005.452,-2115.253;Inherit;False;Property;_FovMaskSmoothness;Fov Mask Smoothness;19;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;268;-851.6562,-448.6747;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;440;-960.9471,-301.7272;Inherit;False;Property;_EdgeMultiplier;Edge Multiplier;11;0;Create;True;0;0;0;False;0;False;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;628;-497.7538,46.93848;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;629;-456.3019,648.5834;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1257.345,-3263.961;Inherit;True;Property;_MainTex;_MainTex;0;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;218;27.46035,701.8582;Inherit;False;1741.686;572.5008;;11;449;18;447;88;448;450;451;456;460;461;462;Out Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;137;-1049.408,-1542.73;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;219;-1084.81,-1777.486;Inherit;False;214;SphereSDF;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;19;-918.8327,-3264.229;Inherit;False;MainTex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;206;-686.4506,-2168.253;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;203;-1387.811,-2500.678;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;446;-323.5444,127.6025;Inherit;False;Highlight Value Out;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;438;-734.947,-324.7272;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;630;-295.9753,691.5173;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;698;-1500.32,-1104.439;Inherit;False;Property;_OriginOffset;_OriginOffset;9;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;134;-793.446,-1666.707;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;147;-744.7891,-1486.473;Inherit;False;Property;_MaskPower;Mask Power;17;0;Create;True;0;0;0;False;0;False;1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;456;140.7108,780.1638;Inherit;False;19;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;448;85.55949,1073.894;Inherit;False;446;Highlight Value Out;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;200;-589.6371,-2395.51;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;450;57.38018,898.987;Inherit;False;Property;_HighlightColor;Highlight Color;12;1;[HDR];Create;True;0;0;0;False;0;False;0.1333333,0.5768225,0.8509804,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;623;-585.1735,-2541.218;Inherit;False;Constant;_Float2;Float 2;38;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;439;-564.9471,-273.7271;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;458;-166.7226,512.5139;Inherit;False;Overlay Value Out;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;377;-1547.234,-674.1022;Inherit;False;Property;_EdgeHardness;Edge Hardness;8;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;697;-1491.798,-937.5549;Inherit;False;214;SphereSDF;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;699;-1324.549,-1063.784;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;146;-514.5757,-1665.703;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;445;-258.582,-58.84865;Inherit;False;Edge Value Out;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;451;354.0811,818.6262;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;462;555.3679,1160.079;Inherit;False;458;Overlay Value Out;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;461;544.8993,952.3843;Inherit;False;Property;_OverlayColor;Overlay Color;22;1;[HDR];Create;True;0;0;0;False;0;False;0.8490566,0.4626606,0.1321645,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;622;-382.1735,-2538.218;Inherit;False;Property;_FovMaskEnabled;_FovMaskEnabled;38;0;Create;True;0;0;0;False;0;False;0;0;0;False;_FOVMASKENABLED;Toggle;2;Key0;Key1;Create;False;False;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;378;-1237.234,-690.1022;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.9999;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;700;-1155.549,-1016.784;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;460;833.8369,826.7728;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;447;1054.879,1195.287;Inherit;False;445;Edge Value Out;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;107;-291.4847,-1669.088;Inherit;False;SphereMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;1057.46,996.8582;Inherit;False;Property;_EdgeColor;Edge Color;7;1;[HDR];Create;True;0;0;0;False;0;False;0.8490566,0.4626606,0.1321645,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;177;-159.2576,-2435.617;Inherit;False;FovMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;384;46.74089,-17.12265;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;376;-1063.671,-768.1424;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;701;-1014.549,-976.7844;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;449;1194.398,825.2467;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;999.8899,141.8907;Inherit;False;107;SphereMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;230;1005.408,257.4283;Inherit;False;177;FovMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;631;163.0247,55.51727;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;696;-834.5938,-867.6053;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;229;1236.408,149.4284;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;266;241.3647,-93.41389;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;88;1455.145,821.6157;Inherit;False;Out Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;1388.469,-1.340594;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;385;1373.586,-230.5116;Inherit;False;88;Out Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;386;1377.571,-362.7059;Inherit;False;19;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;364;1672.339,-190.061;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;159;1829.805,-203.0762;Float;False;True;-1;2;ASEMaterialInspector;0;9;INab Studio/Post Processing Scan FX;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;True;7;False;;False;True;0;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;422;0;429;0
WireConnection;423;0;422;0
WireConnection;424;0;423;0
WireConnection;424;1;423;1
WireConnection;425;0;430;0
WireConnection;425;1;424;0
WireConnection;426;0;425;0
WireConnection;426;1;419;0
WireConnection;428;0;426;0
WireConnection;427;0;428;0
WireConnection;427;1;428;1
WireConnection;427;2;428;2
WireConnection;421;0;432;0
WireConnection;421;1;427;0
WireConnection;420;0;421;0
WireConnection;382;0;463;0
WireConnection;382;1;383;0
WireConnection;388;0;382;0
WireConnection;387;0;382;0
WireConnection;518;1;498;1
WireConnection;519;1;498;2
WireConnection;389;0;387;0
WireConnection;390;0;388;0
WireConnection;520;0;518;0
WireConnection;520;1;519;0
WireConnection;391;0;389;0
WireConnection;391;1;390;0
WireConnection;521;0;520;0
WireConnection;521;1;500;0
WireConnection;392;0;391;0
WireConnection;392;1;393;0
WireConnection;556;0;527;0
WireConnection;556;1;521;0
WireConnection;529;0;528;0
WireConnection;529;1;521;0
WireConnection;530;0;526;0
WireConnection;530;1;521;0
WireConnection;531;0;525;0
WireConnection;531;1;521;0
WireConnection;395;0;382;0
WireConnection;395;1;392;0
WireConnection;394;0;382;0
WireConnection;394;1;392;0
WireConnection;557;0;563;0
WireConnection;557;1;556;0
WireConnection;532;0;531;0
WireConnection;532;1;563;0
WireConnection;533;0;530;0
WireConnection;533;1;563;0
WireConnection;534;0;529;0
WireConnection;534;1;563;0
WireConnection;400;0;395;0
WireConnection;405;0;394;0
WireConnection;551;0;533;0
WireConnection;552;0;534;0
WireConnection;553;0;557;0
WireConnection;554;0;532;0
WireConnection;401;0;400;0
WireConnection;401;1;402;0
WireConnection;407;0;405;0
WireConnection;407;1;402;0
WireConnection;560;0;559;0
WireConnection;560;1;557;0
WireConnection;501;0;559;0
WireConnection;501;1;532;0
WireConnection;502;0;559;0
WireConnection;502;1;533;0
WireConnection;503;0;559;0
WireConnection;503;1;534;0
WireConnection;535;0;554;0
WireConnection;535;1;553;0
WireConnection;536;0;552;0
WireConnection;536;1;551;0
WireConnection;403;0;401;0
WireConnection;396;0;395;0
WireConnection;408;0;407;0
WireConnection;404;0;394;0
WireConnection;561;0;560;0
WireConnection;504;0;501;0
WireConnection;505;0;502;0
WireConnection;506;0;503;0
WireConnection;548;0;536;0
WireConnection;548;1;546;0
WireConnection;549;0;535;0
WireConnection;549;1;546;0
WireConnection;397;0;396;0
WireConnection;397;1;403;0
WireConnection;406;0;404;0
WireConnection;406;1;408;0
WireConnection;507;0;504;1
WireConnection;507;1;561;1
WireConnection;508;0;506;1
WireConnection;508;1;505;1
WireConnection;537;0;549;0
WireConnection;537;1;548;0
WireConnection;398;0;397;0
WireConnection;398;1;406;0
WireConnection;509;0;507;0
WireConnection;509;1;507;0
WireConnection;510;0;508;0
WireConnection;510;1;508;0
WireConnection;538;0;537;0
WireConnection;641;0;398;0
WireConnection;511;0;509;0
WireConnection;511;1;510;0
WireConnection;540;0;553;0
WireConnection;540;1;545;0
WireConnection;555;0;538;0
WireConnection;227;0;56;0
WireConnection;643;0;641;0
WireConnection;399;0;641;0
WireConnection;512;0;514;0
WireConnection;512;1;511;0
WireConnection;539;0;540;0
WireConnection;539;1;555;0
WireConnection;347;0;358;0
WireConnection;645;0;643;0
WireConnection;411;0;399;0
WireConnection;411;1;399;1
WireConnection;411;2;399;2
WireConnection;513;0;512;0
WireConnection;513;1;515;0
WireConnection;541;0;539;0
WireConnection;541;1;544;0
WireConnection;212;0;436;0
WireConnection;212;1;228;0
WireConnection;279;0;102;0
WireConnection;360;0;359;0
WireConnection;360;1;347;0
WireConnection;644;0;645;0
WireConnection;644;1;645;1
WireConnection;644;2;645;2
WireConnection;410;0;411;0
WireConnection;517;1;516;0
WireConnection;517;3;513;0
WireConnection;550;1;543;0
WireConnection;550;3;541;0
WireConnection;214;0;212;0
WireConnection;606;0;605;1
WireConnection;606;1;605;2
WireConnection;603;0;570;0
WireConnection;412;0;410;0
WireConnection;642;0;644;0
WireConnection;562;0;517;0
WireConnection;562;1;550;0
WireConnection;346;0;360;0
WireConnection;346;1;347;0
WireConnection;273;0;346;0
WireConnection;273;1;280;0
WireConnection;607;0;603;0
WireConnection;607;1;606;0
WireConnection;646;0;642;0
WireConnection;646;1;412;0
WireConnection;567;0;562;0
WireConnection;278;0;276;0
WireConnection;270;0;221;0
WireConnection;270;1;273;0
WireConnection;608;0;607;0
WireConnection;608;1;603;1
WireConnection;691;0;646;0
WireConnection;691;1;695;0
WireConnection;690;0;567;0
WireConnection;690;1;692;0
WireConnection;180;0;437;0
WireConnection;275;0;270;0
WireConnection;275;1;278;0
WireConnection;609;0;608;0
WireConnection;609;1;610;0
WireConnection;693;0;691;0
WireConnection;693;1;690;0
WireConnection;182;0;180;0
WireConnection;182;1;180;2
WireConnection;183;0;56;1
WireConnection;183;1;56;3
WireConnection;269;0;275;0
WireConnection;647;0;646;0
WireConnection;694;0;693;0
WireConnection;573;1;609;0
WireConnection;168;0;182;0
WireConnection;168;1;183;0
WireConnection;363;0;269;0
WireConnection;363;1;362;0
WireConnection;465;1;624;0
WireConnection;465;0;573;1
WireConnection;465;2;567;0
WireConnection;465;3;647;0
WireConnection;465;4;694;0
WireConnection;141;0;102;0
WireConnection;193;0;192;1
WireConnection;193;1;192;3
WireConnection;170;0;168;0
WireConnection;441;0;363;0
WireConnection;441;1;442;0
WireConnection;627;0;269;0
WireConnection;627;1;626;0
WireConnection;459;0;455;0
WireConnection;459;1;465;0
WireConnection;140;0;141;0
WireConnection;208;0;204;0
WireConnection;171;0;193;0
WireConnection;171;1;170;0
WireConnection;268;0;269;0
WireConnection;268;1;267;0
WireConnection;628;0;441;0
WireConnection;629;0;627;0
WireConnection;629;1;459;0
WireConnection;137;1;140;0
WireConnection;137;2;105;0
WireConnection;19;0;10;0
WireConnection;206;0;208;0
WireConnection;206;1;207;0
WireConnection;203;0;171;0
WireConnection;446;0;628;0
WireConnection;438;0;268;0
WireConnection;438;1;440;0
WireConnection;630;0;629;0
WireConnection;134;0;219;0
WireConnection;134;1;141;0
WireConnection;134;2;137;0
WireConnection;200;0;203;0
WireConnection;200;1;208;0
WireConnection;200;2;206;0
WireConnection;439;0;438;0
WireConnection;458;0;630;0
WireConnection;699;0;698;0
WireConnection;146;0;134;0
WireConnection;146;1;147;0
WireConnection;445;0;439;0
WireConnection;451;0;456;0
WireConnection;451;1;450;0
WireConnection;451;2;448;0
WireConnection;622;1;623;0
WireConnection;622;0;200;0
WireConnection;378;0;377;0
WireConnection;700;0;699;0
WireConnection;700;1;697;0
WireConnection;460;0;451;0
WireConnection;460;1;461;0
WireConnection;460;2;462;0
WireConnection;107;0;146;0
WireConnection;177;0;622;0
WireConnection;384;0;445;0
WireConnection;384;1;446;0
WireConnection;376;0;270;0
WireConnection;376;2;378;0
WireConnection;701;0;700;0
WireConnection;449;0;460;0
WireConnection;449;1;18;0
WireConnection;449;2;447;0
WireConnection;631;0;384;0
WireConnection;631;1;458;0
WireConnection;696;0;701;0
WireConnection;696;1;376;0
WireConnection;229;0;110;0
WireConnection;229;1;230;0
WireConnection;266;0;696;0
WireConnection;266;1;631;0
WireConnection;88;0;449;0
WireConnection;109;0;266;0
WireConnection;109;1;229;0
WireConnection;364;0;386;0
WireConnection;364;1;385;0
WireConnection;364;2;109;0
WireConnection;159;0;364;0
ASEEND*/
//CHKSM=3311947E0954F2133E05BD25250F38F4004663AA