sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;


float4 Sinewave(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
    if (((color.r + color.g + color.b) / 3) > 0.75)
    {
        color.rbg = lerp(color.rbg, dot(float3(0.3, 0.59, 0.11), color.rgb), 0.875 * uOpacity);
    }
    else
        color.rbg = lerp(color.rbg, dot(float3(0, 0, 0), color.rgb), uOpacity);
    
    if (color.r > 0.6 && color.g > 0.6 && color.b > 0.6)
        color.rgb *= 0.125;
    
    return color;
}

    
technique BlindTechnique
{
    pass BlindTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}