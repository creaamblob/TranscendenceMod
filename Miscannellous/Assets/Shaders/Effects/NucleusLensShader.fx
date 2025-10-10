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
    coords = round(coords / 0.005) * 0.005;
    float4 color = tex2D(uImage0, coords);
    
    if (((color.r + color.g + color.b) / 3) > 0.25)
    {
        color.rbg = lerp(color.rbg, float3(0.75, 0, 0), 0.5 * uOpacity);
    }
    else
        color.rbg = lerp(color.rbg, dot(float3(0.25, 0, 0), color.rgb), uOpacity);
    
    color.rgb = round(color.rgb * (12 - 1)) / (12 - 1);
    
    return color;
}

    
technique NLensTechnique
{
    pass NLensTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}