using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Template;

namespace INFOGR2019Tmpl8
{
    class scenegraph
    {
		public List<Mesh> meshList;
		public Shader renderShader;
		public Shader postShader;

		RenderTarget currentTarget;
		ScreenQuad postQuad;
		Surface panel;

		public scenegraph (Surface panelIn)
        {
			panel = panelIn;
			meshList = new List<Mesh>();

			renderShader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
			postShader = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");

			currentTarget = new RenderTarget(panel.width, panel.height); ;
			postQuad = new ScreenQuad();
		}

		public void RenderMeshes(Matrix4 camMat, Matrix4 viewMat, RenderTarget curTarget, float frameTime)
        {
			currentTarget = curTarget;
			currentTarget.Bind();

			foreach (Mesh mesh in meshList)
			{
				mesh.Render(renderShader, mesh.position, mesh.position, camMat * viewMat, mesh.texture, frameTime);
			}
		}

		public float[,] MatrixTransform(float[,] inputArr1, float[,] inputArr2, int scalar, int operationID)
		{
			//Importing matrices and consolidating info
			float[,] input1 = inputArr1;
			int in1X = input1.GetLength(0);
			int in1Y = input1.GetLength(1);
			float[,] input2 = inputArr2;
			int in2X = input2.GetLength(0);
			int in2Y = input2.GetLength(1);

			// 0 = Addition
			// 1 = Subtraction
			// 2 = Scalar Multiplication
			// 3 = Matrix Multiplication

			if (operationID == 0 || operationID == 1) // Addition and subtraction
			{
				float[,] output = new float[in1X, in1Y];

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
				float[,] output = new float[in1X, in1Y];

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
				float[,] output = new float[in1X, in2Y];

				// Check it matrices are suitable for multiplication, else return input1
				if (in1X != in2Y || in1Y != in2X)
				{
					Console.WriteLine("Matrix multiplication failed, matrices not compatible.");
					return input1;
				}

				for (int i = 0; i < in1X; i++)
				{
					for (int j = 0; j < in2Y; j++)
					{
						output[i, j] = 0;

						for (int k = 0; k < in1Y; k++)
						{
							output[i, j] = output[i, j] + input1[i, k] * input2[k, j];
						}
					}
				}
			}

			// If no valid operationID is given, return input1
			return input1;
		}
	}
}
