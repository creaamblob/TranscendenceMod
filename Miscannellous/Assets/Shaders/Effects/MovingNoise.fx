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
bool useXOffset;
bool useYOffset;
float xOffset;
float yOffset;
float yChange;
float alpha;

float4 Sinewave(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    if (useXOffset)
        coords.x += xOffset;
    if (useYOffset)
        coords.y += yOffset;
    
    float4 color = tex2D(uImage0, coords);
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    
    imgCoords.x += uTime;
    imgCoords.y += yChange;
    float4 noise = tex2D(uImage1, imgCoords);
    noise.a = color.a;
    color.a *= uOpacity;
    
    if (!any(color))
        return color * uOpacity;

    noise.rgb = lerp(noise.rgb, uSecondaryColor, 0.5);
    color.rgb = noise.rgb;
    return float4(color.r, color.g, color.b, uOpacity);
}


    
technique MovingNoiseTechnique
{
    pass MovingNoiseTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}