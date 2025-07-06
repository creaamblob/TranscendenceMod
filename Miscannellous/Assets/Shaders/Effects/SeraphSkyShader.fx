sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
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


float4 Sinewave(float2 coords : TEXCOORD0) : COLOR0
{
    float4 col = float4(0, 0, 0, uOpacity);
    
    for (int i = 0; i < 6; i++)
    {
        float size = lerp(0.75, 2.25, i / 6.0);
        float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / (uImageSize1 * size);
        
        imgCoords.x += uTime * lerp(0.33, 2.25, i / 6.0);
        imgCoords.y += uTime * lerp(0.33, 2.25, i / 6.0);

        float4 noise = tex2D(uImage1, imgCoords);
        
        noise.a = noise.r;
        
        float4 purple = float4(uColor.r, uColor.g, uColor.b, 1.0);
        
        purple.rgb *= noise.a;
        
        float3 col2 = purple.rgb;
        
        col.rgb += col2 * (1 / 12.0);
    }
    
    return float4(col.r, col.g, col.b, uOpacity);
}

    
technique SeraphSkyTechnique
{
    pass SeraphSkyTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}