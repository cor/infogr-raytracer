using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace infogr_raytracer
{
	public class Surface
	{
		public readonly int Width;
		public readonly int Height;
		public int[] Pixels;
	
		// Font
		private static bool _fontReady = false;
		private static Surface _font;
		private static int[] _fontRedir;
		
		// OpenGL values for rendering the screen's texture 
		private readonly float[] _vertices =
		{
			// Position         Texture coordinates
			 1.0f,  1.0f, 0.0f, 1.0f, 0.0f, // top right
			 1.0f, -1.0f, 0.0f, 1.0f, 1.0f, // bottom right
			-1.0f, -1.0f, 0.0f, 0.0f, 1.0f, // bottom left
			-1.0f,  1.0f, 0.0f, 0.0f, 0.0f  // top left
		};
        
		private readonly uint[] _indices =
		{
			0, 1, 3,
			1, 2, 3
		};
        
		private int _vertexBufferObject;
		private int _vertexArrayObject;
		private int _elementBufferObject;
		
		private Shader _shader;
		private int _textureId;
		

		/// <summary>
		/// Constructs a Surface with a specified width and height
		/// </summary>
		/// <param name="w">The width of the Surface</param>
		/// <param name="h">The height of the Surface</param>
		public Surface( int w, int h )
		{
			Width = w;
			Height = h;
			Pixels = new int[w * h];
			
			CreateTexture();
		}


		/// <summary>
		/// Creates the OpenGL VBO, EBO, VAO, Shader, and Texture needed for rendering the Surface.
		/// </summary>
		private void CreateTexture()
		{
			            
			_vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

			_elementBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
			
			_vertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArrayObject);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

			_textureId = GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, _textureId);
            
			_shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
			_shader.Use();

			int vertexLocation = _shader.GetAttribLocation("aPosition");
			GL.EnableVertexAttribArray(vertexLocation);
			GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

			int texCoordLocation = _shader.GetAttribLocation("aTexCoord");
			GL.EnableVertexAttribArray(texCoordLocation);
			GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
		}
		
		/// <summary>
		/// Constructs a Surface using a file
		/// </summary>
		/// <param name="fileName">The filename of the surface</param>
		public Surface( string fileName )
		{
			Bitmap bmp = new Bitmap( fileName );
			Width = bmp.Width;
			Height = bmp.Height;
			Pixels = new int[Width * Height];
			BitmapData data = bmp.LockBits( new Rectangle( 0, 0, Width, Height ), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			IntPtr ptr = data.Scan0;
			System.Runtime.InteropServices.Marshal.Copy( data.Scan0, Pixels, 0, Width * Height );
			bmp.UnlockBits( data );
		}

		/// <summary>
		/// Creates an OpenGL texture
		/// </summary>
		/// <returns>An ID of the texture provided by OpenGL</returns>
		public int GenTexture()
		{
			int id = GL.GenTexture();
			GL.BindTexture( TextureTarget.Texture2D, id );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear );
			GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, Pixels );
			return id;
		}
		
		/// <summary>
		/// Clears the surface to a specified color
		/// </summary>
		/// <param name="c">The color that the surface should be cleared to</param>
		public void Clear( int c )
		{
			for( int s = Width * Height, p = 0; p < s; p++ ) Pixels[p] = c;
		}
		
		// copy the surface to another surface
		/// <summary>
		/// Copy the surface to another surface
		/// </summary>
		/// <param name="target">The surface that should be copied to</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void CopyTo( Surface target, int x = 0, int y = 0 )
		{
			int src = 0;
			int dst = 0;
			int srcwidth = Width;
			int srcheight = Height;
			int dstwidth = target.Width;
			int dstheight = target.Height;
			if( (srcwidth + x) > dstwidth ) srcwidth = dstwidth - x;
			if( (srcheight + y) > dstheight ) srcheight = dstheight - y;
			if( x < 0 )
			{
				src -= x;
				srcwidth += x;
				x = 0;
			}
			if( y < 0 )
			{
				src -= y * Width;
				srcheight += y;
				y = 0;
			}
			if( (srcwidth > 0) && (srcheight > 0) )
			{
				dst += x + dstwidth * y;
				for( int v = 0; v < srcheight; v++ )
				{
					for( int u = 0; u < srcwidth; u++ ) target.Pixels[dst + u] = Pixels[src + u];
					dst += dstwidth;
					src += Width;
				}
			}
		}
		
		/// <summary>
		/// Draw a rectangle
		/// </summary>
		/// <param name="x1">The X component of the top-left coordinate</param>
		/// <param name="y1">The Y component of the top-left coordinate</param>
		/// <param name="x2">The X component of the bottom-right coordinate</param>
		/// <param name="y2">The Y component of the bottom-right coordinate</param>
		/// <param name="c">The color that the bar should be drawn in</param>
		public void Box( int x1, int y1, int x2, int y2, int c )
		{
			int dest = y1 * Width;
			for( int y = y1; y <= y2; y++, dest += Width )
			{
				Pixels[dest + x1] = c;
				Pixels[dest + x2] = c;
			}
			int dest1 = y1 * Width;
			int dest2 = y2 * Width;
			for( int x = x1; x <= x2; x++ )
			{
				Pixels[dest1 + x] = c;
				Pixels[dest2 + x] = c;
			}
		}

		/// <summary>
		/// Draw a solid bar
		/// </summary>
		/// <param name="x1">The X component of the top-left coordinate</param>
		/// <param name="y1">The Y component of the top-left coordinate</param>
		/// <param name="x2">The X component of the bottom-right coordinate</param>
		/// <param name="y2">The Y component of the bottom-right coordinate</param>
		/// <param name="c">The color that the bar should be drawn in</param>
		public void Bar( int x1, int y1, int x2, int y2, int c )
		{
			int dest = y1 * Width;
			for( int y = y1; y <= y2; y++, dest += Width ) for( int x = x1; x <= x2; x++ )
			{
				Pixels[dest + x] = c;
			}
		}
		/// <summary>
		/// helper function for line clipping
		/// </summary>
		int OUTCODE( int x, int y )
		{
			int xmin = 0, ymin = 0, xmax = Width - 1, ymax = Height - 1;
			return (((x) < xmin) ? 1 : (((x) > xmax) ? 2 : 0)) + (((y) < ymin) ? 4 : (((y) > ymax) ? 8 : 0));
		}
		
		/// <summary>
		/// Draw a line, clipped to the window
		/// </summary>
		/// <param name="x1">The X component of the top-left coordinate</param>
		/// <param name="y1">The Y component of the top-left coordinate</param>
		/// <param name="x2">The X component of the bottom-right coordinate</param>
		/// <param name="y2">The Y component of the bottom-right coordinate</param>
		/// <param name="c">The color that the bar should be drawn in</param>
		public void Line( int x1, int y1, int x2, int y2, int c )
		{
			int xmin = 0, ymin = 0, xmax = Width - 1, ymax = Height - 1;
			int c0 = OUTCODE( x1, y1 ), c1 = OUTCODE( x2, y2 );
			bool accept = false;
			while( true )
			{
				if( c0 == 0 && c1 == 0 ) { accept = true; break; }
				else if( (c0 & c1) > 0 ) break;
				else
				{
					int x = 0, y = 0;
					int co = (c0 > 0) ? c0 : c1;
					if( (co & 8) > 0 ) { x = x1 + (x2 - x1) * (ymax - y1) / (y2 - y1); y = ymax; }
					else if( (co & 4) > 0 ) { x = x1 + (x2 - x1) * (ymin - y1) / (y2 - y1); y = ymin; }
					else if( (co & 2) > 0 ) { y = y1 + (y2 - y1) * (xmax - x1) / (x2 - x1); x = xmax; }
					else if( (co & 1) > 0 ) { y = y1 + (y2 - y1) * (xmin - x1) / (x2 - x1); x = xmin; }
					if( co == c0 ) { x1 = x; y1 = y; c0 = OUTCODE( x1, y1 ); }
					else { x2 = x; y2 = y; c1 = OUTCODE( x2, y2 ); }
				}
			}
			if( !accept ) return;
			if( Math.Abs( x2 - x1 ) >= Math.Abs( y2 - y1 ) )
			{
				if( x2 < x1 ) { int h = x1; x1 = x2; x2 = h; h = y2; y2 = y1; y1 = h; }
				int l = x2 - x1;
				if( l == 0 ) return;
				int dy = ((y2 - y1) * 8192) / l;
				y1 *= 8192;
				for( int i = 0; i < l; i++ )
				{
					Pixels[x1++ + (y1 / 8192) * Width] = c;
					y1 += dy;
				}
			}
			else
			{
				if( y2 < y1 ) { int h = x1; x1 = x2; x2 = h; h = y2; y2 = y1; y1 = h; }
				int l = y2 - y1;
				if( l == 0 ) return;
				int dx = ((x2 - x1) * 8192) / l;
				x1 *= 8192;
				for( int i = 0; i < l; i++ )
				{
					Pixels[x1 / 8192 + y1++ * Width] = c;
					x1 += dx;
				}
			}
		}
		// plot a single pixel
		
		/// <summary>
		/// Plot a single pixel
		/// </summary>
		/// <param name="x">The X position of the pixel</param>
		/// <param name="y">The Y position of the pixel</param>
		/// <param name="c">The Color of the pixel</param>
		public void Plot( int x, int y, int c )
		{
			if( (x >= 0) && (y >= 0) && (x < Width) && (y < Height) )
			{
				Pixels[x + y * Width] = c;
			}
		}
		
		/// <summary>
		/// Print a string with it's top-left coroner at a specified position
		/// </summary>
		/// <param name="t">The text of the string</param>
		/// <param name="x">The X component of the top-left corner</param>
		/// <param name="y">The Y component of the top-left corner</param>
		/// <param name="c">The color of the string</param>
		public void Print( string t, int x, int y, int c )
		{
			if( !_fontReady )
			{
				_font = new Surface( "./assets/font.png" );
				string ch = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+={}[];:<>,.?/\\ ";
				_fontRedir = new int[256];
				for( int i = 0; i < 256; i++ ) _fontRedir[i] = 0;
				for( int i = 0; i < ch.Length; i++ )
				{
					int l = (int)ch[i];
					_fontRedir[l & 255] = i;
				}
				_fontReady = true;
			}
			for( int i = 0; i < t.Length; i++ )
			{
				int f = _fontRedir[(int)t[i] & 255];
				int dest = x + i * 12 + y * Width;
				int src = f * 12;
				for( int v = 0; v < _font.Height; v++, src += _font.Width, dest += Width ) for( int u = 0; u < 12; u++ )
				{
					if( (_font.Pixels[src + u] & 0xffffff) != 0 ) Pixels[dest + u] = c;
				}
			}
		}

		public void Draw()
		{
			// Draw the screen
			GL.BindVertexArray(_vertexArrayObject);
			GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, 
				PixelFormat.Bgra, PixelType.UnsignedByte, Pixels);
			GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
		}

		/// <summary>
		/// Unloads all OpenGL buffers, VAOs, and shader programs.
		/// </summary>
		public void Unload()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.UseProgram(0);

			GL.DeleteBuffer(_vertexBufferObject);
			GL.DeleteBuffer(_elementBufferObject);
			GL.DeleteVertexArray(_vertexArrayObject);
            
			_shader.Dispose();
		}
	}
	
	public class Sprite
	{
		Surface bitmap;
		static public Surface target;
		int textureID;
		
		/// <summary>
		/// Constructs a sprite form a spcified filename
		/// </summary>
		/// <param name="fileName">The sprite's filename</param>
		public Sprite( string fileName )
		{
			bitmap = new Surface( fileName );
			textureID = bitmap.GenTexture();
		}
		
		
		/// <summary>
		/// Draw a sprite with scaling
		/// </summary>
		/// <param name="x">The X position where the sprite should be drawn</param>
		/// <param name="y">The Y position where the sprite should be drawn</param>
		/// <param name="scale">The scale at which the sprite should be drawn</param>
		public void Draw( float x, float y, float scale = 1.0f )
		{
			GL.BindTexture( TextureTarget.Texture2D, textureID );
			GL.Enable( EnableCap.Blend );
			GL.BlendFunc( BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha );
			GL.Begin( PrimitiveType.Quads );
			float u1 = (x * 2 - 0.5f * scale * bitmap.Width) / target.Width - 1;
			float v1 = 1 - (y * 2 - 0.5f * scale * bitmap.Height) / target.Height;
			float u2 = ((x + 0.5f * scale * bitmap.Width) * 2) / target.Width - 1;
			float v2 = 1 - ((y + 0.5f * scale * bitmap.Height) * 2) / target.Height;
			GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex2( u1, v2 );
			GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex2( u2, v2 );
			GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex2( u2, v1 );
			GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex2( u1, v1 );
			GL.End();
			GL.Disable( EnableCap.Blend );
		}
	}
}