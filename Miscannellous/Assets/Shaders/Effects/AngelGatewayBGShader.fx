sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uProgress;
float uTime;
float4 uSourceRect;
float2 uImageSize0;
float2 uImageSize1;
float2 centre;

float2 Rotate(float2 c, float theta)
{
    return float2(c.x * cos(theta) - c.y * sin(theta), c.x * sin(theta) + c.y * cos(theta));
}

float4 Shader(float2 coords : TEXCOORD0) : COLOR0
{
    float4 col = float4(uColor.r, uColor.g, uColor.b, 1);
    float2 pos = centre;
    float distance = length(coords - pos) * 3;
    float2 pos2 = coords - pos;
    
    float2 imgCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    imgCoords += Rotate(pos2, uTime + length(coords - pos) * 10.0);
    
    float4 noise = tex2D(uImage1, imgCoords);
    
    noise.rgb = lerp(noise.rgb, float3(uSecondaryColor), 0.66);

    
    col.rgb += noise.rgb * 0.66 * (distance - uProgress);
    
    col.rgb = round(col.rgb * (24 - 1)) / (24 - 1);

    return float4(col.r, col.g, col.b, 0.5);
}

    
technique AGBGt
{
    pass AGBGt2
    {
        PixelShader = compile ps_2_0 Shader();
    }
}