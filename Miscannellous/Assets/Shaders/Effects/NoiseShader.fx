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
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    float4 noise = tex2D(uImage1, imgCoords);
    
	if (!any(color))
		return color;

    float3 extraCol = (uSaturation, uRotation, uTime);
    color.rgb = noise.rgb;
    
    if (uOpacity > 0)
        color *= uOpacity;
    return color;
}


    
technique NoiseTechnique
{
    pass NoiseTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}