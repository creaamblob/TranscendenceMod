sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float4 Sinewave(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
    if (!any(color))
        return color;
    
    color.rgb = lerp(color.rgb, float3(uRotation, uTime, uDirection), uSaturation);
    return float4(color.r, color.g, color.b, uOpacity) * uOpacity;
}


    
technique SeraphOutlineTechnique
{
    pass SeraphOutlineTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}