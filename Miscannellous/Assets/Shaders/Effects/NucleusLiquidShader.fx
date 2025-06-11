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
    float y = 0.25 * sin(coords.x * 18 + uTime) * 0.25 + 0.25;
    
    float4 color = tex2D(uImage0, coords);
    color.rgba *= 0;
    
    for (int i = 0; i < 6; i++)
    {
        float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
        imgCoords.x += uTime;
        if (i > 4)
        {
            imgCoords.x += uTime * 0.2 * (i * 0.75);
            imgCoords.y -= y * (i * 0.75);
        }
        imgCoords.y -= y * 20;
        float4 noise = tex2D(uImage1, imgCoords);
        color.rgba += noise.rgba * 0.1375;
    }
    
    color.r += 0.25;
    color.a *= color.r;

    if (coords.y > 0.25)
    {
        color.rgb *= (1 - (coords.y - 0.25) * 3);
        color.b = lerp(color.b, 1, coords.y - 0.25);
    }
    
    if (coords.y < y)
    {
        float pix = 4 / uImageSize0;
        if (coords.y < (y - pix))
            return float4(0, 0, 0, 0);
        else
            return float4(1, 1, 1, 1);
    }
    
    color.rgba = round(color.rgba * (16 - 1)) / (16 - 1);
    
    return color;
}


    
technique NucleusLiquidTech
{
    pass NucleusLiquidTech2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}