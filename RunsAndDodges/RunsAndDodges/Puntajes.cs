#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.IO;

#endregion

namespace RunsAndDodges
{
	public class Puntajes : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		int width=920, height=510, i, f, cuentoLineas;
		string resultadoNombre="", resultadoPuntaje="";
		//Fondo
		Texture2D fondo;
		Vector2 coordenadasdelFondo;

		//Fuente
		Vector2 posicionNombre;
		Vector2 posicionPuntaje;
		SpriteFont fuente;

		//Ranking
		public Ranking[] rankingVec;
		Ranking aux;

		public Puntajes()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "../../Content";	            
			graphics.IsFullScreen = false;		
		}

		protected override void Initialize()
		{

			Window.Title = "Puntajes - Runs and Dodges";
			graphics.PreferredBackBufferWidth=width;
			graphics.PreferredBackBufferHeight = height;

			posicionNombre = new Vector2 (290,130);
			posicionPuntaje = new Vector2 (480,130);

			fuente = Content.Load<SpriteFont> ("Fuentes/fuente1");

			coordenadasdelFondo = Vector2.Zero;


			FileStream fileStream = new FileStream ("Ranking.txt", FileMode.OpenOrCreate, FileAccess.Read);
			BinaryReader Leyendo = new BinaryReader (fileStream);

			while (Leyendo.PeekChar() != -1)
			{
				Leyendo.ReadString ();
				Leyendo.ReadInt32 ();
				cuentoLineas++;
			}
			Leyendo.Close ();
			fileStream.Close ();

			rankingVec = new Ranking[cuentoLineas];

			fileStream = new FileStream ("Ranking.txt", FileMode.Open, FileAccess.Read);
			Leyendo = new BinaryReader (fileStream);

			for (i = 0; i < cuentoLineas; i++)
			{
				rankingVec[i].Nombre = Leyendo.ReadString ();
				rankingVec[i].Puntaje = Leyendo.ReadInt32 ();
			}

			for (i = 0; i < (cuentoLineas-1); i++)
				for (f = i+1; f < cuentoLineas; f++)
			{
				if (rankingVec [i].Puntaje < rankingVec [f].Puntaje)
				{
					aux = rankingVec [i];
					rankingVec [i] = rankingVec [f];
					rankingVec [f] = aux;
				}

			}

			for(i = 0; i < cuentoLineas; i++)
			{
				if (i < 10)
				{
					resultadoNombre += String.Format ("{0:} \n",
					                                rankingVec [i].Nombre).ToUpper();
					resultadoPuntaje += String.Format ("{0} \n", rankingVec [i].Puntaje);
				}
			}

			fileStream.Close ();
			Leyendo.Close ();


			base.Initialize();

		}

		protected override void LoadContent()
		{

			spriteBatch = new SpriteBatch(GraphicsDevice);
			fondo = Content.Load<Texture2D> ("Fondo/Fondo-Puntajes");
			fuente = Content.Load<SpriteFont> ("Fuentes/fuente1");

		}



		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin ();
			spriteBatch.Draw (fondo, coordenadasdelFondo, Color.White);
			spriteBatch.DrawString (fuente, resultadoNombre, posicionNombre, Color.DarkGray);
			spriteBatch.DrawString (fuente, resultadoPuntaje, posicionPuntaje, Color.DarkGray);
			spriteBatch.End ();


			base.Draw(gameTime);
		}
	}
}

