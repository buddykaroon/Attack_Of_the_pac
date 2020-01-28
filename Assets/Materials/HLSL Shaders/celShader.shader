//CelShader is an HLSL shader that renders affected surfaces with only
//3 shades; Either a Unlit color, a 'shiny' highlight color (often white),
// and an lit color. Whichever one is chosen depends on light level on the
// surface, and the Lighting threshold / Shininess variables.
//Additionally, we used Outline Thickness to get 
// the cartoon edge. 

Shader "celShader" {
	
	//Set Properties of Material
   Properties {
    _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
    _UnlitColor ("Unlit Color", Color) = (0.5,0.5,0.5,1)
    _DiffuseThreshold ("Lighting Threshold", Range(-1.1,1)) = 0.1
    _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
    _Shininess ("Shininess", Range(0.5,1)) = 1	
    _OutlineThickness ("Outline Thickness", Range(0,1)) = 0.1
    _MainTex ("Main Texture", 2D) = "white" {}   
	}
   
    SubShader {
     Pass { 

	Tags{ "LightMode" = "ForwardBase" 
	      "RenderType"="Opaque" }
         
      	CGPROGRAM
      	
		//Declare vertex and fragment shaders. 
      	#pragma vertex vert  
      	#pragma fragment frag  
		  
		//Declare Variables. 
      	uniform float4 _LightColor0;
      	uniform sampler2D _MainTex;
      	uniform float4 _MainTex_ST;     
      	uniform float4 _Color;
      	uniform float4 _UnlitColor;
      	uniform float _DiffuseThreshold;
      	uniform float4 _SpecColor;
      	uniform float _Shininess;
      	uniform float _OutlineThickness;  	 
		  
		//Declare Structure of Input
      	struct vertexInput {  
      	float4 vertex : POSITION;
      	float3 normal : NORMAL;
      	float4 texcoord : TEXCOORD0;
      	};

		//Declare Structure of Output
      	struct vertexOutput {
           	float4 pos : SV_POSITION;
           	float3 normalDir : TEXCOORD1;
           	float4 lightDir : TEXCOORD2;
           	float3 viewDir : TEXCOORD3;
      		float2 uv : TEXCOORD0; 
      	};
      	
		//Vertex shader vert is in charge of assigning vertex directions
      	vertexOutput vert(vertexInput input)
      	{
      		vertexOutput output;
      		//Normals
      		output.normalDir = normalize ( mul( float4( input.normal, 0.0 ), unity_WorldToObject).xyz );
      		//World
      		float4 posWorld = mul(unity_ObjectToWorld, input.vertex);
      		//View
      		output.viewDir = normalize( _WorldSpaceCameraPos.xyz - posWorld.xyz ); 
      		//Light Direction
      		float3 fragmentToLightSource = ( _WorldSpaceCameraPos.xyz - posWorld.xyz);
      		output.lightDir = float4(
      			normalize( lerp(_WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w) ),
      			lerp(1.0 , 1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w)
      		);
      		output.pos = UnityObjectToClipPos( input.vertex );  
      		output.uv =input.texcoord;
      		return output;

      	}
      	
		//Fragment shader Frag is in charge of 
      	float4 frag(vertexOutput input) : COLOR
      	{
		float nDotL = saturate(dot(input.normalDir, input.lightDir.xyz)); 
				
		//Determine the cutofff for Diffuse. Varies on variable _DiffisueThreshold.
		float diffuseCutoff = saturate( ( max(_DiffuseThreshold, nDotL) - _DiffuseThreshold ) *1000 );
		//Determine cutoff for Specular. Varies on variable Shininess
		float specularCutoff = saturate( max(_Shininess, dot(reflect(-input.lightDir.xyz, input.normalDir), input.viewDir))-_Shininess ) * 1000;
		//Calculate Outlines
		float outlineVar = saturate( (dot(input.normalDir, input.viewDir ) - _OutlineThickness) * 1000 );
		float3 ambient = (1-diffuseCutoff) * _UnlitColor.xyz; 

		//Use calculated cutt-offs to find Reflections for Diffuse and Specular 
		float3 diffReflection = (1-specularCutoff) * _Color.xyz * diffuseCutoff;
		float3 specReflection = _SpecColor.xyz * specularCutoff;
			
		float3 combinedLight = (ambient + diffReflection) * outlineVar + specReflection;
				
		return float4(combinedLight, 1.0); // + tex2D(_MainTex, input.uv); // DELETE LINE COMMENTS & ';' TO ENABLE TEXTURE
      	}
      	ENDCG
      }

   
   }
	Fallback "Diffuse"
}