Shader "Hidden/GlitchShader" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_DispTex("Base (RGB)", 2D) = "bump" {}
	_Intensity("Glitch Intensity", Range(0.1, 1.0)) = 1
		_Chroma("Chroma Intensity", Range(0, 2)) = 1
	}

		SubShader{
		Pass{
		ZTest Always Cull Off ZWrite Off
		Fog{ Mode off }

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 

#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
	uniform sampler2D _DispTex;
	float _Intensity;
	float _Chroma;

	float filterRadius;
	float flip_up, flip_down;
	float displace;
	float scale;

	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;

		return o;
	}

	half4 frag(v2f i) : COLOR
	{

		half4 normal = tex2D(_DispTex, i.uv.xy * scale);

		if (i.uv.y < flip_up)
			i.uv.y = 1 - (i.uv.y + flip_up);

		if (i.uv.y > flip_down)
			i.uv.y = 1 - (i.uv.y - flip_down);

		i.uv.xy += (_Chroma > 1) ? (normal.xy - 0.01) * displace * _Intensity
			: -(normal.xy - 0.01) * displace * _Intensity;



		half4 color = tex2D(_MainTex,  i.uv.xy);
		half4 chroma = tex2D(_MainTex, _Chroma > 1 ? i.uv.xy - 0.0001 * filterRadius * _Intensity
			: i.uv.xy + 0.0001 * filterRadius * _Intensity);

		if (_Intensity != 0)
		{
			if (filterRadius > 0) {
				color.r = chroma.r * _Chroma;
				color.b = chroma.b * _Chroma;
			}
			else {
				color.g = chroma.b * _Chroma;
				color.b = chroma.g * _Chroma;
			}
		}


		return color;
	}
		ENDCG
	}
	}

		Fallback off

}