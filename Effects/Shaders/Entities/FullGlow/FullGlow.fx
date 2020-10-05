sampler uImage0 : register(s0);


float4 FullGlow(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
	
    return color;
}

technique Technique1
{
    pass FullGlow
    {
        PixelShader = compile ps_2_0 FullGlow();
    }
}