using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Ui.Logic;
using Newtonsoft.Json.Serialization;

namespace MonoUtils;

public class ConnectedGameObject : GameObject
{
    private enum OnTextureRef
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Center,
        Right,
        BottomLeft,
        Bottom,
        BottomRight,
        CornerTopLeft,
        CornerTopRight,
        CornerBottomLeft,
        CornerBottomRight
    }

    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;

    public new static Texture2D DefaultTexture;

    private List<Vector2> ImageLocations;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(32, 32),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 32, 32)
        }
    };

    public ConnectedGameObject()
    {
    }

    public ConnectedGameObject(Vector2 position) : this(position, 1F)
    {
    }

    public ConnectedGameObject(Vector2 position, float scale) : this(position, scale * DefaultSize)
    {
    }

    public ConnectedGameObject(Vector2 position, Vector2 size) : this(position, size, DefaultTexture, DefaultMapping)
    {
    }

    public ConnectedGameObject(Vector2 position, Vector2 size, Texture2D texture, TextureHitboxMapping mapping) : base(
        position, size, texture, mapping)
    {
        ImageLocations = new List<Vector2>();
        ImageLocations.Add(GetImageLocation(OnTextureRef.Center));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var imageLocation in ImageLocations)
        {
            MoveImageLocation(imageLocation * ImageLocation.Size.ToVector2());
            base.Draw(spriteBatch);
        }
    }

    private Vector2 GetImageLocation(OnTextureRef onTexture)
        => onTexture switch
        {
            OnTextureRef.TopLeft => new Vector2(0, 0),
            OnTextureRef.Top => new Vector2(1, 0),
            OnTextureRef.TopRight => new Vector2(2, 0),
            OnTextureRef.Left => new Vector2(0, 1),
            OnTextureRef.Right => new Vector2(2, 1),
            OnTextureRef.BottomLeft => new Vector2(0, 2),
            OnTextureRef.Bottom => new Vector2(1, 2),
            OnTextureRef.BottomRight => new Vector2(2, 2),
            OnTextureRef.CornerTopLeft => new Vector2(3, 1),
            OnTextureRef.CornerTopRight => new Vector2(4, 1),
            OnTextureRef.CornerBottomLeft => new Vector2(3, 2),
            OnTextureRef.CornerBottomRight => new Vector2(4, 2),
            _ => new Vector2(1, 1)
        };

    public void SetTextureLocation(List<ConnectedGameObject> surroundingObjects)
    {
        ImageLocations = new List<Vector2>();
        Vector2 mainImageLocation = Vector2.Zero;
        List<Vector2> imageLocations = new List<Vector2>();

        bool[] values = new bool[8];
        foreach (var surroundingObject in surroundingObjects)
        {
            var rectangle = surroundingObject.Rectangle;

            if (Rectangle.Top != rectangle.Bottom &&
                Rectangle.Bottom != rectangle.Top &&
                Rectangle.Left != rectangle.Right &&
                Rectangle.Right != rectangle.Left)
                continue;

            if (Rectangle.TopLeftCorner() == rectangle.BottomRightCorner())
                values[0] = true;
            else if (Rectangle.TopRightCorner() == rectangle.BottomLeftCorner())
                values[2] = true;
            else if (Rectangle.BottomLeftCorner() == rectangle.TopRightCorner())
                values[5] = true;
            else if (Rectangle.BottomRightCorner() == rectangle.TopLeftCorner())
                values[7] = true;

            if (Rectangle.Left == rectangle.Left &&
                Rectangle.Right == rectangle.Right)
            {
                if (Rectangle.Top == rectangle.Bottom)
                    values[1] = true;
                else if (Rectangle.Bottom == rectangle.Top)
                    values[6] = true;
            }

            if (Rectangle.Top == rectangle.Top &&
                Rectangle.Bottom == rectangle.Bottom)
            {
                if (Rectangle.Left == rectangle.Right)
                    values[3] = true;
                else if (Rectangle.Right == rectangle.Left)
                    values[4] = true;
            }
        }

        mainImageLocation = GetImageLocation(OnTextureRef.Center);

        bool topLeft = !values[0];
        bool top = !values[1];
        bool topRight = !values[2];
        bool left = !values[3];
        bool right = !values[4];
        bool bottomLeft = !values[5];
        bool bottom = !values[6];
        bool bottomRight = !values[7];



        if (left)
            mainImageLocation = GetImageLocation(OnTextureRef.Left);

        if (right)
            mainImageLocation = GetImageLocation(OnTextureRef.Right);

        if (top)
            mainImageLocation = GetImageLocation(OnTextureRef.Top);

        if (bottom)
            mainImageLocation = GetImageLocation(OnTextureRef.Bottom);


        if (topLeft)
        {
            if (left && top)
                mainImageLocation = GetImageLocation(OnTextureRef.TopLeft);
            else if (!left && !top)
                imageLocations.Add(GetImageLocation(OnTextureRef.CornerBottomRight));
        }

        if (topRight)
        {
            if (right && top)
                mainImageLocation = GetImageLocation(OnTextureRef.TopRight);
            else if (!right && !top)
                imageLocations.Add(GetImageLocation(OnTextureRef.CornerBottomLeft));
        }

        if (bottomLeft)
        {
            if (left && bottom)
                mainImageLocation = GetImageLocation(OnTextureRef.BottomLeft);
            else if (!left && !bottom)
                imageLocations.Add(GetImageLocation(OnTextureRef.CornerTopRight));
        }

        if (bottomRight)
        {
            if (right && bottom)
                mainImageLocation = GetImageLocation(OnTextureRef.BottomRight);
            else if (!right && !bottom)
                imageLocations.Add(GetImageLocation(OnTextureRef.CornerTopLeft));
        }

        ImageLocations.Add(mainImageLocation);
        ImageLocations.AddRange(imageLocations);
    }
}