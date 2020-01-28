//Hologram Shader is a HLSL script that creates a fade from left to right,
//often from an opaque color to a transparent one. 
//It is a simple shader, useful when combined with a rotational script
//to create interesting visuals. 

Shader "Hologram" 
{
    //Left and Right would act as color varaibles to be changed according
    // to the specific material (i.e. a boost would be green / transparent,
    // a powerup would be yellow / transparent.)
Properties {
     [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
     _Color ("Left", Color) = (1,1,1,1)
     _Color2 ("Right", Color) = (1,1,1,1)
           }

SubShader {
     Tags {"Queue"="Transparent"  "IgnoreProjector"="True"}
     LOD 100
     ZWrite Off
     Pass {
         
        Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
         
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        fixed4 _Color;
        fixed4 _Color2;
         struct v2f {
             float4 pos : SV_POSITION;
             fixed4 col : COLOR;
         };
        //Apply variable col (Color) to each vertex based on their position.
         v2f vert (appdata_full v)
         {
             v2f o;
             o.pos = UnityObjectToClipPos (v.vertex);
             //Lerp to smoothly transition between the two colors. 
             o.col = lerp(_Color,_Color2, v.texcoord.x );
             return o;
         }

         float4 frag (v2f i) : COLOR {
             float4 c = i.col;
             return c;
         }
             ENDCG
         }
     }
}