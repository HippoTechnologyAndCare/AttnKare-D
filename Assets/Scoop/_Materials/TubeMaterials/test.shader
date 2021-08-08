Shader "test"
{

    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _Emission("Emissive Color", Color) = (0,0,0,0)
        _MainTex("Base (RGB)", 2D) = "white" { }
        _Transparency("Transparency", Float) = 0.25
    }

        SubShader
        {
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass {
                Name "BASE"
                CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag

                    #include "UnityCG.cginc"
                    #include "BendEffect.cginc"

                    struct appdata_t {
                        float4 vertex : POSITION;
                        float2 texcoord : TEXCOORD0;
                    };

                    struct v2f {
                        float4 vertex : SV_POSITION;
                        half2 texcoord : TEXCOORD0;
                    };

                    sampler2D _MainTex;
                    float4 _MainTex_ST;
                    float4 _Emission;
                    float4 _Color;
                    float4 _Transparency;

                    v2f vert(appdata_t v)
                    {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(BendEffect(v.vertex));
                        o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                        return o;
                    }

                    fixed4 frag(v2f i) : COLOR
                    {
                        fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;
                        col.a = _Transparency;
                        return col + _Emission;
                    }
                ENDCG
            }
        }

}