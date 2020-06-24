using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template
{
    class scenegraph
    {
        public Shader shader;
        public Shader postShader;
        RenderTarget rTarget;
        ScreenQuad postQuad;
        Surface display;

        public List<Mesh> renderTree;

        public scenegraph (Surface display)
        {
            //Assign target
            rTarget = new RenderTarget(display.width, display.height);
            postQuad = new ScreenQuad();
            //Begin init of render tree
            renderTree = new List<Mesh>();
            //Assign shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postShader = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
        }
        
        //Method to allow us to add meshes directly to the tree without list interaction
        public void addChild(Mesh childMesh)
        {
            renderTree.Add(childMesh);
        }

        public void Render(Matrix4 Tcamera, Matrix4 Tview, RenderTarget target)
        {
            this.rTarget = target;
            target.Bind();

            foreach (Mesh mesh in renderTree)
            {
                mesh.Render(shader, mesh.location, mesh.texture, mesh.location);
            }

            target.Unbind();
            postQuad.Render(postShader, target.GetTextureID());
        }
    }
}
