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
float xChange;

float4 Sinewave(float2 coords : TEXCOORD0) : COLOR0
{
    float sine = sin(uTime * 35 + (75 + (sin((coords.x * 2) * 7.5))));
    coords.y += lerp(-0.075f, 0.075f, sine);
    coords.x -= xChange;
    
    float4 color = tex2D(uImage0, coords);
    
    color.rgb = lerp(color.rgb, float3(0, 1, 1), 0.75);
    
    float2 imgCoords = ((coords * uImageSize0 - uSourceRect.xy)) / uImageSize1;
   
    
    imgCoords.x += uTime;
    
    float4 noise = tex2D(uImage1, imgCoords);
    noise.a = color.a;
    color.a *= uOpacity;
    
    
    if (!any(color))
        return color * uOpacity;
    
    color.rgb = lerp(noise.rgb, color.rgb, 0.75);
    
    return float4(color.r, color.g, color.b, color.a);
}


    
technique SeraphsTechnique
{
    pass SeraphsTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}