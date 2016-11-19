using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio; // Efectos de sonido
using Microsoft.Xna.Framework.Media; //Musica de Fondo

namespace RunsAndDodges
{
	public class Juego : Game
	{
		public static Perdio perdio = new Perdio();
		public static Inicio inicio;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		int i, width=920, height=510, mouseX, mouseY;
		public float elapsedTime=0, elapsedTime2=0, elapsedTime3=0, elapsedTimeSegundos=0;
		bool colision = false;

		MouseState mouseState;
		MouseState prevMouseState;

		//Sonido
		public Song musicadeFondo;
		public SoundEffect efectodeSonido;

		//SALTO
		float gravedad = 0.8f, fuerza = -20;
		bool estadosalto=false;

		//Texto Puntaje y Cartel
		SpriteFont fuente;
		Vector2 coordenadasCartelPuntaje;
		Vector2 coordenadasPuntajeTxt;
		Texture2D cartelPuntaje;

		//Intentos
		public int Intentos=0;
		//Boton Mute
		Texture2D botonMute;
		Vector2 coordenadasBotonMute;
		Rectangle tiraBotonMute = new Rectangle();
		Rectangle[] BotonMuteVec = new Rectangle[2];
		int altoBotonMute, anchoBotonMute;

		//Enemigos
		Texture2D tanque, caja, fuego;
		Vector2 coordenadastanque;
		Vector2 coordenadaTronco;
		Vector2 coordenadasfuego;
		Rectangle tirafuego = new Rectangle();
		Rectangle[] fuegoVec = new Rectangle[5];
		int framesfuego, cantidadFramefuego=5, altofuego, anchofuego;
		Rectangle colisionfuego;
		Rectangle colisiontanque;
		Rectangle colisioncaja;

		//Sprite
		Rectangle spritedelaTira = new Rectangle();
		Rectangle[] spriteVec =  new Rectangle[29];
		Vector2 coordenadasdelsprite;
		Texture2D tiradeSprite;
		int anchoSprite, altoSprite, frames=0, delay=180, cantidadeframe=29;
		Rectangle colisionSprite;

		//Fondo
		Texture2D fondo1, fondo2;
		Vector2 coordenadasfondo1;
		Vector2 coordenadasfondo2;

		//Animaciones
		Texture2D animacionTanque;
		Rectangle[] animacionTanqueVec = new Rectangle[8];
		Rectangle animacionTanqueRec;
		int anchoAnimacionTanque, altoAnimacionTanque, contadorAnimacion, delayAnimacion=400;
		Vector2 coordenadasAnimacionTanque;

		public Juego()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "../../Content";	            
			graphics.IsFullScreen = false;	

		}

		protected override void Initialize()
		{
			Window.Title = "Runs and Dodges";

			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;

			coordenadasfuego = new Vector2 (1750, 230);
			coordenadaTronco = new Vector2 (1500, 367);
			coordenadastanque = new Vector2(1000, 370);
			coordenadasCartelPuntaje = new Vector2 (3,7);
			coordenadasPuntajeTxt = new Vector2(90,31);
			coordenadasBotonMute = new Vector2 (850,21);
			coordenadasdelsprite = new Vector2 (320, 268); 
			coordenadasfondo1 = Vector2.Zero;
			coordenadasfondo2 = new Vector2(920,0);



			base.Initialize ();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			//Efecto de sonido
			efectodeSonido = Content.Load<SoundEffect> ("Sonidos/Salto");
			//Fondo
			fondo1 = Content.Load<Texture2D> ("Fondo/Fondo");
			fondo2 = Content.Load<Texture2D> ("Fondo/Fondo");
			//Tira de Sprite
			tiradeSprite = Content.Load<Texture2D> ("Sprite/Sprite");
			//Texto del puntaje
			fuente = Content.Load<SpriteFont> ("Fuentes/fuente1");
			//Cartel del texto de puntaje
			cartelPuntaje = Content.Load<Texture2D> ("Carteles/Puntaje");
			//Boton Mute
			botonMute = Content.Load<Texture2D> ("Botones/botonMute");
			//tanque
			tanque = Content.Load<Texture2D> ("Colisiones/tanque");
			//caja
			caja = Content.Load<Texture2D> ("Colisiones/caja");
			//fuego
			fuego = Content.Load<Texture2D> ("Colisiones/fuego");
			//Animacion
			animacionTanque = Content.Load<Texture2D> ("Animaciones/animacionTanque");

			//Divido la tira de la animacion del tanque en las posiciones del vector
			anchoAnimacionTanque = animacionTanque.Width / 8;
			altoAnimacionTanque = animacionTanque.Height;

			for (i=0; i<8;i++)
			{
				animacionTanqueVec [i] = new Rectangle (i * anchoAnimacionTanque, 0, anchoAnimacionTanque, altoAnimacionTanque);
			}

			//Divido la tira del sprite en las posiciones del vector
			anchoSprite = tiradeSprite.Width / cantidadeframe;
			altoSprite = tiradeSprite.Height;

			for (i=0; i<29;i++)
			{
				spriteVec [i] = new Rectangle (i * anchoSprite, 0, anchoSprite, altoSprite);
			}
			//Divido la tira del fuego en las posiciones del vector
			anchofuego = fuego.Width / cantidadFramefuego;
			altofuego = fuego.Height;

			for (i=0; i<5; i++) 
			{
				fuegoVec [i] = new Rectangle (i*anchofuego, 0, anchofuego, altofuego );
			}

			//Divido la tira del boton mute en las posiciones del vector
			anchoBotonMute = botonMute.Width / 2;
			altoBotonMute = botonMute.Height;

			for (i=0; i<2; i++) 
			{
				BotonMuteVec [i] = new Rectangle (i*anchoBotonMute, 0, anchoBotonMute, altoBotonMute );
			}
			tiraBotonMute = BotonMuteVec [1];

			//Sonido de fondo
			musicadeFondo = Content.Load<Song> ("Sonidos/Musicafondo");
		    MediaPlayer.Play (musicadeFondo);
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = 1f;



		}

