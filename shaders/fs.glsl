#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 worldPos;               // world space position of fragment
uniform sampler2D pixels;		// texture sampler

// shader output
out vec4 outputColor;           // shader out

uniform vec3 lightPos;          // light position in world space
uniform vec3 lightColor;        // light color
uniform vec3 ambientLight;      // ambient light
uniform vec4 specRef;           // specular refection

// fragment shader
void main()
{
    vec3 L = lightPos - worldPos.xyz;
    float dist = length(L);
    L = normalize( L );
    vec3 materialColor = texture( pixels, uv).xyz;
    float attenuation = 1.0f / (dist * dist);
    vec3 normView = normalize(specRef.xyz - worldPos.xyz);
    vec3 refView = normalize(reflect(-normView, normal.xyz));
    int specStrength = 16;
    float specCalc = pow(max(dot(L, refView), 0.0f), specStrength);
    outputColor = vec4( ambientLight + (specCalc*attenuation*lightColor) + materialColor * max(0.0f, dot(L, normal.xyz)) * attenuation * lightColor, 1);
}