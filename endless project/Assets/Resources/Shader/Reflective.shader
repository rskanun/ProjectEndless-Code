Shader"Custom/ReflectionShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _ReflectionTex ("Reflection", 2D) = "white" { }
    }
    
    SubShader
    {
        Tags { "Queue" = "Overlay" }
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
                float3 normal : NORMAL;
            };
            
struct v2f
{
    float4 pos : POSITION;
    float4 color : COLOR;
};
            
sampler2D _MainTex;
sampler2D _ReflectionTex;
float _ReflectionStrength;
            
v2f vert(appdata v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.color = float4(1, 1, 1, 1);
    return o;
}
            
            fixed4 frag(v2f i) : COLOR
            {
                fixed4 col = tex2D(_MainTex, i.pos.xy / i.pos.w);
                fixed4 reflection = tex2D(_ReflectionTex, i.pos.xy / i.pos.w);
                col.rgb += reflection.rgb * _ReflectionStrength; // Adjust strength as needed
                return col;
            }
            ENDCG
        }
    }
}