using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Ui.Logic;
using Newtonsoft.Json.Serialization;

namespace MonoUtils;

public class ConnectedGameObject : GameObject
{
    public enum OnTextureReference
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Right,
        BottomLeft,
        Bottom,
        BottomRight,
        CornerTopLeft,
        CornerTopRight,
        CornerBottomLeft,
        CornerBottomRight,
        Center,
        VariationCenter1,
        VariationCenter2
    }

    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;

    public new static Texture2D DefaultTexture;

    private List<Vector2> _imageLocations;
    private int _variation = 0;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(32, 32),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 32, 32)
        }
    };

    public ConnectedGameObject(int variation = 0): this(Vector2.Zero, variation)
    {
    }

    public ConnectedGameObject(Vector2 position, int variation) : this(position, 1F, variation)
    {
    }

    public ConnectedGameObject(Vector2 position, float scale, int variation) : this(position, scale * DefaultSize, variation)
    {
    }

    public ConnectedGameObject(Vector2 position, Vector2 size, int variation) : this(position, size, variation, DefaultTexture, DefaultMapping)
    {
    }

    public ConnectedGameObject(Vector2 position, Vector2 size, int variation, Texture2D texture, TextureHitboxMapping mapping) : base(
        position, size, texture, mapping)
    {
        _imageLocations = new List<Vector2>();
        _imageLocations.Add(GetImageLocation(OnTextureReference.Center + variation));
        _variation = variation;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var imageLocation in _imageLocations)
        {
            MoveImageLocation(imageLocation * ImageLocation.Size.ToVector2());
            base.Draw(spriteBatch);
        }
    }

    private Vector2 GetImageLocation(OnTextureReference onTexture)
        => onTexture switch
        {
            OnTextureReference.TopLeft => new Vector2(0, 0),
            OnTextureReference.Top => new Vector2(1, 0),
            OnTextureReference.TopRight => new Vector2(2, 0),
            OnTextureReference.Left => new Vector2(0, 1),
            OnTextureReference.Right => new Vector2(2, 1),
            OnTextureReference.BottomLeft => new Vector2(0, 2),
            OnTextureReference.Bottom => new Vector2(1, 2),
            OnTextureReference.BottomRight => new Vector2(2, 2),
            OnTextureReference.CornerTopLeft => new Vector2(3, 1),
            OnTextureReference.CornerTopRight => new Vector2(4, 1),
            OnTextureReference.CornerBottomLeft => new Vector2(3, 2),
            OnTextureReference.CornerBottomRight => new Vector2(4, 2),
            OnTextureReference.VariationCenter1 =>  new Vector2(3,0),
            OnTextureReference.VariationCenter2 =>  new Vector2(4,0),
            _ => new Vector2(1, 1)
        };

    public void SetTextureLocation(List<ConnectedGameObject> surroundingObjects)
    {
        _imageLocations = new List<Vector2>();
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

        mainImageLocation = GetImageLocation(OnTextureReference.Center + _variation);

        bool topLeft = !values[0];
        bool top = !values[1];
        bool topRight = !values[2];
        bool left = !values[3];
        bool right = !values[4];
        bool bottomLeft = !values[5];
        bool bottom = !values[6];
        bool bottomRight = !values[7];



        if (left)
            mainImageLocation = GetImageLocation(OnTextureReference.Left);

        if (right)
            mainImageLocation = GetImageLocation(OnTextureReference.Right);

        if (top)
            mainImageLocation = GetImageLocation(OnTextureReference.Top);

        if (bottom)
            mainImageLocation = GetImageLocation(OnTextureReference.Bottom);


        if (topLeft)
        {
            if (left && top)
                mainImageLocation = GetImageLocation(OnTextureReference.TopLeft);
            else if (!left && !top)
                imageLocations.Add(GetImageLocation(OnTextureReference.CornerBottomRight));
        }

        if (topRight)
        {
            if (right && top)
                mainImageLocation = GetImageLocation(OnTextureReference.TopRight);
            else if (!right && !top)
                imageLocations.Add(GetImageLocation(OnTextureReference.CornerBottomLeft));
        }

        if (bottomLeft)
        {
            if (left && bottom)
                mainImageLocation = GetImageLocation(OnTextureReference.BottomLeft);
            else if (!left && !bottom)
                imageLocations.Add(GetImageLocation(OnTextureReference.CornerTopRight));
        }

        if (bottomRight)
        {
            if (right && bottom)
                mainImageLocation = GetImageLocation(OnTextureReference.BottomRight);
            else if (!right && !bottom)
                imageLocations.Add(GetImageLocation(OnTextureReference.CornerTopLeft));
        }

        _imageLocations.Add(mainImageLocation);
        _imageLocations.AddRange(imageLocations);
    }
}