sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
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
float2 uImageSize2;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float4 Sinewave(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    imgCoords.x += uTime;
    
    float2 imgCoords2 = (coords * uImageSize0 - uSourceRect.xy) / uImageSize2;
    imgCoords2.x += uTime * 0.75;
    
    float4 noise = tex2D(uImage1, imgCoords);
    float4 noise2 = tex2D(uImage2, imgCoords2);
    
    float2 pos = float2(0.5, 0.5);
    float distance = length(coords - pos) * 3;
    
    noise.rgb = lerp(noise.rgb, float3(1, 1, 0), 0.66);
    
    color.rgb = float3(0.85, 0.65, 0);
    
    if (noise.r > 0.8 && noise.g > 0.8 && distance < 0.85)
    {
        color.rgb *= 1.0 + (noise.r - 0.8) * 4.0;
        color.rgb += noise2.rgb * 0.75;
    }

    
    
    
    if (distance < 0.85)
    {
        float chance = frac(sin(dot(coords, float2(23.235236346, 124.436346346) * uTime)) * 235235.345644);
        
        if (chance > (distance * 0.75))
        {
            color.rgb *= 1.25;
        }

    }
    
    if (distance > 0.725 && distance < 0.8)
        color.rgb *= 1.0 + (distance - 0.725) * 12;
    
    if (distance > 0.775)
    {
        return lerp(float4(1, 1, 1, 1), float4(1, 1, 1, 0), (distance - 0.8) * 20);
    }
    

    return color;
}


    
technique AnthroSpiritTechnique
{
    pass AnthroSpiritTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}