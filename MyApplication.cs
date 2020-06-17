using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using OpenTK;

namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;                  // background surface for printing etc.
		Mesh mesh, floor;                       // a mesh to draw using OpenGL
		const float PI = 3.1415926535f;         // PI
		float a = 0;                            // teapot rotation angle
		Stopwatch timer;                        // timer for measuring frame duration
		Shader shader;                          // shader to use for rendering
		Shader postproc;                        // shader to use for post processing
		Texture wood;                           // texture to use for rendering
		RenderTarget target;                    // intermediate render target
		ScreenQuad quad;                        // screen filling quad for post processing
		bool useRenderTarget = true;

		// initialize
		public void Init()
		{
			// load teapot
			mesh = new Mesh( "../../assets/teapot.obj" );
			floor = new Mesh( "../../assets/floor.obj" );
			// initialize stopwatch
			timer = new Stopwatch();
			timer.Reset();
			timer.Start();
			// create shaders
			shader = new Shader( "../../shaders/vs.glsl", "../../shaders/fs.glsl" );
			postproc = new Shader( "../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl" );
			// load a texture
			wood = new Texture( "../../assets/wood.jpg" );
			// create the render target
			target = new RenderTarget( screen.width, screen.height );
			quad = new ScreenQuad();
		}

		// tick for background surface
		public void Tick()
		{
			screen.Clear( 0 );
			screen.Print( "hello world", 2, 2, 0xffff00 );
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
			Matrix4 Tcamera = Matrix4.CreateTranslation( new Vector3( 0, -14.5f, 0 ) ) * Matrix4.CreateFromAxisAngle( new Vector3( 1, 0, 0 ), angle90degrees );
			Matrix4 Tview = Matrix4.CreatePerspectiveFieldOfView( 1.2f, 1.3f, .1f, 1000 );

			// update rotation
			a += 0.001f * frameDuration;
			if( a > 2 * PI ) a -= 2 * PI;

			if( useRenderTarget )
			{
				// enable render target
				target.Bind();

				// render scene to render target
				mesh.Render( shader, Tpot * Tcamera * Tview, wood );
				floor.Render( shader, Tfloor * Tcamera * Tview, wood );

				// render quad
				target.Unbind();
				quad.Render( postproc, target.GetTextureID() );
			}
			else
			{
				// render scene directly to the screen
				mesh.Render( shader, Tpot * Tcamera * Tview, wood );
				floor.Render( shader, Tfloor * Tcamera * Tview, wood );
			}
		}
	}

	public class SceneGraph
    {
		public SceneGraph (SceneGraph parent, SceneGraph child)
        {
			SceneGraph parentEl = parent;
			SceneGraph childEl = child;
        }

        public float[,] MatrixTransform (float[,] inputArr1, float[,] inputArr2, int scalar, int operationID)
        {
			//Importing matrices and consolidating info
			float[,] input1 = inputArr1;
			int in1X = input1.GetLength(0);
			int in1Y = input1.GetLength(1);
			float[,] input2 = inputArr2;
			int in2X = input2.GetLength(0);
			int in2Y = input2.GetLength(1);

			float[,] output = new float[in1X, in1Y];

			// 0 = Addition
			// 1 = Subtraction
			// 2 = Scalar Multiplication
			// 3 = Matrix Multiplication

			if (operationID == 0 || operationID == 1) // Addition and subtraction
			{
				// Check if matrices are both same size, else return input1
				if (in1X != in2X || in1Y != in2Y)
				{
					Console.WriteLine("Matrix addition / subtraction failed, matrices not compatible.");
					return input1;
				}

				// Set coefficient for adding / subtracting
				int coefficient;
				if (operationID == 0)
					coefficient = 1;
				else
					coefficient = -1;

				// Add / subtract matrices
				for (int x = 0; x < in1X; x++)
				{
					for (int y = 0; y < in1Y; y++)
					{
						output[x, y] = input1[x, y] + (input2[x, y] * coefficient);
					}
				}

				return output;
			}

			else if (operationID == 2) // Scalar multiplication
			{
				for (int x = 0; x < in1X; x++)
				{
					for (int y = 0; y < in1Y; y++)
					{
						output[x, y] = (input1[x, y] * scalar);
					}
				}

				return output;
			}

			else if (operationID == 3) // Matrix multiplication
			{
				// Check it matrices are suitable for multiplication, else return input1
				if (in1X != in2Y || in1Y != in2X)
				{
					Console.WriteLine("Matrix multiplication failed, matrices not compatible.");
					return input1;
				}

				int c[,] = new int[a.GetLength(0), b.GetLength(1)];
				for (int i = 0; i < c.GetLength(0); i++)
				{
					for (int j = 0; j < c.GetLength(1); j++)
					{
						c[i, j] = 0;
						for (int k = 0; k < a.GetLength(1); k++) // OR k<b.GetLength(0)
							c[i, j] = c[i, j] + a[i, k] * b[k, j];
					}
				}
			}

			// If no valid operationID is given, return input1
			return input1;
        }
    }
}