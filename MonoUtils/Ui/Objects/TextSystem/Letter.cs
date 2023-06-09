﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;

namespace MonoUtils.Ui.Objects.TextSystem;

public class Letter : GameObject, IMoveable
{
    public const char Full = '⬜';
    public const char LockLocked = '\u229E';
    public const char LockUnlocked = '\u229F';
    public const char AmongUsBean = 'ඞ';
    public const char Checkmark = '✔';
    public const char Crossout = '❌';
    public const char Bean = '\u22a0';

    public Rectangle FrameSpacing;
    public Character RepresentingCharacter;
    public Vector2 InitialScale { get; private set; }
    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(8, 8),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 8, 8)
        }
    };

    public Letter(Vector2 position, Vector2 size, Character character) : base(position, size,
        DefaultTexture, DefaultMapping)
    {
        InitialScale = size / DefaultMapping.ImageSize;
        RepresentingCharacter = character;
        TextureHitboxMapping.Hitboxes = new[]
        {
            new Rectangle((position + FrameSpacing.Location.ToVector2()).ToPoint(),
                (FrameSpacing.Size.ToVector2() * ScaleToTexture).ToPoint())
        };
        UpdateCharacter(character);
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color color)
    {
        DrawColor = color;
    }

    public void UpdateCharacter(Character character)
    {
        FrameSpacing = GetCharacterSpacing(character);
        var inImagePosition = new Rectangle(new Point((int) character % 5 * 8, (int) character / 5 * 8),
            (FrameSize).ToPoint());
        ImageLocation = new Rectangle(
            inImagePosition.X + FrameSpacing.X
            , inImagePosition.Y + FrameSpacing.Y
            , FrameSpacing.Width
            , FrameSpacing.Height
        );
        Size = FrameSpacing.Size.ToVector2() * InitialScale;
        UpdateRectangle();
        RepresentingCharacter = character;
    }

    private static Rectangle GetCharacterSpacing(Character character)
    {
        return character switch
        {
            Character.Zero => new Rectangle(1, 0, 5, 8),
            Character.One => new Rectangle(1, 0, 4, 8),
            Character.Two => new Rectangle(1, 0, 5, 8),
            Character.Three => new Rectangle(1, 0, 4, 8),
            Character.Four => new Rectangle(1, 0, 5, 8),
            Character.Five => new Rectangle(1, 0, 6, 8),
            Character.Six => new Rectangle(1, 0, 5, 8),
            Character.Seven => new Rectangle(1, 0, 6, 8),
            Character.Eight => new Rectangle(1, 0, 5, 8),
            Character.Nine => new Rectangle(1, 0, 5, 8),
            Character.BigA => new Rectangle(1, 0, 5, 8),
            Character.BigB => new Rectangle(1, 0, 5, 8),
            Character.BigC => new Rectangle(1, 0, 4, 8),
            Character.BigD => new Rectangle(1, 0, 5, 8),
            Character.BigE => new Rectangle(1, 0, 5, 8),
            Character.BigF => new Rectangle(1, 0, 5, 8),
            Character.BigG => new Rectangle(1, 0, 5, 8),
            Character.BigH => new Rectangle(1, 0, 5, 8),
            Character.BigI => new Rectangle(1, 0, 1, 8),
            Character.BigJ => new Rectangle(1, 0, 2, 8),
            Character.BigK => new Rectangle(1, 0, 4, 8),
            Character.BigL => new Rectangle(1, 0, 4, 8),
            Character.BigM => new Rectangle(0, 0, 6, 8),
            Character.BigN => new Rectangle(1, 0, 5, 8),
            Character.BigO => new Rectangle(1, 0, 5, 8),
            Character.BigP => new Rectangle(1, 0, 5, 8),
            Character.BigQ => new Rectangle(1, 0, 5, 8),
            Character.BigR => new Rectangle(1, 0, 5, 8),
            Character.BigS => new Rectangle(1, 0, 5, 8),
            Character.BigT => new Rectangle(1, 0, 5, 8),
            Character.BigU => new Rectangle(1, 0, 5, 8),
            Character.BigV => new Rectangle(1, 0, 5, 8),
            Character.BigW => new Rectangle(0, 0, 7, 8),
            Character.BigX => new Rectangle(1, 0, 5, 8),
            Character.BigY => new Rectangle(1, 0, 5, 8),
            Character.BigZ => new Rectangle(1, 0, 5, 8),
            Character.Exclamation => new Rectangle(1, 0, 1, 8),
            Character.Question => new Rectangle(1, 0, 5, 8),
            Character.Slash => new Rectangle(2, 0, 4, 8),
            Character.Minus => new Rectangle(2, 4, 4, 4),
            Character.SmallerAs => new Rectangle(1, 1, 3, 6),
            Character.Equal => new Rectangle(1, 2, 6, 5),
            Character.BiggerAs => new Rectangle(2, 1, 3, 6),
            Character.Asterisk => new Rectangle(1, 1, 5, 6),
            Character.Plus => new Rectangle(1, 1, 5, 6),
            Character.Percent => new Rectangle(0, 0, 8, 8),
            Character.OpenBracket => new Rectangle(1, 0, 2, 8),
            Character.CloseBracket => new Rectangle(1, 0, 2, 8),
            Character.Semicolon => new Rectangle(1, 2, 2, 6),
            Character.Dot => new Rectangle(0, 6, 1, 2),
            Character.Space => new Rectangle(0, 0, 2, 0),
            Character.Checkmark => new Rectangle(0, 1, 8, 7),
            Character.Crossout => new Rectangle(0, 0, 7, 8),
            Character.Down => new Rectangle(0, 2, 8, 6),
            Character.Up => new Rectangle(0, 2, 8, 6),
            Character.Line => new Rectangle(0, 7, 8, 1),
            Character.DoubleDots => new Rectangle(3, 2, 1, 6),
            Character.Comma => new Rectangle(2, 4, 2, 3),
            Character.Left => new Rectangle(2, 0, 4, 8),
            Character.Right => new Rectangle(2, 0, 4, 8),
            Character.Parentheses => new Rectangle(1, 0, 5, 8),
            Character.Backslash => new Rectangle(2, 0, 4, 8),
            Character.SmallA => new Rectangle(1, 2, 5, 6),
            Character.SmallB => new Rectangle(1, 0, 4, 8),
            Character.SmallC => new Rectangle(1, 2, 3, 6),
            Character.SmallD => new Rectangle(1, 0, 4, 8),
            Character.SmallE => new Rectangle(1, 2, 4, 6),
            Character.SmallF => new Rectangle(1, 0, 3, 8),
            Character.SmallG => new Rectangle(1, 2, 4, 6),
            Character.SmallH => new Rectangle(1, 0, 4, 8),
            Character.SmallI => new Rectangle(1, 0, 1, 8),
            Character.SmallJ => new Rectangle(1, 0, 2, 8),
            Character.SmallK => new Rectangle(1, 0, 3, 8),
            Character.SmallL => new Rectangle(1, 0, 2, 8),
            Character.SmallM => new Rectangle(1, 2, 5, 6),
            Character.SmallN => new Rectangle(1, 2, 4, 6),
            Character.SmallO => new Rectangle(1, 2, 4, 6),
            Character.SmallP => new Rectangle(1, 2, 4, 6),
            Character.SmallQ => new Rectangle(1, 2, 4, 6),
            Character.SmallR => new Rectangle(1, 2, 3, 6),
            Character.SmallS => new Rectangle(1, 2, 4, 6),
            Character.SmallT => new Rectangle(1, 0, 3, 8),
            Character.SmallU => new Rectangle(1, 2, 4, 6),
            Character.SmallV => new Rectangle(1, 2, 5, 6),
            Character.SmallW => new Rectangle(1, 2, 5, 6),
            Character.SmallX => new Rectangle(1, 2, 5, 6),
            Character.SmallY => new Rectangle(1, 1, 4, 6),
            Character.SmallZ => new Rectangle(1, 2, 5, 6),
            Character.OpenSquaredBrackets => new Rectangle(1, 0, 2, 8),
            Character.CloseSquaredBrackets => new Rectangle(1, 0, 2, 8),
            Character.Bean => new Rectangle(1,0,7,8),
            _ => new Rectangle(0, 0, 8, 8)
        };
    }

    public enum Character
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        BigA,
        BigB,
        BigC,
        BigD,
        BigE,
        BigF,
        BigG,
        BigH,
        BigI,
        BigJ,
        BigK,
        BigL,
        BigM,
        BigN,
        BigO,
        BigP,
        BigQ,
        BigR,
        BigS,
        BigT,
        BigU,
        BigV,
        BigW,
        BigX,
        BigY,
        BigZ,
        Exclamation,
        Question,
        Slash,
        Minus,
        SmallerAs,
        Equal,
        BiggerAs,
        Asterisk,
        Plus,
        Percent,
        OpenBracket,
        CloseBracket,
        Semicolon,
        Dot,
        Space,
        Checkmark,
        Crossout,
        Down,
        Up,
        Full,
        Line,
        DoubleDots,
        Comma,
        Left,
        Right,
        Parentheses,
        Backslash,
        LockLocked,
        LockUnlocked,
        AmongUsBean,
        SmallA,
        SmallB,
        SmallC,
        SmallD,
        SmallE,
        SmallF,
        SmallG,
        SmallH,
        SmallI,
        SmallJ,
        SmallK,
        SmallL,
        SmallM,
        SmallN,
        SmallO,
        SmallP,
        SmallQ,
        SmallR,
        SmallS,
        SmallT,
        SmallU,
        SmallV,
        SmallW,
        SmallX,
        SmallY,
        SmallZ,
        OpenSquaredBrackets,
        CloseSquaredBrackets,
        Bean
    }

    public static Character Parse(char character)
        => character switch
        {
            '0' => Character.Zero,
            '1' => Character.One,
            '2' => Character.Two,
            '3' => Character.Three,
            '4' => Character.Four,
            '5' => Character.Five,
            '6' => Character.Six,
            '7' => Character.Seven,
            '8' => Character.Eight,
            '9' => Character.Nine,
            'a' => Character.SmallA,
            'A' => Character.BigA,
            'b' => Character.SmallB,
            'B' => Character.BigB,
            'c' => Character.SmallC,
            'C' => Character.BigC,
            'd' => Character.SmallD,
            'D' => Character.BigD,
            'e' => Character.SmallE,
            'E' => Character.BigE,
            'f' => Character.SmallF,
            'F' => Character.BigF,
            'g' => Character.SmallG,
            'G' => Character.BigG,
            'h' => Character.SmallH,
            'H' => Character.BigH,
            'i' => Character.SmallI,
            'I' => Character.BigI,
            'j' => Character.SmallJ,
            'J' => Character.BigJ,
            'k' => Character.SmallK,
            'K' => Character.BigK,
            'l' => Character.SmallL,
            'L' => Character.BigL,
            'm' => Character.SmallM,
            'M' => Character.BigM,
            'n' => Character.SmallN,
            'N' => Character.BigN,
            'o' => Character.SmallO,
            'O' => Character.BigO,
            'p' => Character.SmallP,
            'P' => Character.BigP,
            'q' => Character.SmallQ,
            'Q' => Character.BigQ,
            'r' => Character.SmallR,
            'R' => Character.BigR,
            's' => Character.SmallS,
            'S' => Character.BigS,
            't' => Character.SmallT,
            'T' => Character.BigT,
            'u' => Character.SmallU,
            'U' => Character.BigU,
            'v' => Character.SmallV,
            'V' => Character.BigV,
            'w' => Character.SmallW,
            'W' => Character.BigW,
            'x' => Character.SmallX,
            'X' => Character.BigX,
            'y' => Character.SmallY,
            'Y' => Character.BigY,
            'z' => Character.SmallZ,
            'Z' => Character.BigZ,
            '!' => Character.Exclamation,
            '?' => Character.Question,
            '/' => Character.Slash,
            '-' => Character.Minus,
            '<' => Character.SmallerAs,
            '=' => Character.Equal,
            '>' => Character.BiggerAs,
            '*' => Character.Asterisk,
            '+' => Character.Plus,
            '%' => Character.Percent,
            '(' => Character.OpenBracket,
            ')' => Character.CloseBracket,
            ';' => Character.Semicolon,
            '.' => Character.Dot,
            ' ' => Character.Space,
            Checkmark => Character.Checkmark,
            Crossout => Character.Crossout,
            '⬇' => Character.Down,
            '⬆' => Character.Up,
            '_' => Character.Line,
            ':' => Character.DoubleDots,
            ',' => Character.Comma,
            '⬅' => Character.Left,
            '➡' => Character.Right,
            '\"' => Character.Parentheses,
            '\\' => Character.Backslash,
            Full => Character.Full,
            LockLocked => Character.LockLocked,
            LockUnlocked => Character.LockUnlocked,
            AmongUsBean => Character.AmongUsBean,
            '[' => Character.OpenSquaredBrackets,
            ']' => Character.CloseSquaredBrackets,
            Bean => Character.Bean,
            _ => Character.Full,
        };

    public static char ReverseParse(Character character)
        => character switch
        {
            Character.Zero => '0',
            Character.One => '1',
            Character.Two => '2',
            Character.Three => '3',
            Character.Four => '4',
            Character.Five => '5',
            Character.Six => '6',
            Character.Seven => '7',
            Character.Eight => '8',
            Character.Nine => '9',
            Character.BigA => 'A',
            Character.BigB => 'B',
            Character.BigC => 'C',
            Character.BigD => 'D',
            Character.BigE => 'E',
            Character.BigF => 'F',
            Character.BigG => 'G',
            Character.BigH => 'H',
            Character.BigI => 'I',
            Character.BigJ => 'J',
            Character.BigK => 'K',
            Character.BigL => 'L',
            Character.BigM => 'M',
            Character.BigN => 'N',
            Character.BigO => 'O',
            Character.BigP => 'P',
            Character.BigQ => 'Q',
            Character.BigR => 'R',
            Character.BigS => 'S',
            Character.BigT => 'T',
            Character.BigU => 'U',
            Character.BigV => 'V',
            Character.BigW => 'W',
            Character.BigX => 'X',
            Character.BigY => 'Y',
            Character.BigZ => 'Z',
            Character.SmallA => 'a',
            Character.SmallB => 'b',
            Character.SmallC => 'c',
            Character.SmallD => 'd',
            Character.SmallE => 'e',
            Character.SmallF => 'f',
            Character.SmallG => 'g',
            Character.SmallH => 'h',
            Character.SmallI => 'i',
            Character.SmallJ => 'j',
            Character.SmallK => 'k',
            Character.SmallL => 'l',
            Character.SmallM => 'm',
            Character.SmallN => 'n',
            Character.SmallO => 'o',
            Character.SmallP => 'p',
            Character.SmallQ => 'q',
            Character.SmallR => 'r',
            Character.SmallS => 's',
            Character.SmallT => 't',
            Character.SmallU => 'u',
            Character.SmallV => 'v',
            Character.SmallW => 'w',
            Character.SmallX => 'x',
            Character.SmallY => 'y',
            Character.SmallZ => 'z',
            Character.Exclamation => '!',
            Character.Question => '?',
            Character.Slash => '/',
            Character.Minus => '-',
            Character.SmallerAs => '<',
            Character.Equal => '=',
            Character.BiggerAs => '>',
            Character.Asterisk => '*',
            Character.Plus => '+',
            Character.Percent => '%',
            Character.OpenBracket => '(',
            Character.CloseBracket => ')',
            Character.Semicolon => ';',
            Character.Dot => '.',
            Character.Space => ' ',
            Character.Checkmark => Checkmark,
            Character.Crossout => Crossout,
            Character.Down => '⬇',
            Character.Up => '⬆',
            Character.Line => '_',
            Character.DoubleDots => ':',
            Character.Comma => ',',
            Character.Left => '⬅',
            Character.Right => '➡',
            Character.Parentheses => '\"',
            Character.Backslash => '\\',
            Character.Full => Full,
            Character.LockLocked => LockLocked,
            Character.LockUnlocked => LockUnlocked,
            Character.AmongUsBean => AmongUsBean,
            Character.OpenSquaredBrackets => '[',
            Character.CloseSquaredBrackets => ']',
            Character.Bean => Bean,
            _ => '⬜'
        };

    public Vector2 GetPosition()
        => Position;

    public Vector2 GetSize()
        => Size;

    public void Move(Vector2 newPosition)
    {
        Position = newPosition;
        UpdateRectangle();
    }
}