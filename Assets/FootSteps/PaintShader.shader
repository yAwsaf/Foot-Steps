Shader "Unlit/PaintShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FootTex("Texture", 2D) = "white" {}
        _Color("Color",COLOR) = (1,1,1,1)
        _Coord("Coordinates",Vector) = (0,0,0,0)
        _SquareSize("SquareSize",Float) = 1
        _Angle("Angle", Range(0,6.283)) = 0.0
       // _LocalToWorld("_LocalToWorld", Matrix4x4)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex, _FootTex;
            float4 _MainTex_ST;
            fixed4 _Coord, _Color;
            float _SquareSize, _Angle;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float cosAngle = cos(_Angle);
                float sinAngle = sin(_Angle);
                float2x2 rot = float2x2(cosAngle, -sinAngle
                    , sinAngle, cosAngle);

                /*
                i.uv = i.uv * 2 - 1;
                i.uv = mul(rot, i.uv);
                i.uv = i.uv * 0.5f + 0.5;
                */

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 center = (_Coord.xy);
                float sdf = max(abs(i.uv.x - center.x) - 0.5 * _SquareSize, abs(i.uv.y - center.y) - 0.5 * _SquareSize);
                float SDF = step(0, sdf);
                float2 uv = (1 - SDF) * smoothstep(-_SquareSize * 0.5f, _SquareSize * 0.5f, (i.uv - center) + 0.0f * _SquareSize);

                uv =  (uv * 2 - 1);
                uv = mul(rot, uv);
                uv = (1 - SDF) * (uv * 0.5f + 0.5);
                //return float4(uv, 0, 1);
                if (uv.x == 0.0f)
                {
                    return col;
                }
                else {
                    return saturate(col + tex2D(_FootTex,uv));
                }
                //return lerp(float4(0,0,0,0), _Color, SDF);
                
            }
            ENDCG
        }
    }
}
