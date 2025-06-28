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


float4 Sinewave(float2 coords : TEXCOORD0) : COLOR0
{
    float4 col = float4(uColor.r, uColor.g, uColor.b, uOpacity);
    float2 pos = float2(0.5, 0.5);
    float distance = length(coords - pos) * 3;
    float alpha = 0;
    
    coords = round(coords / 0.005) * 0.005;
    
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    imgCoords.y -= uTime;
    
    float4 noise = tex2D(uImage1, imgCoords);
    col.rgb += noise.rgb * 0.25;

    if (distance > 0.99 && distance < 1)
        return (noise + float4(0, 1, 1, 1)) * uOpacity;
    
    if (distance > 0.85 && distance < 1)
    {
        alpha = lerp(0, 0.75, distance - 0.85);
    }
    else
        return float4(0, 0, 0, 0);
    
    return float4(col.r, col.g, col.b, uOpacity * alpha);
}

    
technique BoundaryTechnique
{
    pass BoundaryTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}