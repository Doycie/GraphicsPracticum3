#version 330
 
in vec3 vertex;
in float vtime;

// shader output
out vec4 outputColor;

const float PI = 3.141592653;

 float Noise(int x, int y)
    {
        int n = x + y * 57;
        n = (n << 13) ^ n;
        return (1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
    }

     float SmoothNoise_1(int x, int y)
    {
        float corners = (Noise(x - 1, y - 1) + Noise(x + 1, y - 1) + Noise(x - 1, y + 1) + Noise(x + 1, y + 1)) / 16;
        float sides = (Noise(x - 1, y) + Noise(x + 1, y) + Noise(x, y - 1) + Noise(x, y + 1)) / 8;
        float center = Noise(x, y) / 4;
        return corners + sides + center;
    }

     float Interpolate(float a, float b, float x)
    {
        float ft = x * 3.1415927f;
        float f = (1 - cos(ft)) * .5f;
        return a * (1 - f) + b * f;
    }

     float InterpolatedNoise_1(float x, float y)
    {
        int integer_X = int(x);
        float fractional_X = x - integer_X;

        int integer_Y = int(y);
        float fractional_Y = y - integer_Y;

        float v1 = SmoothNoise_1(integer_X, integer_Y);
        float v2 = SmoothNoise_1(integer_X + 1, integer_Y);
        float v3 = SmoothNoise_1(integer_X, integer_Y + 1);
        float v4 = SmoothNoise_1(integer_X + 1, integer_Y + 1);

        float i1 = Interpolate(v1, v2, fractional_X);
        float i2 = Interpolate(v3, v4, fractional_X);

        return Interpolate(i1, i2, fractional_Y);
    }

     float perlinNoise(vec2 c)
    {
        float total = 0;
        float p = 0.25f;
        int n = 2 - 1;

        for (int i = 0; i < n; i++)
        {
            int frequency = 2 ^ i;
            float amplitude = pow(p, i);

            total = total + (InterpolatedNoise_1(c.x * frequency, c.y * frequency) * amplitude);
        }
        return total + 0.5f;
    }


int octaves = 8;
float gain = 0.65;
float lacunarity = 2.0;
float amplitude = 4.0;

void main()
{
	vec3 n = normalize(vertex);
	
	vec2 v = vec2((acos(n.z) + PI + vtime), atan(n.y / n.x) + PI);
		
	float total = 0;
	float frequency = 25;

	for(int i = 0; i < octaves; i++){
		total += perlinNoise(v.xy * frequency ) * amplitude;
		frequency *= lacunarity;
		amplitude  *= gain;
	}
	total /= 25 ;
	vec4 noise = vec4(total,total,total,1.0);

	outputColor = vec4(.05,0.1,0.8,1) * 0.5 + noise ;
	

	//outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
}

