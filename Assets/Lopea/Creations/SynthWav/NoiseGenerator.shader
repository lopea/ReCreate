Shader "Unlit/NoiseGenerator"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

        float2 random2d(float2 st)
        {
              st = float2( dot(st,float2(127.1,311.7)),
                         dot(st,float2(269.5,183.3)) );
            return -1.0 + 2.0*frac(sin(st)*43758.5453123);
        }
        
        float noise(float2 st)
        {

            
            //get decimal and integer portions from position
            float2 i = floor(st);
            float2 f = frac(st);
            
            //randomize each position around current position
            float2 a = random2d(i);
            float2 b = random2d(i + float2(1.0, 0.0));
            float2 c = random2d(i + float2(0.0, 1.0));
            float2 d = random2d(i + float2(1.0, 1.0));
            
            //interpolate decimal position
            float2 m = smoothstep(0,1,f);
            
            //apply interpolation to 4 corners and return result.
            return lerp(lerp(dot(a,f), dot(b, f-float2(1.0,0.0)), m.x),
                       lerp(dot(c,f - float2(0.0, 1.0)), dot(d,f - float2(1,1)), m.x), m.y);
        }
        
        float fbm(float2 uv)
        {
            
            float value = 0;
            float amplitude =.5;
            
            for(int i = 0; i < 10; i++)
            {
                value += (amplitude * noise(uv));
                amplitude *= 0.5;
                uv *= 2;
            }
            
            return value;
        }
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                col *= fbm(i.uv + _Time) * 10;
                return col;
            }
            ENDCG
        }

    }
}
