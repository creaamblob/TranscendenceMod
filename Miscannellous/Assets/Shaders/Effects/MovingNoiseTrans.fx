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
bool useAlpha = false;
float alpha;
bool useExtraCol;
float3 extraCol;

float4 Sinewave(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float4 color2 = tex2D(uImage0, coords);

    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    
    imgCoords.x += uTime;
    imgCoords.y += yChange;
    
    float4 noise = tex2D(uImage1, imgCoords);
    noise.a = color.a;
    
    if (!any(color))
        return color;

    noise.rgb = lerp(noise.rgb, uSecondaryColor, 0.5);
    if (useExtraCol)
        color.rgb = lerp(noise.rgb, extraCol, 0.25);
    else
        color.rgb = noise.rgb;
    
    if (useAlpha)
        color.a *= alpha;
    
    return float4(color.r, color.g, color.b, color.a);
}


    
technique MovingNoiseTTechnique
{
    pass MovingNoiseTTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}