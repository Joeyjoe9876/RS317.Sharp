using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Rs317.Sharp
{
	public class GLUtil
	{
		private static int ERR_LEN = 1024;

		private static int[] buf = new int[1];
		private static float[] fbuf = new float[1];

		public static int glGetInteger()
		{
			GL.GetInteger(GetPName.MaxSamples, buf);
			return buf[0];
		}

		public static float glGetFloat()
		{
			//Work around: https://github.com/opentk/opentk/issues/688
			GL.GetFloat((GetIndexedPName)All.MaxTextureMaxAnisotropy, 0, fbuf);
			return fbuf[0];
		}

		private static int glGetShader(int shader)
		{
			GL.GetShader(shader, ShaderParameter.CompileStatus, buf);
			Debug.Assert(buf[0] > -1);
			return buf[0];
		}

		private static int glGetProgram(int program, int pname)
		{
			GL.GetProgram(program, (GetProgramParameterName)pname, buf);
			Debug.Assert(buf[0] > -1);
			return buf[0];
		}

		private static String glGetShaderInfoLog(int shader)
		{
			GL.GetShaderInfoLog(shader, ERR_LEN, out int size, out string infoString);
			return infoString;
		}

		public static String glGetProgramInfoLog(int program)
		{
			GL.GetProgramInfoLog(program, ERR_LEN, out int size, out string infoString);
			return infoString;
		}

		public static int glGenVertexArrays()
		{
			GL.GenVertexArrays(1, buf);
			return buf[0];
		}

		public static void glDeleteVertexArrays(int vertexArray)
		{
			buf[0] = vertexArray;
			GL.DeleteVertexArrays(1, buf);
		}

		public static int glGenBuffers()
		{
			GL.GenBuffers(1, buf);
			return buf[0];
		}

		public static void glDeleteBuffer(int buffer)
		{
			buf[0] = buffer;
			GL.DeleteBuffers(1, buf);
		}

		public static int glGenTexture()
		{
			GL.GenTextures(1, buf);
			return buf[0];
		}

		public static void glDeleteTexture(int texture)
		{
			buf[0] = texture;
			GL.DeleteTextures(1, buf);
		}

		public static int glGenFrameBuffer()
		{
			GL.GenFramebuffers(1, buf);
			return buf[0];
		}

		public static void glDeleteFrameBuffer(int frameBuffer)
		{
			buf[0] = frameBuffer;
			GL.DeleteFramebuffers(1, buf);
		}

		public static int glGenRenderbuffer()
		{
			GL.GenRenderbuffers(1, buf);
			return buf[0];
		}

		public static void glDeleteRenderbuffers(int renderBuffer)
		{
			buf[0] = renderBuffer;
			GL.DeleteRenderbuffers(1, buf);
		}

		public static void loadShaders(int glProgram, int glVertexShader, int glGeometryShader, int glFragmentShader,
								String vertexShaderStr, String geomShaderStr, String fragShaderStr)
		{
			compileAndAttach(glProgram, glVertexShader, vertexShaderStr);

			if (glGeometryShader != -1)
			{
				compileAndAttach(glProgram, glGeometryShader, geomShaderStr);
			}

			compileAndAttach(glProgram, glFragmentShader, fragShaderStr);

			GL.LinkProgram(glProgram);

			if (glGetProgram(glProgram, (int)GetProgramParameterName.LinkStatus) == 0) //GL_FALSE
			{
				String err = glGetProgramInfoLog(glProgram);
				throw new ShaderException(err);
			}

			GL.ValidateProgram(glProgram);

			if (glGetProgram(glProgram, (int)GetProgramParameterName.ValidateStatus) == 0) //GL_FALSE
			{
				String err = glGetProgramInfoLog(glProgram);
				throw new ShaderException(err);
			}
		}

		public static void loadComputeShader(int glProgram, int glComputeShader, String str)
		{
			compileAndAttach(glProgram, glComputeShader, str);

			GL.LinkProgram(glProgram);

			if (glGetProgram(glProgram, (int)GetProgramParameterName.LinkStatus) == 0) //GL_FALSE
			{
				String err = glGetProgramInfoLog(glProgram);
				throw new ShaderException(err);
			}

			GL.ValidateProgram(glProgram);

			if (glGetProgram(glProgram, (int)GetProgramParameterName.ValidateStatus) == 0) //GL_FALSE
			{
				String err = glGetProgramInfoLog(glProgram);
				throw new ShaderException(err);
			}
		}

		private static void compileAndAttach(int program, int shader, String source)
		{
			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);

			if (glGetShader(shader) == 1) //GL_TRUE
			{
				GL.AttachShader(program, shader);
			}
			else
			{
				String err = glGetShaderInfoLog(shader);
				throw new ShaderException(err);
			}
		}
	}
}