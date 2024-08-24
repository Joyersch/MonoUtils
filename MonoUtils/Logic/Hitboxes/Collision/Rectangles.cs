/*
	One-Size-Fits-All Rectangle Vs Rectangle Collisions
	"Stupid scanners... making me miss at archery..." - javidx9
	License (OLC-3)
	~~~~~~~~~~~~~~~
	Copyright 2018-2020 OneLoneCoder.com
	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions
	are met:
	1. Redistributions or derivations of source code must retain the above
	copyright notice, this list of conditions and the following disclaimer.
	2. Redistributions or derivative works in binary form must reproduce
	the above copyright notice. This list of conditions and the following
	disclaimer must be reproduced in the documentation and/or other
	materials provided with the distribution.
	3. Neither the name of the copyright holder nor the names of its
	contributors may be used to endorse or promote products derived
	from this software without specific prior written permission.
	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
	"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
	LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
	A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
	HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
	LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
	DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
	THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
	OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
	Relevant Video: https://www.youtube.com/watch?v=8JJ-4JgR7Dg
	Links
	~~~~~
	YouTube:	https://www.youtube.com/javidx9
				https://www.youtube.com/javidx9extra
	Discord:	https://discord.gg/WhwHUMV
	Twitter:	https://www.twitter.com/javidx9
	Twitch:		https://www.twitch.tv/javidx9
	GitHub:		https://www.github.com/onelonecoder
	Patreon:	https://www.patreon.com/javidx9
	Homepage:	https://www.onelonecoder.com
	Community Blog: https://community.onelonecoder.com
	Author
	~~~~~~
	David Barr, aka javidx9, ©OneLoneCoder 2018, 2019, 2020
*/
using Microsoft.Xna.Framework;

namespace MonoUtils.Logic.Hitboxes.Collision;

public sealed class Rectangles
{
    public static bool RayVsRectangle(Vector2 rayOrigin, Vector2 rayDirection, Rectangle target, out Vector2 contactPoint,
        out Vector2 contactNormal, out float timeHitNear)
    {
        contactPoint = new Vector2();
        contactNormal = new Vector2();

        timeHitNear = 0;

        Vector2 invertedDirection = Vector2.Divide(new(1f, 1f), rayDirection);


        Vector2 timeNear = (target.Location.ToVector2() - rayOrigin) * invertedDirection;

        Vector2 timeFar = (target.Location.ToVector2() + target.Size.ToVector2() - rayOrigin) * invertedDirection;

        if (float.IsNaN(timeNear.X) || float.IsNaN(timeFar.X))
            return false;

        if (float.IsNaN(timeNear.Y) || float.IsNaN(timeFar.Y))
            return false;

        #region SortDistance

        Vector2 bufferNear = timeNear;
        Vector2 bufferFar = timeFar;
        if (timeNear.X > timeFar.X)
        {
            bufferNear.X = timeFar.X;
            bufferFar.X = timeNear.X;
        }

        if (timeNear.Y > timeFar.Y)
        {
            bufferNear.Y = timeFar.Y;
            bufferFar.Y = timeNear.Y;
        }

        timeNear = bufferNear;
        timeFar = bufferFar;

        #endregion SortDistance

        if (timeNear.X > timeFar.Y || timeNear.Y > timeFar.X)
            return false;

        timeHitNear = Math.Max(timeNear.X, timeNear.Y);

        float timeHitFar = Math.Min(timeFar.X, timeFar.Y);


        if (timeHitFar < 0)
            return false;

        contactPoint = rayOrigin + timeHitNear * rayDirection;

        if (timeNear.X > timeNear.Y)
        {
            if (rayDirection.X < 0)
                contactNormal = new Vector2(1, 0);
            else
                contactNormal = new Vector2(-1, 0);
        }
        else if (timeNear.X < timeNear.Y)
        {
            if (rayDirection.Y < 0)
                contactNormal = new Vector2(0, 1);
            else
                contactNormal = new Vector2(0, -1);
        }

        return true;
    }

    public static bool DynamicRectangleVsRectangle(Rectangle dynamic, Vector2 velocity, float timeStep,
        Rectangle @static,
        out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime)
    {
        contactPoint = new Vector2();
        contactNormal = new Vector2();
        contactTime = 0;

        if (velocity.X == 0 && velocity.Y == 0)
            return false;

        Rectangle expandedTarget = new Rectangle();
        expandedTarget.Location = (@static.Location.ToVector2() - dynamic.Size.ToVector2() / 2).ToPoint();
        expandedTarget.Size = (@static.Size.ToVector2() * 2).ToPoint();
        if (RayVsRectangle(dynamic.Location.ToVector2() + dynamic.Size.ToVector2() / 2,
                velocity * timeStep, expandedTarget, out contactPoint, out contactNormal,
                out contactTime))
            return (contactTime >= 0.0f && contactTime < 1.0f);
        return false;
    }
}