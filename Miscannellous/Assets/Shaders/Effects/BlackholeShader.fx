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
    float2 distortPos = (uTargetPosition - uScreenPosition) / uScreenResolution;
    float2 center = (coords - distortPos) * (uScreenResolution / uScreenResolution.y);
    
    float dotField = dot(center, center);
    float offset = dotField * 5 * 3.14 - uProgress * 5;
    
    if (offset > 0.075 && offset < 0.5)
        return lerp(tex2D(uImage0, center * sin(coords) + offset * 0.9), tex2D(uImage0, coords), offset);
    if (offset > 0.5 && offset < 0.66)
        return lerp(tex2D(uImage0, coords), tex2D(uImage0, center * cos(coords) + offset * 0.4), offset * 2);
    else
        return tex2D(uImage0, coords);

}

    
technique BlackholeGravLensTech
{
    pass BlackholeGravLensTech2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}