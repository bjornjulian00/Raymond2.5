using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template
{

    class light
    {
        public Vector3 lightPos;
        public Vector3 lightColor;

        public light(Shader shader, Vector3 lightPosIn, Vector3 lightColorIn)
        {
            lightPos = lightPosIn;
            int lightID = GL.GetUniformLocation(shader.programID, "lightPos");
            GL.UseProgram(shader.programID);
            GL.Uniform3(lightID, lightPos);

            lightColor = lightColorIn;
            int colorID = GL.GetUniformLocation(shader.programID, "lightColor");
            GL.UseProgram(shader.programID);
            GL.Uniform3(colorID, lightColor);
        }
    }
}
