// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Legacy Shaders/Better Lightmapped/Reflect Diffuse" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _LightMap ("Lightmap (RGB)", 2D) = "black" {}
    _Cube ("Reflection Cubemap", Cube) = "_Skybox" {}
}

SubShader {
    Lighting Off
    LOD 200
    Tags { "RenderType" = "Opaque" }
CGPROGRAM
#pragma surface surf Lambert nodynlightmap
struct Input {
  float2 uv_MainTex;
  float2 uv2_LightMap;
  float3 worldRefl;
};
sampler2D _MainTex;
sampler2D _LightMap;
fixed4 _Color, _ReflectColor;
samplerCUBE _Cube;
void surf (Input IN, inout SurfaceOutput o)
{
  float4 c = tex2D (_MainTex, IN.uv_MainTex) * ((_Color - (float4(half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w), 0.0)) / 10) / 7);
  //c.rgb = (c.rgb - 0.5) * (1.2) + 0.5;
  float4 lm = tex2D (_LightMap, IN.uv2_LightMap);
  //lm.rgb = (lm.rgb - 0.5) * (1.2) + 0.5;
  lm.rgb *= 14;
  fixed4 reflcol = texCUBE (_Cube, IN.worldRefl) * 7;
  reflcol *= (c.a);
  o.Albedo = c.rgb;
  o.Emission = reflcol.rgb * _ReflectColor.rgb + (lm.rgb*o.Albedo.rgb);
  o.Alpha = reflcol.a * _ReflectColor.a * 4;
}
ENDCG
}
FallBack "Legacy Shaders/Lightmapped/VertexLit"
}