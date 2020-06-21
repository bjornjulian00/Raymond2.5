using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template
{
    public class SceneGraph
    {
        public Shader shaderIn;
        public Shader postShade;

        Surface screenCam;
        ScreenQuad postQuad;


        public SceneGraph(Surface screenIn)
        {
            return screenIn;
        }
    }
}
