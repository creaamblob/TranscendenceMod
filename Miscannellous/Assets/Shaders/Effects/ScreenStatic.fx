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
    if (!any (color)) 
        return color;
    
    
    float2 pixelatedUV = floor(float2(coords.x * (16 * uProgress), coords.y * (12 * uProgress))) + uIntensity;
    float static2 = frac(sin(dot(pixelatedUV + float2(uIntensity, uIntensity), float2(12.9898, 78.233))) * 43758.5453);
    

    color.rbg = lerp(color.rbg, float3(0, 0, 0), static2 * uOpacity);

    return color;
}

    
technique StaticTechnique
{
    pass StaticTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}