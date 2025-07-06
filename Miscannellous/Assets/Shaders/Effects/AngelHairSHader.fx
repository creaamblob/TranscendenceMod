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

float4 EarthHairShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / float2(uImageSize1.x / 4, uImageSize1.y / 2);
    imgCoords.y += uTime;
    
    float4 noise = tex2D(uImage1, imgCoords);


    if (!any(color))
        return color;
    
    noise.rgb = lerp(noise.rgb, float3(1, 0.75, 0), 0.5);
    
    color.rgb = lerp(color.rgb, float3(1.25, 0.66, 0), 0.5);
    
    if (color.r < 0.825 && color.b < 0.825)
        return float4(color.r * 0.75, color.g * 0.4, color.b * 0.4, color.a);
    
    if (noise.r > 0.5 && color.g > 0.5)
        color.rgb = noise.rgb * 24;
    
    return float4(color.r, color.g, color.b, color.a);
}


    
technique AngelHairShaderT
{
    pass AngelHairShaderT2
    {
        PixelShader = compile ps_2_0 EarthHairShader();
    }
}