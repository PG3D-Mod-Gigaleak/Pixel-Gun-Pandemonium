Shader "Mobile/Overlay Diffuse" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OverlayTex ("Overlay (RGBA)", 2D) = "black" {}
        _OverlayStrength ("Overlay Strength", Range(0, 1)) = 0.5
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 150

        CGPROGRAM
        #pragma surface surf Lambert noforwardadd

        sampler2D _MainTex;
        sampler2D _OverlayTex;
        float _OverlayStrength;

        struct Input {
            float2 uv_MainTex;
            float2 uv_OverlayTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 overlayColor = tex2D(_OverlayTex, IN.uv_OverlayTex);
            o.Albedo = lerp(baseColor.rgb, overlayColor.rgb, overlayColor.a * _OverlayStrength);
            o.Alpha = baseColor.a;
        }
        ENDCG
    }

    FallBack "Mobile/VertexLit"
}
