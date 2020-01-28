// WireFrame renders its materials into a wire-frame mode, as in only the edges
// between vertexs are visible, and any planes become invisible. Additionally,
// the HLSL also is in charge of vertex manipulations in the form of waving
Shader "Wireframe"
{
    Properties
    {
        //Variables are set
        //Front and Back Color are the colors of the visible wireframe 'wires'.
        //Front is only visible from above the normal, and back from below. 
        //Wireframe width is the thickness of the lines.  
        _MainTex ("Texture", 2D) = "blue" {}
        [PowerSlider(3.0)]
        _WireframeVal ("Wireframe width", Range(0., 0.34)) = 0.0
        _FrontColor ("Front color", color) = (0, 0, 1., 1.) 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        //Front Pass
        Pass
        {
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            #include "UnityCG.cginc"
 			uniform float waveIntensity;
 
            struct v2g {
                float4 pos : SV_POSITION;
            };
 
            struct g2f {
                float4 pos : SV_POSITION;
                float3 bary : TEXCOORD0;
            };
 
            v2g vert(appdata_base v) { 
                //This line of code determines the waviness of the WireFrame.
                //Note that waveIntensitiy is a variable that is changed throughout other scripts 
                // (i.e. Mine explosions intensify waves)
                float4 displacement = float4(0.0f, sin((v.vertex.x + _Time.y))/waveIntensity, 0.0f, 0.0f);
				v.vertex += displacement;

				v2g o;
				o.pos = UnityObjectToClipPos(v.vertex); 
				return o;

            }
 
            [maxvertexcount(3)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
                g2f o;
                o.pos = IN[0].pos;
                o.bary = float3(1., 0., 0.);
                triStream.Append(o);
                o.pos = IN[1].pos;
                o.bary = float3(0., 0., 1.);
                triStream.Append(o);
                o.pos = IN[2].pos;
                o.bary = float3(0., 1., 0.);
                triStream.Append(o);
            }
            //Set Wireframe values. 
            float _WireframeVal;
            fixed4 _FrontColor;
 
            fixed4 frag(g2f i) : SV_Target {
            if(!any(bool3(i.bary.x < _WireframeVal, i.bary.y < _WireframeVal, i.bary.z < _WireframeVal)))
                 discard;
                return _FrontColor;
            }
 
            ENDCG
        }
    }
}
