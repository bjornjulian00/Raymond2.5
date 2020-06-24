using System.Diagnostics;
using System.Linq.Expressions;
using OpenTK;
//using OpenTK.Graphics.ES10;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;                  // background surface for printing etc.
		Mesh mesh, floor;                       // a mesh to draw using OpenGL
		const float PI = 3.1415926535f;         // Pi
		float a = 0;                            // teapot rotation angle
		Stopwatch timer;                        // timer for measuring frame duration
		Shader shader;                          // shader to use for rendering
		Shader postproc;                        // shader to use for post processing
		Texture wood;                           // texture to use for rendering
		RenderTarget target;                    // intermediate render target
		ScreenQuad quad;                        // screen filling quad for post processing
		bool useRenderTarget = true;
		float xMove, zMove, rotate;

		scenegraph sGraph;

		// initialize
		public void Init()
		{
			sGraph = new scenegraph(this.screen);
			wood = new Texture("../../assets/wood.jpg");

			// load stuff
			mesh = new Mesh( "../../assets/teapot.obj" );
			floor = new Mesh( "../../assets/floor.obj" );
			floor.texture = wood;
			floor.modelAxis = new Vector3(0, 0, 0);
			floor.worldAxis = new Vector3(0, 0, 0);
			floor.modelRotate = 0.0f;
			floor.modelRSpeed = 0.0f;
			mesh.texture = wood;
			mesh.location = Matrix4.CreateScale(0.75f) * Matrix4.CreateTranslation(0, 0, 0);
			sGraph.addChild(floor);

			// initialize stopwatch
			timer = new Stopwatch();
			timer.Reset();
			timer.Start();
			
			// create the render target
			target = new RenderTarget( screen.width, screen.height );
			quad = new ScreenQuad();

			shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
			postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
			int lightID = GL.GetUniformLocation(shader.programID, "ambientLight");
			GL.UseProgram(shader.programID);
			GL.Uniform3(lightID, new Vector3(0.1f, 0.1f, 0.1f));

			light light1 = new light(shader, new Vector3(0, 7, 0), new Vector3(10, 10, 10));
		}

		// tick for background surface
		public void Tick()
		{
			screen.Clear( 0 );
			screen.Print( "hello world", 2, 2, 0xffff00 );
			readKey();
		}

		public void readKey()
        {
			KeyboardState keyboardIn = OpenTK.Input.Keyboard.GetState();
			if (keyboardIn.IsKeyDown(OpenTK.Input.Key.W))	zMove += .05f;
			if (keyboardIn.IsKeyDown(OpenTK.Input.Key.A))	xMove += .05f;
			if (keyboardIn.IsKeyDown(OpenTK.Input.Key.S))	zMove -= .05f;
			if (keyboardIn.IsKeyDown(OpenTK.Input.Key.D))	xMove -= .05f;
			if (keyboardIn.IsKeyDown(OpenTK.Input.Key.Q))	rotate += .01f;
			if (keyboardIn.IsKeyDown(OpenTK.Input.Key.E))	rotate -= .01f;
		}

		// tick for OpenGL rendering code
		public void RenderGL()
		{
			// measure frame duration
			float frameDuration = timer.ElapsedMilliseconds;
			timer.Reset();
			timer.Start();

			// prepare matrix for vertex shader
			float angle90degrees = PI / 2;
			Matrix4 Tpot = Matrix4.CreateScale( 0.5f ) * Matrix4.CreateFromAxisAngle( new Vector3( 0, 1, 0 ), a );
			Matrix4 Tfloor = Matrix4.CreateScale( 4.0f ) * Matrix4.CreateFromAxisAngle( new Vector3( 0, 1, 0 ), a );
			Matrix4 Tcamera = Matrix4.CreateTranslation(
					new Vector3( xMove, -14.5f, zMove ) ) * 
				Matrix4.CreateFromAxisAngle( 
					new Vector3( 1, 0, 0 ), angle90degrees );
			Matrix4.CreateFromAxisAngle(
					new Vector3(0, 0, 1), rotate);
			Matrix4 Tview = Matrix4.CreatePerspectiveFieldOfView( 1.2f, 1.3f, .1f, 1000 );
			Matrix4 toWorld = Tpot;
			Matrix4 specRef = Tcamera;

			int specIn = GL.GetUniformLocation(shader.programID, "specRef");
			GL.UseProgram(shader.programID);
			GL.Uniform3(specIn, new Vector3(0.1f, 0.1f, 0.1f));

			// update rotation
			a += 0.001f * frameDuration;
			if( a > 2 * PI ) a -= 2 * PI;

			sGraph.Render(Tcamera, Tview, target);

			/*if( useRenderTarget )
			{
				// enable render target
				target.Bind();

				// render scene to render target
				mesh.Render( shader, Tpot * Tcamera * Tview, wood, Tpot );
				floor.Render( shader, Tfloor * Tcamera * Tview, wood, Tfloor );

				// render quad
				target.Unbind();
				quad.Render( postproc, target.GetTextureID() );
			}
			else
			{
				// render scene directly to the screen
				mesh.Render( shader, Tpot * Tcamera * Tview, wood, Tpot );
				floor.Render( shader, Tfloor * Tcamera * Tview, wood, Tfloor );
			}*/
		}
	}
}