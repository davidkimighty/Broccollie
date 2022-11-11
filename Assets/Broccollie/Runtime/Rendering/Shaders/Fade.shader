Shader "Hidden/Fade"
{
    Properties
    {
		_FadeColor ("FadeColor", Color) = (1, 1, 1)
		_Alpha ("Alpha", float) = 0.0
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			fixed4 _FadeColor;
			half _Alpha;

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 color = fixed4(_FadeColor.rgb, _Alpha);
				return color;
            }
            ENDCG
        }
    }
}
