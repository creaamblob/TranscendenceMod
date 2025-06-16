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
    float4 col = float4(uColor.r, uColor.g, uColor.b, uOpacity);
    
    coords.x -= uTime / 4;
    
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;

    imgCoords.y -= uTime / 80;
    float4 noise = tex2D(uImage1, imgCoords);
    col.rgb -= 0.75;
    col.rgb += noise.rgb * 0.75;
    
    float4 trans = float4(0, 0, 0, 0);
    float4 color = float4(col.r, col.g, col.b, uOpacity);
    
    if (col.r > 0.6)
    {
        return lerp(trans, color, (color.r - 0.6) * 12);

    }
    else
        return trans;
}

    
technique DesperateTechnique
{
    pass DesperateTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}