sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float2 uImageSize0;
float2 uImageSize1;
float maxColors;

float4 PixelShaderFunc(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 size = 1.0 / uImageSize1;
    size *= 2.0;
    
    float2 pixelatedCoords = floor(coords / size) * size;
    
    float4 color = tex2D(uImage0, pixelatedCoords);
    color.rgb = round(color.rgb * (maxColors - 1)) / (maxColors - 1);
    
    return color * sampleColor;
}


    
technique PixelationTechnique
{
    pass PixelationTechnique2
    {
        PixelShader = compile ps_2_0 PixelShaderFunc();
    }
}