		protected override void Update(GameTime gameTime)
		{



			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
				Exit ();
			}
			elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds; // Tiempo animacion Sprite
			elapsedTime2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds; // Tiempo animacion fuego          
			elapsedTime3 += (float)gameTime.ElapsedGameTime.TotalMilliseconds; // Tiempo animacion Agache
			//Compruebo que no haya colision para tomar el tiempo
			if (colision==false)
			elapsedTimeSegundos += ((float)gameTime.ElapsedGameTime.TotalSeconds); //Tiempo en segundos


			KeyboardState keyboard = Keyboard.GetState ();

			prevMouseState = Mouse.GetState ();
			mouseState = Mouse.GetState ();
			mouseX = mouseState.X;
			mouseY = mouseState.Y;

			//Le paso las coordenadas de los enemigos a los rectangulos de colisiones
			colisionfuego = new Rectangle ((int)coordenadasfuego.X, (int) coordenadasfuego.Y, anchofuego, altofuego);
			colisionSprite = new Rectangle ((int)coordenadasdelsprite.X, (int)coordenadasdelsprite.Y, anchoSprite, altoSprite);
			colisiontanque = new Rectangle ((int)coordenadastanque.X, (int)coordenadastanque.Y, tanque.Width, tanque.Height);
			colisioncaja = new Rectangle ((int)coordenadaTronco.X, (int)coordenadaTronco.Y, caja.Width, caja.Height);

			//Detectando colisiones
			if (colisionfuego.Intersects (colisionSprite))
			{
				colision = true;
			
				perdio= new Perdio ();
				perdio.Puntaje = (int)elapsedTimeSegundos;
				perdio.Run();
			}

			if (colisiontanque.Intersects (colisionSprite)) 
			{  
				colision = true;
				coordenadasAnimacionTanque.X = (int)coordenadastanque.X-150;
				coordenadasAnimacionTanque.Y = (int)coordenadastanque.Y - 240;

				if (elapsedTime >= delayAnimacion) 
				{
					if (contadorAnimacion == 7) 
					{
						perdio= new Perdio ();
						perdio.Puntaje = (int)elapsedTimeSegundos;
						perdio.Run();
					} else 
					{
						contadorAnimacion++;
						animacionTanqueRec= animacionTanqueVec [contadorAnimacion];
					}
					elapsedTime = 0;
				}
			}
			if (colisioncaja.Intersects (colisionSprite))
			{
				colision = true;

				perdio= new Perdio ();
				perdio.Puntaje = (int)elapsedTimeSegundos;
				perdio.Run();
			}


