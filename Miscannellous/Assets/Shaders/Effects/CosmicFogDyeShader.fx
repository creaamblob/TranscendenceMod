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

float4 CosmicFogDyeS(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
    if (!any(color))
        return color;
    
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / (uImageSize1 / 3.75);

    imgCoords.x += cos(uTime) * 0.125 - sin(uTime) * 0.5;
    imgCoords.y += sin(uTime) * 0.125 + cos(uTime) * 0.5;

    float4 noise = tex2D(uImage1, imgCoords);
    
    noise.rgb = lerp(noise.rgb, float3(0.25, 0, 1), 0.5);
    noise.rgb = round(noise.rgb * (12 - 1)) / (12 - 1);
    
    //Scaled size of the pixels
    float pixX = 2 / uImageSize0.x;
    float pixY = 2 / uImageSize0.y;
    
    for (int i = -1; i < 2; i++)
    {
        for (int j = -1; j < 2; j++)
        {
            float4 color2 = tex2D(uImage0, coords + float2(pixX * i, pixY * j));
            if (!any(color2))
                noise.rgb *= 0.66;
        }
    }
    
    if (coords.y < pixY)
        noise.rgb *= 0.66;
    
    return float4(noise.r, noise.g, noise.b, color.a);
}


    
technique CosmicFogDyeShaderT
{
    pass CosmicFogDyeShaderT2
    {
        PixelShader = compile ps_2_0 CosmicFogDyeS();
    }
}