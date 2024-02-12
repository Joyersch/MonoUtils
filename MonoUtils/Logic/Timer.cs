﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;

namespace MonoUtils.Logic;

public class Timer : IManageable, IMoveable, IColorable
{
    private readonly OverTimeInvoker _invoker;
    private readonly Ui.Objects.TextSystem.Text _display;

    private readonly double _time;
    private bool _invoked;

    public event Action Trigger;

    public Timer(double time, bool start = false) : this(3F, time, start)
    {
    }

    public Timer(float scale, double time, bool start = false) : this(Vector2.Zero, scale, time, start)
    {
    }

    public Timer(Vector2 position, float scale, double time, bool start = false)
    {
        _time = time;
        _invoker = new OverTimeInvoker(time, start)
        {
            InvokeOnce = true
        };
        _invoker.Trigger += delegate
        {
            _invoked = true;
            Trigger?.Invoke();
        };

        _display = new Ui.Objects.TextSystem.Text($"{time/1000:n2}", position, scale);
    }

    public Rectangle Rectangle => _invoker.Rectangle;

    public void Update(GameTime gameTime)
    {
        if (_invoked)
        {
            _display.ChangeText("0.00");
        }
        else
        {
            var difference = (_time - _invoker.ExecutedTime) / 1000;
            _display.ChangeText($"{difference:n2}");
        }

        _invoker.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _display.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => _display.GetPosition();

    public Vector2 GetSize()
        => _display.GetSize();

    public void Move(Vector2 newPosition)
        => _display.Move(newPosition);

    public void Start()
        => _invoker.Start();

    public void ChangeColor(Color[] input)
        => _display.ChangeColor(input);

    public int ColorLength()
        => _display.ColorLength();

    public void Stop()
    {
        _invoked = false;
        _invoker.Stop();
    }

    public void Reset()
    {
        _invoked = false;
        _invoker.Reset();
    }
}