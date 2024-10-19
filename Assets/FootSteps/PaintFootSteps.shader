Shader "Custom/PaintFootSteps"
{
    Properties{
        _Tess("Tessellation", Range(1,32)) = 4
        _MainTex("Base (RGB)", 2D) = "white" {}
        _UpColor("UpColor", color) = (1,1,1,0)
        _DownColor("DownColor", color) = (1,1,1,0)
        _DispTex("Disp Texture", 2D) = "gray" {}
        _Displacement("Displacement", Range(0, 1.0)) = 0.3

        _Glossiness("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 300

        CGPROGRAM
        #pragma surface surf Standard addshadow fullforwardshadows vertex:disp tessellate:tessDistance nolightmap
        #pragma target 4.6
        #include "Tessellation.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };

        float _Tess;

        float4 tessDistance(appdata v0, appdata v1, appdata v2) {
            float minDist = 20.0;
            float maxDist = 100.0;
            return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
        }

        sampler2D _DispTex;
        float _Displacement;

        void disp(inout appdata v)
        {
            float d = tex2Dlod(_DispTex, float4(v.texcoord.xy,0,0)).r * _Displacement;
            v.vertex.xyz -= v.normal * d;
            v.vertex.xyz += v.normal * _Displacement;
        }

        struct Input {
            float2 uv_MainTex;
            float2 uv_DispTex;
        };

        sampler2D _MainTex;
        fixed4 _UpColor,_DownColor;
        half _Glossiness;

        void surf(Input IN, inout SurfaceOutputStandard o) {

            float des = tex2Dlod(_DispTex, float4(IN.uv_DispTex, 0, 0)).r;
            half4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 c = lerp(_UpColor, _DownColor, des) * mainTex;

            o.Albedo = c.rgb;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
