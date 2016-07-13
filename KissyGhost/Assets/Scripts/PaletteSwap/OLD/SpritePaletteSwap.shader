Shader "Custom/SpritePaletteSwap" 
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_ColorTint("Tint", Color) = (1,1,1,1)
		_Color1in("Color 1 In", Color) = (1,1,1,1)
		_Color1out("Color 1 Out", Color) = (1,1,1,1)
		_Color2in("Color 2 In", Color) = (1,1,1,1)
		_Color2out("Color 2 Out", Color) = (1,1,1,1)
		_Color3in("Color 3 In", Color) = (1,1,1,1)
		_Color3out("Color 3 Out", Color) = (1,1,1,1)
		_Color4in("Color 4 In", Color) = (1,1,1,1)
		_Color4out("Color 4 Out", Color) = (1,1,1,1)
		_Color5in("Color 5 In", Color) = (1,1,1,1)
		_Color5out("Color 5 Out", Color) = (1,1,1,1)
		_Color6in("Color 6 In", Color) = (1,1,1,1)
		_Color6out("Color 6 Out", Color) = (1,1,1,1)
		_Color7in("Color 7 In", Color) = (1,1,1,1)
		_Color7out("Color 7 Out", Color) = (1,1,1,1)
		_Color8in("Color 8 In", Color) = (1,1,1,1)
		_Color8out("Color 8 Out", Color) = (1,1,1,1)
		_Color9in("Color 9 In", Color) = (1,1,1,1)
		_Color9out("Color 9 Out", Color) = (1,1,1,1)
		_Color10in("Color 10 In", Color) = (1,1,1,1)
		_Color10out("Color 10 Out", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag          
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			fixed4 _ColorTint;
			fixed4 _Color1in;
			fixed4 _Color1out;
			fixed4 _Color2in;
			fixed4 _Color2out;
			fixed4 _Color3in;
			fixed4 _Color3out;
			fixed4 _Color4in;
			fixed4 _Color4out;
			fixed4 _Color5in;
			fixed4 _Color5out;
			fixed4 _Color6in;
			fixed4 _Color6out;
			fixed4 _Color7in;
			fixed4 _Color7out;
			fixed4 _Color8in;
			fixed4 _Color8out;
			fixed4 _Color9in;
			fixed4 _Color9out;
			fixed4 _Color10in;
			fixed4 _Color10out;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _ColorTint;

#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : COLOR
			{
				float4 texColor = tex2D(_MainTex, IN.texcoord);
				texColor = (texColor == _Color1in) ? _Color1out : texColor;
				/*
				texColor = all(texColor == _Color2in) ? _Color2out : texColor;
				texColor = all(texColor == _Color3in) ? _Color3out : texColor;
				texColor = all(texColor == _Color4in) ? _Color4out : texColor;
				texColor = all(texColor == _Color5in) ? _Color5out : texColor;
				texColor = all(texColor == _Color6in) ? _Color6out : texColor;
				texColor = all(texColor == _Color7in) ? _Color7out : texColor;
				texColor = all(texColor == _Color8in) ? _Color8out : texColor;
				texColor = all(texColor == _Color9in) ? _Color9out : texColor;
				texColor = all(texColor == _Color10in) ? _Color10out : texColor;
				*/

				return texColor * IN.color;
			}
			ENDCG
		}
	}
}
