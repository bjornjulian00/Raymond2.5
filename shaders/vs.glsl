#version 330
 
// shader input
in vec2 vUV;				// vertex uv coordinate
in vec3 vNormal;			// untransformed vertex normal
in vec3 vPosition;			// untransformed vertex position

uniform mat4 transform;		// model space to screen space
uniform mat4 toWorld;		// model space to world space

// shader output
out vec4 normal;			// transformed vertex normal
out vec2 uv;
out vec4 worldPos;
 
// vertex shader
void main()
{
	// forward uv; will be interpolated over triangle
	uv = vUV;

	// transform vertex using supplied matrix
	gl_Position = transform * vec4( vPosition, 1.0); 
	worldPos = toWorld * vec4( vPosition, 1.0f );

	// forward normal and worldPos; they will be interpolated over triangle
	normal = normalize(toWorld * vec4( vNormal, 0.0f ));
	
}