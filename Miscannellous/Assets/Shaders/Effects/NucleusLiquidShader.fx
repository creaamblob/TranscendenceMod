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
    float y = 0.25 * sin(coords.x * 18 + uTime) * 0.3 + 0.25;
    
    float2 size = 1.0 / uImageSize0;
    size *= 4.0;
    
    float2 pixelatedCoords = floor(coords / size) * size;
    float4 color = tex2D(uImage0, pixelatedCoords);
    color.rgba *= 0;
    
    for (int i = 0; i < 5; i++)
    {
        float2 imgCoords = (pixelatedCoords * uImageSize0 - uSourceRect.xy) / uImageSize1;
        
        if (i > 2)
        {
            imgCoords.x += uTime * (i * 0.25);
            imgCoords.y -= y * (i * 0.75);
        }
        else
            imgCoords.x += uTime * (i * 0.25) * 0.2;
        
        imgCoords.y += sin(coords.x * 18 + uTime) * 0.02;
        
        float4 noise = tex2D(uImage1, imgCoords);
        color.rgba += noise.rgba * 0.15;
    }
    
    color.r += 0.25;
    color.a *= color.r;

    if (coords.y > 0.25)
    {
        color.rgb *= (1 - (coords.y - 0.25) * 3);
        color.b = lerp(color.b, 1, coords.y - 0.25);
    }
    
    if (pixelatedCoords.y < y)
    {
        float pix = 6.0 / uImageSize0;
        if (pixelatedCoords.y < (y - pix))
            return float4(0, 0, 0, 0);
        else
            return float4(1, 0, 0, 1);
    }
    
    color.rgba = round(color.rgba * (18 - 1)) / (18 - 1);

    coords = floor(coords / size) * size;

    
    return color;
}


    
technique NucleusLiquidTech
{
    pass NucleusLiquidTech2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}