Shader "GGJ14/TileTransition" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DestinyTex ("Destino (RGB)", 2D) = "white" {}
			}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _DestinyTex;
		
		float _PastTime = 0;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			if(_PastTime != 0)
			{
				half4 d = tex2D (_DestinyTex, IN.uv_MainTex);
				float dis = distance(IN.uv_MainTex, float2(0.5, 0.35));
				dis = min(1, max(0, dis )); 
				c = lerp(c, d, _PastTime - dis);
			} 
			o.Albedo = c;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
