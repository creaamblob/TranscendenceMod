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

static const float weights[9] =
{
    0.05, 0.09, 0.12, 0.15, 0.16, 0.15, 0.12, 0.09, 0.05
};

float4 Sinewave(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
    if (!any(color))
        return color;
    
    float am = 20;
    float2 pixelUV = floor(coords * am) / am + uOpacity;
    
    float static2 = frac(sin(dot(pixelUV, float2(12.9898, 78.233))) * 43758.5453);
    
    color.rbg = lerp(color.rbg, color.rbg * 0.5, static2);
    
    float r = color.r;
    float g = color.g;
    float b = color.b;
    if (sampleColor.r < 0.1)
        r = 0;
    if (sampleColor.g < 0.1)
        g = 0;
    if (sampleColor.b < 0.1)
        b = 0;
    
    return float4(r, g, b, color.a);
}


    
technique CRTt
{
    pass CRTt2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}