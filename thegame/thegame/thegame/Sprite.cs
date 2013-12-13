using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace thegame
{


    public class Sprite
    {
        /// <summary>
        /// Récupère ou définit l'image du sprite
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        protected Texture2D texture;

        /// <summary>
        /// Récupère ou définit la position du Sprite
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        protected Vector2 position;

        /// <summary>
        /// Récupère ou définit la direction du sprite. Lorsque la direction est modifiée, elle est automatiquement normalisée.
        /// </summary>
        public Vector2 Direction
        {
            get { return direction; }
            set { direction = Vector2.Normalize(value); }
        }
        protected Vector2 direction;

        /// <summary>
        /// Récupère ou définit la vitesse de déplacement du sprite.
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        protected float speed;


        /// <summary>
        /// Initialise les variables du Sprite
        /// </summary>
        public virtual void Initialize()
        {
             position = new Vector2 (0,0);
             direction = new Vector2 (1,0);
             speed = 0.2f;
        }

        /// <summary>
        /// Charge l'image voulue grâce au ContentManager donné
        /// </summary>
        /// <param name="content">Le ContentManager qui chargera l'image</param>
        /// <param name="assetName">L'asset name de l'image à charger pour ce Sprite</param>
        public virtual void LoadContent(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
        }

        /// <summary>
        /// Met à jour les variables du sprite
        /// </summary>
        /// <param name="gameTime">Le GameTime associé à la frame</param>
        public virtual void Update(KeyboardState keyboardState, MouseState mouseState)
        {
        }


        /// <summary>
        /// Dessine le sprite en utilisant ses attributs et le spritebatch donné
        /// </summary>
        /// <param name="spriteBatch">Le spritebatch avec lequel dessiner</param>
        /// <param name="gameTime">Le GameTime de la frame</param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
