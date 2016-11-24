// Shader created with Shader Forge v1.13 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.13;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,bsrc:0,bdst:1,culm:2,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:9123,x:32987,y:32692,varname:node_9123,prsc:2|diff-8824-OUT,spec-9061-OUT,gloss-1274-OUT,normal-7410-OUT,clip-1035-A;n:type:ShaderForge.SFN_Tex2d,id:1035,x:32449,y:32363,ptovrint:False,ptlb:Diffuse Map (Cutout A),ptin:_DiffuseMapCutoutA,varname:node_1035,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8824,x:32719,y:32472,varname:node_8824,prsc:2|A-1035-RGB,B-9096-RGB;n:type:ShaderForge.SFN_Color,id:9096,x:32509,y:32557,ptovrint:False,ptlb:Diffuse color,ptin:_Diffusecolor,varname:node_9096,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3318,x:32059,y:33156,ptovrint:False,ptlb:Normal Map,ptin:_NormalMap,varname:node_3318,prsc:2,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:3024,x:31891,y:33385,ptovrint:False,ptlb:Normal Intensity,ptin:_NormalIntensity,varname:node_3024,prsc:2,min:1,cur:0,max:0;n:type:ShaderForge.SFN_Vector1,id:3767,x:32028,y:33517,varname:node_3767,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:1765,x:32244,y:33385,varname:node_1765,prsc:2|A-3024-OUT,B-3767-OUT;n:type:ShaderForge.SFN_Add,id:7410,x:32378,y:33223,varname:node_7410,prsc:2|A-3318-RGB,B-1765-OUT;n:type:ShaderForge.SFN_Tex2d,id:9576,x:32246,y:32621,ptovrint:False,ptlb:Specular Map,ptin:_SpecularMap,varname:node_9576,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:3546,x:32613,y:32917,ptovrint:False,ptlb:Spec color,ptin:_Speccolor,varname:node_3546,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:1450,x:32588,y:32689,varname:node_1450,prsc:2|A-9576-RGB,B-7242-OUT;n:type:ShaderForge.SFN_Multiply,id:9061,x:32790,y:32744,varname:node_9061,prsc:2|A-1450-OUT,B-3546-RGB;n:type:ShaderForge.SFN_Slider,id:7242,x:32245,y:32887,ptovrint:False,ptlb:Spec Intensity,ptin:_SpecIntensity,varname:node_7242,prsc:2,min:0,cur:1,max:2;n:type:ShaderForge.SFN_Slider,id:1274,x:32835,y:32256,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_1274,prsc:2,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Color,id:2480,x:33743,y:33371,ptovrint:False,ptlb:Emission Intesity_copy,ptin:_EmissionIntesity_copy,varname:node_6567,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;proporder:9096-1035-3318-3024-9576-3546-7242-1274;pass:END;sub:END;*/

Shader "Ciconia Studio/Double Sided/Transparent/Diffuse Bump Cutout" {
    Properties {
        _Diffusecolor ("Diffuse color", Color) = (1,1,1,1)
        _DiffuseMapCutoutA ("Diffuse Map (Cutout A)", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalIntensity ("Normal Intensity", Range(1, 0)) = 0
        _SpecularMap ("Specular Map", 2D) = "white" {}
        _Speccolor ("Spec color", Color) = (1,1,1,1)
        _SpecIntensity ("Spec Intensity", Range(0, 2)) = 1
        _Gloss ("Gloss", Range(0, 1)) = 0.5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _DiffuseMapCutoutA; uniform float4 _DiffuseMapCutoutA_ST;
            uniform float4 _Diffusecolor;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _NormalIntensity;
            uniform sampler2D _SpecularMap; uniform float4 _SpecularMap_ST;
            uniform float4 _Speccolor;
            uniform float _SpecIntensity;
            uniform float _Gloss;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = (_NormalMap_var.rgb+(_NormalIntensity*1.0));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float4 _DiffuseMapCutoutA_var = tex2D(_DiffuseMapCutoutA,TRANSFORM_TEX(i.uv0, _DiffuseMapCutoutA));
                clip(_DiffuseMapCutoutA_var.a - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _SpecularMap_var = tex2D(_SpecularMap,TRANSFORM_TEX(i.uv0, _SpecularMap));
                float3 specularColor = ((_SpecularMap_var.rgb*_SpecIntensity)*_Speccolor.rgb);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = (_DiffuseMapCutoutA_var.rgb*_Diffusecolor.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _DiffuseMapCutoutA; uniform float4 _DiffuseMapCutoutA_ST;
            uniform float4 _Diffusecolor;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _NormalIntensity;
            uniform sampler2D _SpecularMap; uniform float4 _SpecularMap_ST;
            uniform float4 _Speccolor;
            uniform float _SpecIntensity;
            uniform float _Gloss;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = (_NormalMap_var.rgb+(_NormalIntensity*1.0));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float4 _DiffuseMapCutoutA_var = tex2D(_DiffuseMapCutoutA,TRANSFORM_TEX(i.uv0, _DiffuseMapCutoutA));
                clip(_DiffuseMapCutoutA_var.a - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _SpecularMap_var = tex2D(_SpecularMap,TRANSFORM_TEX(i.uv0, _SpecularMap));
                float3 specularColor = ((_SpecularMap_var.rgb*_SpecIntensity)*_Speccolor.rgb);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = (_DiffuseMapCutoutA_var.rgb*_Diffusecolor.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform sampler2D _DiffuseMapCutoutA; uniform float4 _DiffuseMapCutoutA_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _DiffuseMapCutoutA_var = tex2D(_DiffuseMapCutoutA,TRANSFORM_TEX(i.uv0, _DiffuseMapCutoutA));
                clip(_DiffuseMapCutoutA_var.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
