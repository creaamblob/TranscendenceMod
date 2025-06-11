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
    
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / (uImageSize1 / 7.5);

    imgCoords.x += uTime * 0.33;
    
    float4 noise = tex2D(uImage1, imgCoords);
    
    if (!any(color) || color.a < 0.01)
        return float4(0, 0, 0, 0);
    
    bool Alt = false;
    
    if (Alt)
    {
        color.rgb = noise.rgb;
    
        bool outline = false;
        
        //Scaled size of the pixels
        float pixX = 1 / uImageSize0.x;
        float pixY = 1 / uImageSize0.y;
    
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                float4 color2 = tex2D(uImage0, coords + float2(pixX * i, pixY * j));
                if (!any(color2))
                    outline = true;
            }
        }
    
        if (outline)
            color.rgb *= 0.4;
    }
    else
    {
        color.rgb = ((color.rgb - 0.5) * 3) + 0.125;
        noise.rgb = ((noise.rgb - 0.5) * 1.25) + 0.5;
        color.rgb = lerp(color.rgb, noise.rgb, 0.875);
        
        if (color.g < 0.625 && color.b < 0.625)
            return float4(color.r * 0.75, color.g * 0.75, color.b * 0.75, color.a);
    }
    
    return float4(color.r, color.g, color.b, color.a);
}


    
technique EarthHairShaderT
{
    pass EarthHairShaderT2
    {
        PixelShader = compile ps_2_0 EarthHairShader();
    }
}