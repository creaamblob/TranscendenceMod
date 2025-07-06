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
float2 uTargetPosition;


float2 Rotate(float2 c, float theta)
{
    return float2(c.x * cos(theta) - c.y * sin(theta), c.x * sin(theta) + c.y * cos(theta));
}

float4 Sinewave(float2 coords : TEXCOORD0) : COLOR0
{
    float4 col = float4(uColor.r, uColor.g, uColor.b, uOpacity);
    float2 pos = float2(0.5, 0.5);
    float distance = length(coords - pos) * 3;
    float alpha = 0;
    float2 pos2 = coords - pos;
    
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    imgCoords += Rotate(pos2, uTime * 2.0 + length(coords - pos) * 250.0);
    
    float4 noise = tex2D(uImage1, imgCoords);
    col.rgb -= noise.rgb * 0.66;

    if (distance > 1)
        return float4(0, 0, 0, 0);
    
    if (distance > 0.1)
    {
        alpha = lerp(0, 0.75, distance - 0.1f);
    }
    
    if (distance > 0.9)
        alpha = lerp(alpha, 0, (distance - 0.9) * 10);
    
    return float4(col.r, col.g, col.b, uOpacity * alpha);
}

    
technique SeraphArenaTechnique
{
    pass SeraphArenaTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}