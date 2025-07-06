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

float4 Sinewave(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
	if (!any(color))
		return color;

    float2 pos = float2(0.5, (coords.x + uTime) % 1.0);

    return tex2D(uImage1, pos) * color;
}


    
technique SeraphsFullTechnique
{
    pass SeraphsFullTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}