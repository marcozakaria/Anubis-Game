Shader "Custom/DoubleSided" {
     Properties
     {
         _Color ("Main Color", Color) = (1,1,1,1)
         _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
         _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		 _Contrast ("Contrast",Range(0,10)) = 1
		 _Brig ("Brightness",Range(0,10)) = 1
     }
 
     SubShader
     {
         Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
         LOD 300
         		 cull off
         CGPROGRAM
         #pragma surface surf Lambert alphatest:_Cutoff


         sampler2D _MainTex;
         sampler2D _AlphaTex;
         fixed4 _Color;
		 float _Contrast;
		 float _Brig;


         struct Input
         {
             float2 uv_MainTex;
             float2 uv_AlphaTex;
         };
 
         void surf (Input IN, inout SurfaceOutput o)
         {
             fixed4 MAIN = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			 MAIN.rgb = (MAIN.rgb - 0.5f) * (_Contrast) + 0.5f;
             o.Albedo = MAIN.rgb*_Brig;
             o.Alpha = MAIN.a;
         }
         ENDCG
     }
 
     FallBack "Transparent/Cutout/Diffuse"
 }