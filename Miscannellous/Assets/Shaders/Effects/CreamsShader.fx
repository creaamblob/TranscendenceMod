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
    
    float frame = (coords.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;
    float sine = 0.1 + frac(frame + uTime * 0.85);

    color.rgb -= float3(0.5f, 0.5f, 0.5f);
    
    if (sine > 1.05)
        color = float4(1, 1, 1, 1);
    
    color.rgb += lerp(float3(0, 0.5, 0.75) * 1.5f, float3(0, 0.5, 0.75) * 0.25f, sine);
    
    return color * sampleColor;
}


    
technique TestTechnique
{
    pass Test2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}