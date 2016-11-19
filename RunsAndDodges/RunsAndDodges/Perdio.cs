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
	//Estructura del registro donde se ubica el ranking
	public struct Ranking
	{
		public string Nombre;
		public int Puntaje;
	}


	public class Perdio : Game
	{
		public static Juego perdio= new Juego();
		public static Puntajes puntajes;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;		

		Ranking ListaPuntaje = new Ranking();

		public int i, Puntaje,  width=920, height=510, anchoGuardar, altoGuardar, anchoDeNuevo,altoDeNuevo, mouseY, mouseX;

		//Error
		string error="";
		Color colorError;

		Vector2 posicionError;
		//Fondo
		Texture2D fondo;
		Vector2 coordenadasfondo;

		//Mouse
		MouseState mouseState;
		MouseState prevMouseState;

		//Fuente
		Vector2 posicionFuente;
		SpriteFont fuente;

		//Muestra puntaje
		Vector2 posicionPuntaje;

		//Boton guardar
		Texture2D botonGuardar;
		Vector2 coordenadasbotonGuardar;
		Rectangle spritedebotonGuardar = new Rectangle ();
		Rectangle[] spriteVecbotonGuardar = new Rectangle[2];

		//Boton de nuevo
		Texture2D botonDeNuevo;
		Vector2 coordenadasbotonDeNuevo;
		Rectangle spritedebotonDeNuevo = new Rectangle();
		Rectangle[] spriteVecbotonDeNuevo = new Rectangle[2];


		//Introduccion de texto
		string text="";

		Keys[] keysToCheck = new Keys[] { 
			Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
			Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
			Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
			Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
			Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
			Keys.Z, Keys.Back, Keys.Space };

		KeyboardState currentKeyboardState;
		KeyboardState lastKeyboardState;


		public Perdio()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "../../Content";	            
			graphics.IsFullScreen = false;	
		}


		protected override void Initialize()
		{
			Window.Title = "Has perdido - Runs and Dodges";

			graphics.PreferredBackBufferWidth=width;
			graphics.PreferredBackBufferHeight = height;

			coordenadasfondo = Vector2.Zero;
			coordenadasbotonGuardar = new Vector2 ((float)462.9, 300);
			coordenadasbotonDeNuevo = new Vector2 ((float)319.5, 300);
			posicionError = new Vector2 (360, 275);
			posicionFuente = new Vector2 (378,245);
			posicionPuntaje = new Vector2 (457, 156);

			base.Initialize();

		}

		protected override void LoadContent()
		{

			spriteBatch = new SpriteBatch(GraphicsDevice);

			fondo = Content.Load<Texture2D> ("Fondo/Fondo-Perdio");
			botonGuardar = Content.Load<Texture2D> ("Botones/Guardarboton");
			botonDeNuevo = Content.Load<Texture2D> ("Botones/BotonDeNuevo");
			fuente = Content.Load<SpriteFont> ("Fuentes/fuente1");

			anchoGuardar = botonGuardar.Width / 2;
			altoGuardar = botonGuardar.Height;

			for (i=0; i<2; i++)
			{
				spriteVecbotonGuardar [i] = new Rectangle (i * anchoGuardar, 0, anchoGuardar, altoGuardar);

			}
			spritedebotonGuardar = spriteVecbotonGuardar [0];

			anchoDeNuevo = botonDeNuevo.Width / 2;
			altoDeNuevo = botonDeNuevo.Height;

			for (i=0; i<2; i++)
			{
				spriteVecbotonDeNuevo[i] =  new Rectangle(i* anchoDeNuevo, 0, anchoDeNuevo, altoDeNuevo);
			}
			spritedebotonDeNuevo = spriteVecbotonDeNuevo[0];
		
		}


		protected override void Update(GameTime gameTime)
		{


			//Tomo el estado del mouse
			prevMouseState = Mouse.GetState ();
			mouseState = Mouse.GetState ();

			mouseX = mouseState.X;
			mouseY = mouseState.Y;

			//Compruebo si se pasa el mouse por el boton de guardar
			if (new Rectangle ((int)coordenadasbotonGuardar.X, (int)coordenadasbotonGuardar.Y, botonGuardar.Width/2, botonGuardar.Height).Contains (mouseX, mouseY)) 
			{
				spritedebotonGuardar = spriteVecbotonGuardar [1];
			} else {
				spritedebotonGuardar = spriteVecbotonGuardar [0];
			}

			//Compruebo si se pasa el mouse por el boton de nuevo
			if (new Rectangle ((int)coordenadasbotonDeNuevo.X, (int)coordenadasbotonDeNuevo.Y, botonDeNuevo.Width/2, botonDeNuevo.Height).Contains (mouseX, mouseY)) 
			{
				spritedebotonDeNuevo = spriteVecbotonDeNuevo [1];
			} else {
				spritedebotonDeNuevo = spriteVecbotonDeNuevo [0];
			}

			//Boton guardar presionado
			if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed) 
			{
				if (new Rectangle ((int)coordenadasbotonGuardar.X, (int)coordenadasbotonGuardar.Y, botonGuardar.Width/2, botonGuardar.Height).Contains (mouseX, mouseY)) 
				{
					//Carga del ranking
					ListaPuntaje.Nombre = text + "";
					ListaPuntaje.Puntaje = Puntaje;

					//Compruebo si se ingreso el nombre y tiene mas de cuatro caracteres
					if (ListaPuntaje.Nombre == "" || ListaPuntaje.Nombre.Length < 4)
					{
						error = "Nombre Incorrecto";
						colorError = Color.Red;
					} else 
					{
						error = "Guardado con exito";
						colorError = Color.Green;

						BinaryWriter escribe = new BinaryWriter(File.Open("Ranking.txt", FileMode.Append));
						//Escritura al archivo de texto
						escribe.Write (ListaPuntaje.Nombre);
						escribe.Write (ListaPuntaje.Puntaje);
						escribe.Close ();

						puntajes = new Puntajes ();
						puntajes.Run ();

					}
				
				}

			}

			//Boton de nuevo presionado
			if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed) 
			{
				if (new Rectangle ((int)coordenadasbotonDeNuevo.X, (int)coordenadasbotonDeNuevo.Y, botonDeNuevo.Width/2,botonDeNuevo.Height).Contains (mouseX, mouseY)) 
				{

						Inicio.game.elapsedTimeSegundos = 0;
					    perdio = new Juego ();
						perdio.Run ();


				}

			}

			//Lectura de caracteres
			currentKeyboardState = Keyboard.GetState();

			foreach (Keys key in keysToCheck)
			{
				if (CheckKey(key))
				{
					AddKeyToText(key);
					break;
				}
			}

			base.Update(gameTime);

			lastKeyboardState = currentKeyboardState;
		}
		private void AddKeyToText(Keys key)
		{
			string newChar = "";

			if (text.Length >= 15 && key != Keys.Back)
				return;

			switch (key)
			{
				case Keys.A:
				newChar += "a";
				break;
				case Keys.B:
				newChar += "b";
				break;
				case Keys.C:
				newChar += "c";
				break;
				case Keys.D:
				newChar += "d";
				break;
				case Keys.E:
				newChar += "e";
				break;
				case Keys.F:
				newChar += "f";
				break;
				case Keys.G:
				newChar += "g";
				break;
				case Keys.H:
				newChar += "h";
				break;
				case Keys.I:
				newChar += "i";
				break;
				case Keys.J:
				newChar += "j";
				break;
				case Keys.K:
				newChar += "k";
				break;
				case Keys.L:
				newChar += "l";
				break;
				case Keys.M:
				newChar += "m";
				break;
				case Keys.N:
				newChar += "n";
				break;
				case Keys.O:
				newChar += "o";
				break;
				case Keys.P:
				newChar += "p";
				break;
				case Keys.Q:
				newChar += "q";
				break;
				case Keys.R:
				newChar += "r";
				break;
				case Keys.S:
				newChar += "s";
				break;
				case Keys.T:
				newChar += "t";
				break;
				case Keys.U:
				newChar += "u";
				break;
				case Keys.V:
				newChar += "v";
				break;
				case Keys.W:
				newChar += "w";
				break;
				case Keys.X:
				newChar += "x";
				break;
				case Keys.Y:
				newChar += "y";
				break;
				case Keys.Z:
				newChar += "z";
				break;
				case Keys.Space:
				newChar += " ";
				break;
				case Keys.Back:
				if (text.Length != 0)
					text = text.Remove(text.Length - 1);
				return;
			}
			if (currentKeyboardState.IsKeyDown(Keys.RightShift) ||
			    currentKeyboardState.IsKeyDown(Keys.LeftShift))
			{
				newChar = newChar.ToUpper();
			}
			text += newChar;
		}

		private bool CheckKey(Keys theKey)
		{
			return lastKeyboardState.IsKeyDown(theKey) && currentKeyboardState.IsKeyUp(theKey);
		}



		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin ();
			spriteBatch.Draw (fondo, coordenadasfondo, Color.White);
			spriteBatch.DrawString (fuente, text, posicionFuente, Color.DarkGray);
			spriteBatch.DrawString (fuente, error, posicionError, colorError);
			spriteBatch.DrawString (fuente, "" + Convert.ToInt32(Puntaje), posicionPuntaje, Color.DarkGray);
			spriteBatch.Draw (botonDeNuevo, coordenadasbotonDeNuevo, spritedebotonDeNuevo, Color.White);
			spriteBatch.Draw (botonGuardar, coordenadasbotonGuardar, spritedebotonGuardar, Color.White);
			spriteBatch.End ();

			//TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}

