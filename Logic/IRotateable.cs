using Microsoft.Xna.Framework;

namespace MonoUtils.Logic;

public interface IRotateable: ISpatial
{
    public float Rotation { get; set; }
}