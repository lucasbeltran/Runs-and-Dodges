#region Using Statements
using System;
using RunsAndDodges;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace RunsAndDodges
{
	 public class Inicio : Game
    {
		public static Juego game;
		public static Inicio inicio;
		public static Puntajes puntaje;


		int width=920, height=510, mouseX, mouseY, cantidadframe=2, altosprite;
		int anchosprite;
		float elapsedtime=0;
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;		

		Song sonidoFondo;

		public SoundEffect sonidoBoton;


		Texture2D fondo;
		Texture2D jugarBoton;
		Texture2D puntajesBoton;

		Rectangle spritedeJugar = new Rectangle ();
		Rectangle spritedePuntajes = new Rectangle ();

	
		Rectangle[] spriteVecJugar = new Rectangle[2];
		Rectangle[] spriteVecPuntajes = new Rectangle[2];

		Vector2 coordenadasjugarBoton;
		Vector2 coordenadaspuntajesBoton;
		Vector2 coordenadasdelfondo;


		MouseState mouseState;
		MouseState prevMouseState;

		public Inicio()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "../../Content";	            
			graphics.IsFullScreen = false;		
		}

	
		protected override void Initialize()
		{
			Window.Title = "Runs and Dodges";

			graphics.PreferredBackBufferWidth=width;
			graphics.PreferredBackBufferHeight = height;

			coordenadasdelfondo = Vector2.Zero;
			coordenadasjugarBoton = new Vector2 (240, 120);
			coordenadaspuntajesBoton = new Vector2 (462, 120);

			base.Initialize();

		}

		protected override void LoadContent()
		{
		
			spriteBatch = new SpriteBatch(GraphicsDevice);


			fondo = Content.Load<Texture2D> ("Fondo/Fondo-Inicio");
			jugarBoton = Content.Load<Texture2D> ("Botones/Jugarboton1");
			puntajesBoton = Content.Load<Texture2D> ("Botones/Puntajesboton1");

			sonidoFondo = Content.Load<Song> ("Sonidos/SonidoFondo.wav");


			MediaPlayer.Play (sonidoFondo);
			MediaPlayer.IsRepeating = true;
			sonidoBoton = Content.Load<SoundEffect> ("Sonidos/Boton.wav");

			anchosprite = puntajesBoton.Width / cantidadframe;
			altosprite = puntajesBoton.Height;

			for (int i=0; i<2; i++)
			{
				spriteVecJugar [i] = new Rectangle (i * anchosprite, 0, anchosprite, altosprite);
				spriteVecPuntajes [i] = new Rectangle (i * anchosprite, 0, anchosprite, altosprite);


			}


			spritedeJugar = spriteVecJugar [0];
			spritedePuntajes = spriteVecPuntajes [0];

		}
		protected override void Update(GameTime gameTime)
		{
			elapsedtime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			prevMouseState = Mouse.GetState ();
			mouseState = Mouse.GetState ();

			mouseX = mouseState.X;
			mouseY = mouseState.Y;

			//Detecto si se encuentra en el area del boton para animacion
			if (new Rectangle ((int)coordenadasjugarBoton.X, (int)coordenadasjugarBoton.Y, 206, 242).Contains (mouseX, mouseY)) 
			{
				spritedeJugar = spriteVecJugar [1];
			} else {
				spritedeJugar = spriteVecJugar [0];
			}

		
			if (new Rectangle ((int)coordenadaspuntajesBoton.X, (int)coordenadaspuntajesBoton.Y, 206, 242).Contains (mouseX, mouseY)) 
			{
				spritedePuntajes = spriteVecPuntajes [1];
			} else {
				spritedePuntajes = spriteVecPuntajes [0];
			}
			//Boton presionado
			if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed) 
			{
				if(new Rectangle((int)coordenadasjugarBoton.X, (int)coordenadasjugarBoton.Y, 206, 242).Contains(mouseX, mouseY))
				   {
						sonidoBoton.Play ();
						game = new Juego ();
					    game.Run ();
													
				
				 }
				else if(new Rectangle((int)coordenadaspuntajesBoton.X, (int)coordenadaspuntajesBoton.Y, 206, 242).Contains(mouseX, mouseY))
				{

					sonidoBoton.Play ();
					puntaje = new Puntajes ();
					puntaje.Run ();
				}
				   
			}

			KeyboardState keyboard = Keyboard.GetState ();

			if (keyboard.IsKeyDown (Keys.Escape))
				this.Exit ();
		
			base.Update(gameTime);
		}
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin ();
			spriteBatch.Draw (fondo, coordenadasdelfondo, Color.White);
			spriteBatch.Draw (jugarBoton, coordenadasjugarBoton, spritedeJugar,  Color.White);
			spriteBatch.Draw (puntajesBoton, coordenadaspuntajesBoton, spritedePuntajes ,Color.White);
			spriteBatch.End ();

			base.Draw(gameTime);
		}
	}
}

