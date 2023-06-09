﻿using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;

namespace MonoUtils.Ui.Objects.TextSystem;

public class Text : IColorable, IMoveable, IManageable
{
    private List<Letter> _letters;
    protected readonly int Spacing;
    private string _represent;
    public Vector2 Position;
    public Vector2 Size;
    public Rectangle Rectangle { get; private set; }
    public string Value => _represent;

    public Letter this[int i] => _letters[i];

    public int Length => _letters.Count;

    public static Vector2 DefaultLetterSize => new Vector2(16, 16);

    public Text(string text) : this(text, Vector2.Zero, 1, 1)
    {
    }

    public Text(string text, float scale) : this(text, Vector2.Zero, DefaultLetterSize * scale, 1)
    {
    }

    public Text(string text, Vector2 position) : this(text, position, DefaultLetterSize, 1)
    {
    }

    public Text(string text, Vector2 position, float scale) : this(text, position, DefaultLetterSize * scale, 1)
    {
    }

    public Text(string text, Vector2 position, float scale, int spacing) : this(text, position,
        DefaultLetterSize * scale, spacing)
    {
    }

    public Text(string text, Vector2 position, Vector2 letterSize, int spacing)
    {
        Spacing = spacing;
        Size = letterSize;
        Position = position;

        ChangeText(text);
    }

    private void ChangePosition(Vector2 newPosition)
    {
        foreach (Letter letter in _letters)
            letter.Move(letter.Position + (newPosition - Position));
        Position = newPosition;
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] color)
    {
        for (int i = 0; i < color.Length; i++)
        {
            if (_letters.Count > i)
                _letters[i].ChangeColor(color[i]);
        }
    }

    public int ColorLength()
        => Length;

    public void ChangeColor(Microsoft.Xna.Framework.Color color)
    {
        for (int i = 0; i < _letters.Count; i++)
        {
            _letters[i].ChangeColor(color);
        }
    }

    public void ChangeText(string text)
    {
        string input = PrepareText(text);
        _represent = input;
        CreateLetters(ParseArray(input.ToCharArray()));
    }

    public void AppendText(string text)
    {
        ChangeText(_represent + text);
    }

    private void CreateLetters(Letter.Character[] characters)
    {
        var letters = new List<Letter>();

        int length = 0;
        float sizeScale = Size.X / 8;
        foreach (Letter.Character character in characters)
        {
            var letter = new Letter(new Vector2(length, 0) + Position, Size, character);
            letter.Move(letter.Position + new Vector2(0, 8F * letter.InitialScale.Y) - new Vector2(0, letter.Rectangle.Height));
            length += (int) ((letter.FrameSpacing.Width + Spacing) * sizeScale);
            letters.Add(letter);
        }

        _letters = letters;
        UpdateRectangle();
    }

    private Letter.Character[] ParseArray(char[] text)
        => text.Select(Letter.Parse).ToArray();

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
    }

    public override string ToString()
        => BuildString();

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var letter in _letters)
        {
            letter.Draw(spriteBatch);
        }
    }

    private string BuildString()
    {
        string build = string.Empty;

        foreach (var letter in _letters)
        {
            build += Letter.ReverseParse(letter.RepresentingCharacter);
        }

        return build;
    }

    public virtual Vector2 GetPosition()
        => Position;

    public virtual Vector2 GetSize()
        => Rectangle.Size.ToVector2();

    public void Move(Vector2 newPosition)
    {
        ChangePosition(newPosition);
        UpdateRectangle();
    }

    private string PrepareText(string input)
    {
        StringBuilder result = new StringBuilder();

        foreach (char c in input)
        {
            if (c == '\b')
            {
                if (result.Length > 0)
                    result.Remove(result.Length - 1, 1);
            }
            else
                result.Append(c);
        }

        return result.ToString();
    }
}