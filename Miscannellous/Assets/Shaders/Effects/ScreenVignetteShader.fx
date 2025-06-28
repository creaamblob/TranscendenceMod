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
    float2 pos = float2(1, 0.5);
    float distance = length(float2(coords.x * 2, coords.y * 1.1) - pos) * uIntensity;
    float alpha = 0;
    
    if (distance > 0.1) 
    {
        alpha = lerp(0, 0.1875, (distance - 0.1) * 20);
        if (distance > 0.3)
            alpha = lerp(0.75, lerp(0.75, 0.8, distance * 2), (distance - 0.3) * 10);
    }
    
    float4 col = lerp(color.rgba, float4(0, 0, 0, alpha), uProgress * alpha);
    return col;
}

    
technique ScreenVignetteTechnique
{
    pass ScreenVignetteTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}