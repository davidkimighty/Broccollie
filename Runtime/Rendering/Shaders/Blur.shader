Shader "Hidden/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white"
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"
        }

        HLSLINCLUDE
        #pragma vertex vert
        #pragma fragment frag

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        struct Attributes
        {
            float4 positionOS : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionHCS : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        TEXTURE2D(_MainTex);

        SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

        float _BlurStrength;

        Varyings vert(Attributes IN)
        {
            Varyings OUT;
            OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
            OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
            return OUT;
        }
        ENDHLSL

        Pass
        {
            Name "VERTICAL BOX BLUR"

            HLSLPROGRAM
            half4 frag(Varyings IN) : SV_TARGET
            {
                float2 res = _MainTex_TexelSize.xy;
                half4 sum = 0;

                int samples = 2 * _BlurStrength + 1;

                for (float y = 0; y < samples; y++)
                {
                    float2 offset = float2(0, y - _BlurStrength);
                    sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + offset * res);
                }
                return sum / samples;
            }
            ENDHLSL
        }

        Pass
        {
            Name "HORIZONTAL BOX BLUR"

            HLSLPROGRAM
            half4 frag(Varyings IN) : SV_TARGET
            {
                float2 res = _MainTex_TexelSize.xy;
                half4 sum = 0;

                int samples = 2 * _BlurStrength + 1;

                for (float x = 0; x < samples; x++)
                {
                    float2 offset = float2(x - _BlurStrength, 0);
                    sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + offset * res);
                }

                return sum / samples;
            }
            ENDHLSL
        }
    }
}
