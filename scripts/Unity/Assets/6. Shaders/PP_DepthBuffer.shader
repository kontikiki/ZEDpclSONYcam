Shader "Hidden/PP_DepthBuffer"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_DepthDistance("DepthDistance", float) = 25
    }
    SubShader
    {
        Cull Off 
		ZWrite On 
		ZTest Always

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
				float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			sampler2D_float _CameraDepthTexture;	//! 카메라로부터 뎁스텍스처를 받아옴
			half _DepthDistance;
            float fDepthData;
            float fSceneZ;
            float fCalc_Depth;
            fixed4 frag (v2f i) : SV_Target
            {
				fDepthData = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(i.uv, 1, 1))).r;	//! 텝스텍스처 샘플러에서 텍셀정보를 가져옴
			    fSceneZ = LinearEyeDepth(fDepthData);		//! DepthData를 0~1 Linear데이터로 변환(0 = 카메라, 1 = 먼거리)
                /*
                fSceneZ=1.0/(_ZBufferParams.z * z + _ZBufferParams.w);
                zc0 = 1.0 - m_FarClip/m_NearClip;
                zc1 = m_FarClip/m_NearClip;
                _ZBufferParams={ 1.0 - m_FarClip/m_NearClip, m_FarClip/m_NearClip, (m_NearClip-m_FarClip)/(m_FarClip*m_NearClip), 1/m_NearClip};
                fSceneZ=m_FarClip*m_NearClip/((m_NearClip-m_FarClip)*z+ m_FarClip);
                */
                fCalc_Depth =saturate(fSceneZ / _DepthDistance);		//! 거리 값 조절용 계산
                return fCalc_Depth;
            }  
            ENDCG
        }
    }
}