using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;

namespace MonoUtils.Ui.Objects.TextSystem;

public class Text : IColorable, IMoveable, IManageable
{
    private List<Letter> _letters;
    protected readonly int Spacing;
    private readonly float _letterScale;
    public Vector2 Position;
    public Vector2 Size;
    public Rectangle Rectangle { get; private set; }

    public Letter this[int i] => _letters[i];

    public List<Letter> Letters => _letters;

    public int Length => _letters.Count;

    public static float DefaultLetterScale => 2F;

    public Text(string text) : this(text, Vector2.Zero, DefaultLetterScale, 1)
    {

    }

    public Text(string text, float scale) : this(text, Vector2.Zero, scale * DefaultLetterScale, 1)
    {
    }

    public Text(string text, Vector2 position) : this(text, position, DefaultLetterScale, 1)
    {
    }

    public Text(string text, Vector2 position, float scale) : this(text, position, scale * DefaultLetterScale, 1)
    {
    }

    public Text(string text, Vector2 position, float scale, int spacing)
    {
        Spacing = spacing;
        _letterScale = scale;
        Position = position;
        ChangeText(text);
    }

    public void ChangeText(string text)
    {
        var letters = Letter.Parse(text, _letterScale);

        int length = 0;
        foreach (var letter in letters)
        {
            var position = Position;
            position.X += length;
            letter.Move(position + new Vector2(0, letter.FullSize.Y) - new Vector2(0, letter.Rectangle.Height));
            length += (int)(letter.Size.X + Spacing * _letterScale);
        }

        _letters = letters;
        UpdateRectangle();
    }


    public virtual void Update(GameTime gameTime)
    {
        foreach (var letter in _letters)
        {
            letter.Update(gameTime);
        }

        UpdateRectangle();
    }

    private void UpdateRectangle()
    {
        Rectangle combination = Rectangle.Empty;
        foreach (Letter letter in _letters)
        {
            Rectangle rec = letter.Rectangle;
            if (combination.IsEmpty)
                combination = letter.Rectangle;
            else
                Rectangle.Union(ref combination, ref rec, out combination);
        }

        Rectangle = combination;
        Size = Rectangle.Size.ToVector2();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var letter in _letters)
        {
            letter.Draw(spriteBatch);
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var letter in _letters)
        {
            builder.Append(letter);
        }
        return builder.ToString();
    }

    public virtual Vector2 GetPosition()
        => Position;

    public virtual Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
    {
        Vector2 offset = newPosition - Position;
        foreach (Letter letter in _letters)
            letter.Move(letter.Position + offset);
        Position = newPosition;
        UpdateRectangle();
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] color)
    {
        for (int i = 0; i < color.Length; i++)
        {
            if (_letters.Count > i)
                _letters[i].DrawColor = color[i];
        }
    }

    public int ColorLength()
        => Length;

    public Microsoft.Xna.Framework.Color[] GetColor()
    =>  (Microsoft.Xna.Framework.Color[])_letters.Select(l => l.DrawColor);

    public void ChangeColor(Microsoft.Xna.Framework.Color color)
    {
        for (int i = 0; i < _letters.Count; i++)
        {
            _letters[i].DrawColor = color;
        }
    }
}