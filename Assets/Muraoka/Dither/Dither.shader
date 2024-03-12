Shader "DitheringFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DitherLevel("DitherLevel", Range(0, 16)) = 1 //�f�B�U�����O�̋�𒲐�
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
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            int _DitherLevel;

            static const int pattern[16] = { //static���Ȃ��ƃA�N�Z�X�ł��Ȃ��̂Œ��ӁI
                 0,  8,  2, 10,
                12,  4, 14,  6,
                 3, 11,  1,  9,
                15,  7, 13,  5
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); //���_��MVP�s��ϊ�
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); //�e�N�X�`���X�P�[���ƃ^�C�����O������
                o.screenPos = ComputeScreenPos(o.vertex); //�N���b�v���W����X�N���[�����W���v�Z
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 viewPortPos = i.screenPos.xy / i.screenPos.w; //w�ŏ��Z���A�X�N���[���ł̓��e�ʒu���擾
                float2 screenPosInPixel = viewPortPos.xy * _ScreenParams.xy; //0�`1�̐��`����s�N�Z���ɕϊ�

                // �f�B�U�����O�e�N�X�`���p��UV���쐬
                int ditherUV_x = (int)fmod(screenPosInPixel.x, 4.0f); //�p�^�[���̑傫���Ŋ������Ƃ��̗]������߂�
                int ditherUV_y = (int)fmod(screenPosInPixel.y, 4.0f); //����̃p�^�[���T�C�Y��4x4�Ȃ̂�4�ŏ��Z
                float dither = pattern[ditherUV_x + ditherUV_y * 4]; //���߂��]�肩��p�^�[���l���擾

                clip(dither - _DitherLevel); //臒l��0�ȉ��Ȃ�`�悵�Ȃ�

                float4 color = tex2D(_MainTex, i.uv); //���C���e�N�X�`������T���v�����O
                return color;
            }
            ENDCG
        }
    }
}