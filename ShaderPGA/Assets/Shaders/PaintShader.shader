Shader "Custom/PaintShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrushPos ("Brush Position", Vector) = (0,0,0,0)
        _BrushSize ("Brush Size", Float) = 0.3
        _BrushColor ("Brush Color", Color) = (1,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            ZTest Always Cull Off ZWrite Off
            
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _BrushPos;
            float  _BrushSize;
            float4 _BrushColor;

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

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(float4(v.positionOS.xyz, 1.0));
                o.uv = v.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.uv;
                float4 baseCol = tex2D(_MainTex, uv);
                float d = distance(uv, _BrushPos.xy);

                if(d < _BrushSize)
                {
                    return _BrushColor;
                }
                return baseCol;
            }
            ENDHLSL
        }
    }
}