			if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed) 
			{
				if (new Rectangle ((int)coordenadasBotonMute.X, (int)coordenadasBotonMute.Y, 47, 22).Contains (mouseX, mouseY)) 
				{
					MediaPlayer.IsMuted = true;
					tiraBotonMute = BotonMuteVec [0];
				} 

			}

			if (keyboard.IsKeyDown (Keys.Escape))
				this.Exit ();

			if (Convert.ToInt32(elapsedTimeSegundos)>=20) 
			{

					fondo1 = Content.Load<Texture2D> ("Fondo/Fondo-Noche");
					fondo2 = Content.Load<Texture2D> ("Fondo/Fondo-Noche");
			}

			// Movimiento de la tanque
			if (colision==false)
			coordenadastanque.X -= 5;
			if (coordenadastanque.X < -200)
			{
				coordenadastanque.X = 1000;
			}
			//Movimiento del tronco
			if (colision==false)
			coordenadaTronco.X -= 5;
			if (coordenadaTronco.X < -200)
			{
				coordenadaTronco.X = coordenadastanque.X + 500;
			}

			// Movimiento del fuego
			if (colision==false)
			coordenadasfuego.X -= 5;
		    if (coordenadasfuego.X < -200)
			{
			coordenadasfuego.X = coordenadastanque.X+750;
			} 

			//Movimiento de la tira de Sprite
			if (elapsedTime >= delay && colision==false) 
			{
				if (frames == 8) 
				{
					frames = 1;
				} else if (coordenadasdelsprite.Y==268)
				{
					frames++;
					spritedelaTira = spriteVec [frames];

				}
				elapsedTime = 0;
			}
		
			//Animacion del fuego
			if (elapsedTime2 >= delay && colision==false) 
			{
				if (framesfuego == 5) 
				{
					framesfuego = 0;
				} else 
				{

					tirafuego = fuegoVec [framesfuego];
					framesfuego++;
				}
				elapsedTime2 = 0;
			}

			//Movimiento fondo
			if (colision==false)
			{
				coordenadasfondo1.X -= 5;
				if (coordenadasfondo1.X == -920)
				coordenadasfondo1.X = 920;

				coordenadasfondo2.X -= 5;
				if (coordenadasfondo2.X == -920)
				coordenadasfondo2.X = 920;
			}
			//Salto
			if ((Keyboard.GetState ().IsKeyDown (Keys.Up)) && !estadosalto) {
				estadosalto = true;

			}
			if (estadosalto && fuerza < 20)
			{
				coordenadasdelsprite.Y += (fuerza += gravedad);

				if ((elapsedTime >= 0) && (elapsedTime <= delay))
					spritedelaTira = spriteVec [11];

			} else 
			{
				estadosalto = false;
				fuerza = -20;
				//correccion de posicion
				if (coordenadasdelsprite.Y > 200)
					coordenadasdelsprite.Y = 268; //Vuelve por defecto
			}

			//Agacharse
			if ((Keyboard.GetState ().IsKeyDown (Keys.Down) && !estadosalto)) 
			{
				coordenadasdelsprite.Y += 39;

					spritedelaTira = spriteVec [25];

			}
				
			//Detectando que se presionan dos teclas a la vez
			if (Keyboard.GetState ().IsKeyDown (Keys.Down) && (Keyboard.GetState ().IsKeyDown (Keys.Up))) 
			{
				coordenadasdelsprite.Y = 258;
			}

			base.Update (gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{

			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin ();
			spriteBatch.Draw (fondo1, coordenadasfondo1, Color.White);
			spriteBatch.Draw (fondo2, coordenadasfondo2, Color.White);
			spriteBatch.Draw (cartelPuntaje, coordenadasCartelPuntaje, Color.White);
			spriteBatch.DrawString (fuente, "" + Convert.ToInt32(elapsedTimeSegundos), coordenadasPuntajeTxt, Color.White);
			spriteBatch.Draw (tiradeSprite, coordenadasdelsprite,spritedelaTira, Color.White);
			spriteBatch.Draw (botonMute, coordenadasBotonMute, tiraBotonMute, Color.White);
			spriteBatch.Draw (tanque, coordenadastanque, Color.White);
			spriteBatch.Draw (caja, coordenadaTronco, Color.White);
			spriteBatch.Draw (fuego, coordenadasfuego, tirafuego, Color.White);
			spriteBatch.Draw (animacionTanque,coordenadasAnimacionTanque, animacionTanqueRec, Color.White);
			spriteBatch.End ();

			base.Draw(gameTime);
		}
	}
}





